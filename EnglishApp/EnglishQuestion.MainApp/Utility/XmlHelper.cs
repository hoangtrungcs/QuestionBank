using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;
using EnglishQuestion.Entity.MetaData;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;

namespace EnglishQuestion.MainApp.Utility
{
    public static class XmlHelper
    {
        private static readonly int PadRightLength = 30;
        private static readonly char PadRightLetter = '@';
        private static readonly string HtmlSpace = "&nbsp;";

        public static XElement BuildStructureSubTest(Dictionary<string, ParagraphMeta> sections, Test test)
        {
            var configSections = XElement.Parse(test.ConfigStructure).Descendants("Section");

            var element = sections.Select(x =>
                new XElement("Section", new XAttribute("Id", x.Key),
                    new XElement("Content", new XCData(DbHelper.Instance.GetB1B2ConfigValue(GetConfigSectionKey(configSections, x.Key).ToEmpty()) + GetListeningPath(x.Value.IsListening, x.Value.AudioFilePath))),
                    new XElement("Paragraph", new XAttribute("Id", x.Value.Id),
                        new XElement("Content", new XCData(DbHelper.Instance.GetParagraphContent(x.Value.Id).ToEmpty())),
                        x.Value.QuestionMeta.Select(y =>
                            new XElement("Question", new XAttribute("Id", y.Id),
                                new XElement("Content", new XCData(DbHelper.Instance.GetQuestionContent(y.Id).ToEmpty())),
                                new XElement("Answers", y.CanMixAnswers ? RandomAnswers(y.AnswerMeta) 
                                    :
                                    y.AnswerMeta.Select(z =>
                                        new XElement("Answer", new XAttribute("Id", z.Id),
                                                               new XAttribute("IsAnswer", z.IsAnswer),
                                                               new XCData(DbHelper.Instance.GetAnswerContent(z.Id).ToEmpty()))
                                    )
                                )
                            )
                        )
                    )
                )
            );

            return new XElement("Test", new XAttribute("Id", test.Level), element);
        }

        public static XElement BuildConfigStructure(GenerateBaseVM model)
        {
            var element = model.ConfigLevels.Select(x =>
                new XElement("Section",
                    new XElement("Id", x.Section),
                    new XElement("TemplateKey", x.Templatekey),
                    new XElement("NumOfQuestion", x.NumOfQuestion),
                    new XElement("QuestionLevel", x.QuestionLevel),
                    new XElement("TimeDone", x.TimeDone)));
            return new XElement("Config", element);
        }

