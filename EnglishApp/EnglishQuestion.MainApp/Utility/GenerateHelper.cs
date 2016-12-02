using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;
using EnglishQuestion.Entity.MetaData;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;

namespace EnglishQuestion.MainApp.Utility
{
    public static class GenerateHelper
    {
        public static Test GenerateTest(GenerateBaseVM model)
        {
            // 1. Get Ids NumOfTest * NumOfQuestion from DB
            // 2. Loop NumOfTest
            // 3. Each NumOfTest, generate all test, random ids, remove from list if can
            // 4. Use something.OrderBy(x => Guid.NewGuid()) to select random row
            if (model.GenerateConfig.NumOfSubTests == 0)
            {
                throw new Exception(AppCommonResource.NumOfSubTestGreaterThan0);
            }

            var subTestMetaList = new List<SubTestMeta>();
            for (int i = 0; i < model.GenerateConfig.NumOfSubTests; i++)
            {
                subTestMetaList.Add(new SubTestMeta());
            }

            foreach (var config in model.ConfigLevels)
            {
                //config.Templatekey: template key
                if (config.IsManual)
                {
                    for (int i = 0; i < model.GenerateConfig.NumOfSubTests; i++)
                    {
                        if (config.IsLitening)
                        {
                            config.ParagraphMeta.IsListening = true;
                            subTestMetaList[i].ListeningParagraphMetas.Add(config.Section, config.ParagraphMeta);
                        }
                        else
                        {
                            subTestMetaList[i].WritingParagraphMetas.Add(config.Section, config.ParagraphMeta);
                        }
                    }
                    continue;
                }

                if (config.IsParagraph)
                {
                    var paragraphMetaList = DbHelper.Instance.GetParagraphMetaOfSection(
                                                model.GenerateConfig.TestLevel.GetSubTypeFromTestLevel(),
                                                config.QuestionLevel,
                                                model.GenerateConfig.Purpose,
                                                config.Section);
                    if (paragraphMetaList.Count == 0 || paragraphMetaList.Count < config.NumOfQuestion)
                    {
                        throw new Exception(AppCommonResource.NotEnoughQuestion);
                    }

                    bool isRemove = paragraphMetaList.Count > config.NumOfQuestion*model.GenerateConfig.NumOfSubTests;
                    for (int i = 0; i < model.GenerateConfig.NumOfSubTests; i++)
                    {
                        if (config.IsLitening)
                        {
                            var listeningParagraph = paragraphMetaList.OrderBy(x => Guid.NewGuid()).First();
                            listeningParagraph.IsListening = true;
                            subTestMetaList[i].ListeningParagraphMetas.Add(config.Section, listeningParagraph);
                        }
                        else
                        {
                            subTestMetaList[i].WritingParagraphMetas.Add(config.Section,
                                paragraphMetaList.OrderBy(x => Guid.NewGuid()).First());
                        }
                        if (isRemove)
                        {
                            paragraphMetaList.Remove(config.IsLitening
                                ? subTestMetaList[i].ListeningParagraphMetas[config.Section]
                                : subTestMetaList[i].WritingParagraphMetas[config.Section]);
                        }
                    }
                }
                else
                {
                    var paragraphMeta = DbHelper.Instance.GetQuestionMetaOfSection(
                                        model.GenerateConfig.TestLevel.GetSubTypeFromTestLevel(),
                                        config.QuestionLevel,
                                        model.GenerateConfig.Purpose,
                                        config.Section);
                    if (paragraphMeta.QuestionMeta.Count == 0 || paragraphMeta.QuestionMeta.Count < config.NumOfQuestion)
                    {
                        throw new Exception(AppCommonResource.NotEnoughQuestion);
                    }
                    bool isRemove = paragraphMeta.QuestionMeta.Count > config.NumOfQuestion*model.GenerateConfig.NumOfSubTests;
                    for (int i = 0; i < model.GenerateConfig.NumOfSubTests; i++)
                    {
                        if (config.IsLitening)
                        {
                            subTestMetaList[i].ListeningParagraphMetas.Add(config.Section, new ParagraphMeta()
                            {
                                Id = 1,
                                QuestionMeta = paragraphMeta.QuestionMeta.OrderBy(x => Guid.NewGuid()).Take(config.NumOfQuestion).ToList()
                            });
                            subTestMetaList[i].ListeningParagraphMetas[config.Section].TimeDone =
                                subTestMetaList[i].ListeningParagraphMetas[config.Section].QuestionMeta.Sum(x => x.TimeDone);

                            if (isRemove)
                            {
                                foreach (var qMeta in subTestMetaList[i].ListeningParagraphMetas[config.Section].QuestionMeta)
                                {
                                    var removeItem = paragraphMeta.QuestionMeta.First(x => x.Id == qMeta.Id);
                                    paragraphMeta.QuestionMeta.Remove(removeItem);
                                }
                            }
                        }
                        else
                        {
                            subTestMetaList[i].WritingParagraphMetas.Add(config.Section, new ParagraphMeta()
                            {
                                Id = 1,
                                QuestionMeta = paragraphMeta.QuestionMeta.OrderBy(x => Guid.NewGuid()).Take(config.NumOfQuestion).ToList()
                            });
                            subTestMetaList[i].WritingParagraphMetas[config.Section].TimeDone =
                                subTestMetaList[i].WritingParagraphMetas[config.Section].QuestionMeta.Sum(x => x.TimeDone);

                            if (isRemove)
                            {
                                foreach (var qMeta in subTestMetaList[i].WritingParagraphMetas[config.Section].QuestionMeta)
                                {
                                    var removeItem = paragraphMeta.QuestionMeta.First(x => x.Id == qMeta.Id);
                                    paragraphMeta.QuestionMeta.Remove(removeItem);
                                }
                            }
                        }
                    }
                }
            }

            var test = new Test()
            {
                Name = model.GenerateConfig.TestName,
                ClassNo = model.GenerateConfig.ClassNo,
                Level = model.GenerateConfig.TestLevel.GetSubTypeFromTestLevel(),
                TotalTime = model.GenerateConfig.TotalTime,
                TotalQuestion = subTestMetaList[0].WritingParagraphMetas.Sum(x => x.Value.QuestionMeta.Count),
                NumOfSubTest = model.GenerateConfig.NumOfSubTests,
                Purpose = model.GenerateConfig.Purpose,
                ConfigStructure = XmlHelper.BuildConfigStructure(model).ToString(),
                SubTests = new ObservableCollection<SubTest>(),
                RealTestTime = subTestMetaList[0].WritingParagraphMetas.Sum(x => x.Value.TimeDone),
                TestDate = model.GenerateConfig.TestDate,
                IsChoice = model.IsChoice,
                No = model.GenerateConfig.TestDate.ToString("yyyyMMddHHmmss")
            };
            int subTestCount = 0;
            foreach (var subTestMeta in subTestMetaList)
            {
                var subTest = new SubTest()
                {
                    Name = $"{AppCommonResource.SubTest} {++subTestCount}",
                    XmlWritingStructure = XmlHelper.BuildStructureSubTest(subTestMeta.WritingParagraphMetas, test).ToString(),
                    XmlListeningStructure = XmlHelper.BuildStructureSubTest(subTestMeta.ListeningParagraphMetas, test).ToString()
                };

                TestLevel level;
                TestLevel.TryParse(model.GenerateConfig.TestLevel, out level);

                var builder = new StringBuilder();
                builder.AppendLine(XmlHelper.BuildTestHeader(level, SubTestType.Writing.ToString(), subTest, test));
                builder.AppendLine(XmlHelper.BuildTestFromXml(subTest.XmlWritingStructure));
                subTest.WritingTestContent = builder.ToString();

                builder = new StringBuilder();
                builder.AppendLine(XmlHelper.BuildTestHeader(level, SubTestType.Listening.ToString(), subTest, test));
                builder.AppendLine(XmlHelper.BuildTestFromXml(subTest.XmlListeningStructure));
                subTest.ListeningTestContent = builder.ToString();

                test.SubTests.Add(subTest);
            }

            return test;
        }
    }
}