        public static string BuildTestFromXml(string xmlContent)
        {
            string[] answerLeters = { "A", "B", "C", "D" };
            int number = 0;
            var contentBuilder = new StringBuilder();
            var element = XElement.Parse(xmlContent);
            var sections = element.Descendants("Section");

            foreach (var section in sections)
            {
                contentBuilder.AppendLine(section.Element("Content")?.Value);

                var paragraphElement = section.Element("Paragraph");
                var questionElements = paragraphElement.Descendants("Question");
                
                int paragraphId = Convert.ToInt32(paragraphElement.Attribute("Id").Value);
                if (paragraphId > 1)
                {
                    // Paragraph
                    var paragraph = DbHelper.Instance.GetParagraphById(paragraphId);

                    string pattern = $"{Regex.Escape("[")}{"\\d+"}{Regex.Escape("]")}";
                    var tmp = number;
                    paragraph.Content = Regex.Replace(paragraph.Content, pattern, match => $"[{++tmp}]");
                    contentBuilder.AppendFormat("<p>{0}</p>", paragraph.Content).AppendLine();

                    if (paragraph.Type == QuestionType.WPA.ToString())
                    {
                        number = tmp;
                        continue;
                    }

                    foreach (var questionElement in questionElements)
                    {
                        var questionId = Convert.ToInt32(questionElement.Attribute("Id").Value);
                        // Question
                        var question = paragraph.Questions.First(x => x.Id == questionId);
                        contentBuilder.AppendFormat("<p>{0}. {1}</p>", ++number, paragraph.Type == QuestionType.RPA.ToString() ? string.Empty : question.Content).AppendLine();
                        
                        // Answer
                        if (question.Type == QuestionType.WQA.ToString()
                            || question.Type == QuestionType.WPA.ToString()
                            || question.Type == QuestionType.LQA1.ToString())
                        {
                            // Dont generate answer for writing and listening
                            continue;
                        }

                        var answerElements = questionElement.Descendants("Answer").ToList();
                        var answerContent = string.Empty;
                        var maxAnswer = question.Answers.Max(x => x.Content.Length);
                        if (maxAnswer <= PadRightLength)
                        {
                            answerContent += "<p>";
                            for (int i = 0; i < answerElements.Count; i++)
                            {
                                int answerId = Convert.ToInt32(answerElements[i].Attribute("Id").Value);
                                answerContent += $"{answerLeters[i]}. {question.Answers.First(x => x.Id == answerId).Content.PadRight(PadRightLength, PadRightLetter)}";
                            }
                            answerContent = answerContent.TrimEnd(PadRightLetter);
                            answerContent += "</p>";
                        }
                        else if (maxAnswer <= PadRightLength*2)
                        {
                            for (int i = 0; i < answerElements.Count; i++)
                            {
                                if (i == 0 || i == 2)
                                {
                                    answerContent += "<p>";
                                }
                                int answerId = Convert.ToInt32(answerElements[i].Attribute("Id").Value);
                                answerContent += $"{answerLeters[i]}. {question.Answers.First(x => x.Id == answerId).Content.PadRight(PadRightLength*2, PadRightLetter)}";
                                if (i == 1 || i == 3)
                                {
                                    answerContent = answerContent.TrimEnd(PadRightLetter);
                                    answerContent += "</p>";
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < answerElements.Count; i++)
                            {
                                int answerId = Convert.ToInt32(answerElements[i].Attribute("Id").Value);
                                answerContent += $"<p>{answerLeters[i]}. {question.Answers.First(x => x.Id == answerId).Content.TrimEnd(PadRightLetter)}</p>";
                            }
                        }
                        answerContent = answerContent.Replace(PadRightLetter.ToString(), HtmlSpace);
                        contentBuilder.Append(answerContent).AppendLine();
                    }
                } // end if (paragraphId > 1)
                else
                {
                    foreach (var questionElement in questionElements)
                    {
                        var questionId = Convert.ToInt32(questionElement.Attribute("Id").Value);
                        // Question
                        var question = DbHelper.Instance.GetQuestionById(questionId);
                        contentBuilder.AppendFormat("<p>{0}. {1}</p>", ++number, question.Content).AppendLine();

                        // Answer
                        if (question.Type == QuestionType.WQA.ToString()
                            || question.Type == QuestionType.WPA.ToString()
                            || question.Type == QuestionType.LQA1.ToString())
                        {
                            // Dont generate answer for writing and listening
                            continue;
                        }

                        var answerElements = questionElement.Descendants("Answer").ToList();
                        var answerContent = string.Empty;
                        var maxAnswer = question.Answers.Max(x => x.Content.Length);
                        if (maxAnswer <= PadRightLength)
                        {
                            answerContent += "<p>";
                            for (int i = 0; i < answerElements.Count; i++)
                            {
                                int answerId = Convert.ToInt32(answerElements[i].Attribute("Id").Value);
                                answerContent +=
                                    $"{answerLeters[i]}. {question.Answers.First(x => x.Id == answerId).Content.PadRight(PadRightLength, PadRightLetter)}";
                            }
                            answerContent = answerContent.TrimEnd(PadRightLetter);
                            answerContent += "</p>";
                        }
                        else if (maxAnswer <= PadRightLength * 2)
                        {
                            for (int i = 0; i < answerElements.Count; i++)
                            {
                                if (i == 0 || i == 2)
                                {
                                    answerContent += "<p>";
                                }
                                int answerId = Convert.ToInt32(answerElements[i].Attribute("Id").Value);
                                answerContent += $"{answerLeters[i]}. {question.Answers.First(x => x.Id == answerId).Content.PadRight(PadRightLength*2, PadRightLetter)}";
                                if (i == 1 || i == 3)
                                {
                                    answerContent = answerContent.TrimEnd(PadRightLetter);
                                    answerContent += "</p>";
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < answerElements.Count; i++)
                            {
                                int answerId = Convert.ToInt32(answerElements[i].Attribute("Id").Value);
                                answerContent += $"<p>{answerLeters[i]}. {question.Answers.First(x => x.Id == answerId).Content.TrimEnd(PadRightLetter)}</p>";
                            }
                        }
                        answerContent = answerContent.Replace(PadRightLetter.ToString(), HtmlSpace);
                        contentBuilder.Append(answerContent).AppendLine();
                    }
                }
                contentBuilder.AppendLine("<br/>");
            }

            return contentBuilder.ToString();
        }

        public static string BuildTestResult(string xmlStructure)
        {
            int count = 1;
            var builder = new StringBuilder();
            var element = XElement.Parse(xmlStructure);
            var sections = element.Descendants("Section");

            foreach (var section in sections)
            {
                builder.Append(section.Element("Content")?.Value).Append("<br/>");

                var questions = section.Descendants("Question");
                foreach (var question in questions)
                {
                    builder.Append($"{count++}. {question.Element("Content")?.Value}").Append("<br/>");

                    var answers = question.Descendants("Answer");
                    var rightAnswer = answers.Count() > 1 ? answers.FirstOrDefault(x => x.Attribute("IsAnswer").Value == "true")
                                        : answers.FirstOrDefault();

                    builder.Append($"<font color='blue'>{rightAnswer?.Value}</font>").Append("<br/>");
                }

                builder.Append("<br/>");
            }

            return builder.ToString();
        }

        public static string BuildTestHeader(TestLevel level, string type, SubTest subTest, Test test)
        {
            switch (level)
            {
                case TestLevel.GLevelA:
                case TestLevel.GcLevelA:
                    return (type == SubTestType.Writing.ToString())
                        ? string.Format(AppCommonResource.LevelAWritingHeader, subTest.Name, test.CreatedDate, test.TotalTime)
                        : string.Format(AppCommonResource.LevelAListeningHeader, test.CreatedDate);

                case TestLevel.GLevelB:
                case TestLevel.GcLevelB:
                    return string.Format(AppCommonResource.LevelBWritingHeader, test.TotalTime, subTest.Name);

                case TestLevel.GLevelC:
                case TestLevel.GcLevelC:
                    return string.Format(AppCommonResource.LevelCHeader, test.CreatedDate, test.TotalTime);

                default:
                    return string.Empty;
            }
        }

        private static string GetConfigSectionKey(IEnumerable<XElement> configs, string sectionId)
        {
            if (configs == null) return string.Empty;

            var config = configs.FirstOrDefault(x => x.Element("Id").Value == sectionId);
            return (config != null) ? config.Element("TemplateKey").Value : string.Empty;
        }

        private static string GetListeningPath(bool isListening, string filePath)
        {
            if (isListening)
            {
                return $" (<font color='blue'>{filePath}</font>)";
            }

            return String.Empty;
        }

        private static IEnumerable<XElement> RandomAnswers(List<AnswerMeta> answerMeta)
        {
            var result = new List<XElement>();
            var answers = answerMeta.OrderBy(z => Guid.NewGuid()).ToList();

            var answerIds = new List<int>();

            foreach (var answer in answers)
            {
                var content = DbHelper.Instance.GetAnswerContent(answer.Id).ToEmpty();
                result.Add(new XElement("Answer", new XAttribute("Id", answer.Id),
                                                    new XAttribute("IsAnswer", answer.IsAnswer),
                                                    new XCData(content)));

                if (content.ToLower().Contains("all")
                    || content.ToLower().Contains("đều đúng")
                    || content.ToLower().Contains("đều sai")
                    || content.ToLower().Contains("are correct")
                    || content.ToLower().Contains("are wrong")
                    || content.ToLower().Contains("are not correct")
                    || content.ToLower().Contains("a,b,c")
                    || content.ToLower().Contains("a, b, c"))
                {
                    answerIds.Add(answer.Id);
                }
            }

            if (answerIds.Count > 0)
            {
                var a = result.FirstOrDefault(x => Convert.ToInt32(x.Attribute("Id").Value) == answerIds[0]);
                result.Remove(a);
                result.Add(a);
            }

            return result;
        } 
    }
}
