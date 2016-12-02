/*
=========================================================================================================
  Module      : Database helper (DbHelper.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;
using EnglishQuestion.Entity.MetaData;

namespace EnglishQuestion.Service
{
    /// <summary>
    /// Database helper
    /// </summary>
    public class DbHelper
    {
        private static volatile DbHelper m_instance;
        private static object m_syncRoot = new object();

        public Paragraph DummyParagraph
        {
            get
            {
                using (var context = new EnglishQuestionContext())
                {
                    return context.Paragraphs.FirstOrDefault(x => x.Status == "Dummy");
                }
            }
        }

        public static DbHelper Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_syncRoot)
                    {
                        if (m_instance == null)
                            m_instance = new DbHelper();
                    }
                }

                return m_instance;
            }
        }

        public void SaveParagraph(IEnumerable<Paragraph> paragraphs)
        {
            using (var context = new EnglishQuestionContext())
            {
                foreach (var paragraph in paragraphs)
                {
                    if (paragraph.Id > 0)
                    {
                        foreach (var question in paragraph.Questions)
                        {
                            question.ParagraphId = paragraph.Id;
                            question.Paragraph = paragraph;
                            question.Id = 0;
                            context.Entry(question).State = EntityState.Added;

                            foreach (var answer in question.Answers)
                            {
                                context.Entry(answer).State = EntityState.Added;
                            }
                        }

                        context.Entry(paragraph).State = EntityState.Modified;
                    }
                    else
                    {
                        // Add
                        context.Paragraphs.Add(paragraph);
                    }
                }
                context.SaveChanges();
            }
        }

        public int SaveQuestion(IEnumerable<Question> questions)
        {
            using (var context = new EnglishQuestionContext())
            {
                foreach (var question in questions)
                {
                    context.Entry(question).State = (question.Id == 0) ? EntityState.Added : EntityState.Modified;

                    foreach (var answer in question.Answers)
                    {
                        context.Entry(answer).State = (answer.Id == 0) ? EntityState.Added : EntityState.Modified;
                    }
                }
                return context.SaveChanges();
            }
        }

        public IEnumerable<Paragraph> GetParagraph(string testLevel, string type)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Paragraphs.Include("Questions.Answers")
                              .Where(x => x.TestLevel.Contains(testLevel) && (x.Type == type))
                              .ToList();
            }
        }

        public IEnumerable<Question> GetQuestion(string testLevel, string type)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Questions.Include("Answers").Where(x => x.TestLevel.Contains(testLevel)
                                                                    && (x.Type == type)
                                                                    && (x.ParagraphId == DummyParagraph.Id)).ToList();
            }
        }

        public IEnumerable<Paragraph> SearchParagraphs(string testLevel, string section, string content, DateTime from, DateTime to)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Paragraphs.Include("Questions.Answers")
                              .Where(x => x.TestLevel.Contains(testLevel)
                                       && x.Section == section
                                       && x.Content.Contains(content)
                                       && x.CreatedDate >= from
                                       && x.CreatedDate <= to)
                              .ToList();
            }
        } 

        public IEnumerable<Question> SearchQuestion(string testLevel, string section, string content, DateTime from, DateTime to)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Questions.Include("Answers")
                              .Where(x => x.TestLevel.Contains(testLevel)
                                       && x.Section == section
                                       && x.Content.Contains(content)
                                       && x.CreatedDate >= from
                                       && x.CreatedDate <= to)
                              .ToList();
            }
        }

        public void DeleteParagraph(Paragraph paragraph)
        {
            using (var context = new EnglishQuestionContext())
            {
                foreach (var question in paragraph.Questions.ToList())
                {
                    foreach (var answer in question.Answers.ToList())
                    {
                        context.Entry(answer).State = EntityState.Deleted;
                    }
                    context.Entry(question).State = EntityState.Deleted;
                }
                context.Entry(paragraph).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void DeleteParagraph(int id)
        {
            using (var context = new EnglishQuestionContext())
            {
                var paragraph = context.Paragraphs.Include("Questions.Answers").FirstOrDefault(x => x.Id == id);
                if (paragraph != null)
                {
                    DeleteParagraph(paragraph);
                }
            }
        }

        public void DeleteQuestionsOfParagraph(int paragraphId)
        {
            using (var context = new EnglishQuestionContext())
            {
                var paragraph = context.Paragraphs.Include("Questions.Answers").FirstOrDefault(x => x.Id == paragraphId);
                if (paragraph != null)
                {
                    context.Questions.RemoveRange(paragraph.Questions);
                    context.SaveChanges();
                }
            }
        }

        public void DeleteQuestion(Question question)
        {
            using (var context = new EnglishQuestionContext())
            {
                // delete answer
                foreach (var answer in question.Answers.ToList())
                {
                    context.Entry(answer).State = EntityState.Deleted;
                }
                // delete question
                context.Entry(question).State = EntityState.Deleted;
                
                context.SaveChanges();
            }   
        }

        public void DeleteQuestion(int id)
        {
            using (var context = new EnglishQuestionContext())
            {
                var question = context.Questions.Include("Answers").FirstOrDefault(x => x.Id == id);
                if (question != null)
                {
                    DeleteQuestion(question);
                }
            }
        }

        public ParagraphMeta GetQuestionMetaOfSection(string testLevel, string questionLevel, string purpose, string section)
        {
            using (var context = new EnglishQuestionContext())
            {
                var quetionMeta = context.Questions.Include("Answers")
                                         .Where(x => x.TestLevel.Contains(testLevel)
                                                    && (x.Section == section)
                                                    && (x.Level == questionLevel)
                                                    && (purpose == QuestionPurpose.All.ToString() || x.Purpose == purpose))
                                         .Select(x => new QuestionMeta()
                                            {
                                                Id = x.Id,
                                                TimeDone = x.TimeDone,
                                                CanMixAnswers = x.CanMixAnswer,
                                                AnswerMeta = x.Answers.Select(y => new AnswerMeta()
                                                                        {
                                                                            Id = y.Id,
                                                                            IsAnswer = y.IsAnswer
                                                                        }).ToList()
                                            }).ToList();
                return new ParagraphMeta()
                {
                    Id = 0,
                    QuestionMeta = quetionMeta
                };
            }
        }

        public List<ParagraphMeta> GetParagraphMetaOfSection(string testLevel, string questionLevel, string purpose, string section)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Paragraphs.Include("Questions.Answers")
                              .Where(x => x.TestLevel.Contains(testLevel)
                                        && (x.Section == section)
                                        && (x.Level == questionLevel)
                                        && (purpose == QuestionPurpose.All.ToString() || x.Purpose == purpose))
                              .Select(x => new ParagraphMeta()
                                {
                                    Id = x.Id,
                                    TimeDone = x.Questions.Sum(t => t.TimeDone),
                                    AudioFilePath = x.FileInfo,
                                    QuestionMeta = x.Questions.Select(y => new QuestionMeta()
                                    {
                                        Id = y.Id,
                                        CanMixAnswers = y.CanMixAnswer,
                                        AnswerMeta = y.Answers.Select(z => new AnswerMeta()
                                        {
                                            Id = z.Id,
                                            IsAnswer = z.IsAnswer
                                        }).ToList()
                                    }).ToList()
                                }).ToList();
            }
        }

        public IEnumerable<ParagraphMeta> GetParagraphMetaByManual(string testLevel, string section)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Paragraphs.Include("Questions.Answers")
                              .Where(x => x.TestLevel.Contains(testLevel) && (x.Section == section))
                              .Select(x => new ParagraphMeta()
                              {
                                Id = x.Id,
                                Title = x.Title,
                                TimeDone = x.Questions.Sum(t => t.TimeDone),
                                AudioFilePath = x.FileInfo,
                                QuestionMeta = x.Questions.Select(y => new QuestionMeta()
                                {
                                    Id = y.Id,
                                    Content = y.Content,
                                    CanMixAnswers = y.CanMixAnswer,
                                    AnswerMeta = y.Answers.Select(z => new AnswerMeta()
                                    {
                                        Id = z.Id,
                                        IsAnswer = z.IsAnswer
                                    }).ToList()
                                }).ToList()
                              })
                              .ToList();
            }
        }

        public IEnumerable<QuestionMeta> GetQuestionMetaByManual(string testLevel, string section)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Questions.Include("Answers")
                                        .Where(x => x.TestLevel.Contains(testLevel) && (x.Section == section))
                                        .Select(x => new QuestionMeta()
                                        {
                                            Id = x.Id,
                                            Title = x.Title,
                                            TimeDone = x.TimeDone,
                                            CanMixAnswers = x.CanMixAnswer,
                                            AnswerMeta = x.Answers.Select(y => new AnswerMeta()
                                            {
                                                Id = y.Id,
                                                IsAnswer = y.IsAnswer
                                            }).ToList()
                                        }).ToList();
            }
        }

        public int SaveTest(Test test)
        {
            using (var context = new EnglishQuestionContext())
            {
                try
                {
                    test.EndTestDate = test.TestDate.AddMinutes(test.TotalTime);
                    context.Tests.Add(test);
                    return context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        public int SaveSubTest(SubTest subTest)
        {
            using (var context = new EnglishQuestionContext())
            {
                context.Entry(subTest).State = EntityState.Modified;
                return context.SaveChanges();
            }
        }

        public Test GetTest(int id)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Tests.Find(id);
            }
        }

        public IEnumerable<Test> LoadTest(string testLevel, bool isChoice)
        {
            using (var context = new EnglishQuestionContext())
            {
                var tests = context.Tests.Include("SubTests").Where(x => x.Level == testLevel && x.IsChoice == isChoice).ToList();
                foreach (var test in tests)
                {
                    test.ClassName = GetClass(test.ClassNo).ClassName;
                    yield return test;
                }
            }
        }

        public Paragraph GetParagraphById(int id)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Paragraphs.Include("Questions.Answers").FirstOrDefault(x => x.Id == id);
            }
        }

        public string GetParagraphContent(int paragraphId)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Paragraphs.FirstOrDefault(x => x.Id == paragraphId)?.Content;
            }
        }

        public string GetQuestionContent(int questionId)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Questions.FirstOrDefault(x => x.Id == questionId)?.Content;
            }
        }

        public string GetAnswerContent(int answerId)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Answers.FirstOrDefault(x => x.Id == answerId)?.Content;
            }
        }

        public Question GetQuestionById(int id)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Questions.Include("Answers").FirstOrDefault(x => x.Id == id);
            }
        }

        public IEnumerable<LopHocs> GetClasses()
        {
            using (var bank = new BankQuestionContext())
            {
                return bank.Classes.ToList();
            }
            //return new List<LopHocs>()
            //{
            //    new LopHocs()
            //    {
            //        ClassId = 1,
            //        ClassName = "Anh Van",
            //        ClassNo = "AV"
            //    }
            //};
        }

        public LopHocs GetClass(string classNo)
        {
            //using (var bank = new BankQuestionContext())
            //{
            //    return bank.Classes.FirstOrDefault(x => x.ClassNo == classNo);
            //}
            return new LopHocs()
            {
                ClassId = 1,
                ClassName = "Anh Van",
                ClassNo = "AV"
            };
        }

        public IEnumerable<B1B2ConfigValue> GetB1B2ConfigValues()
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.B1B2ConfigValues.ToList();
            }
        }

        public string GetB1B2ConfigValue(string key)
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.B1B2ConfigValues.FirstOrDefault(x => x.Key == key)?.Value.ToEmpty();
            }
        }

        public bool CheckExistConfig(string key)
        {
            using (var context = new EnglishQuestionContext())
            {
                var config = context.B1B2ConfigValues.FirstOrDefault(x => x.Key == key);
                return config != null;
            }
        }

        public void UpdateConfig(B1B2ConfigValue configValue)
        {
            using (var context = new EnglishQuestionContext())
            {
                context.Entry(configValue).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void AddB1B2Config(B1B2ConfigValue configValue)
        {
            using (var context = new EnglishQuestionContext())
            {
                context.B1B2ConfigValues.Add(configValue);
                context.SaveChanges();
            }
        }

        public IEnumerable<KeyValueDisplay> GetB1B2Configs()
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.B1B2ConfigValues.Select(x => new KeyValueDisplay()
                {
                    Key = x.Key,
                    Value = x.Value
                }).ToList();
            }
        }

        public void DeleteB1B2Config(B1B2ConfigValue configValue)
        {
            using (var context = new EnglishQuestionContext())
            {
                context.Entry(configValue).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public IEnumerable<Test> SynchronizeTest()
        {
            using (var context = new EnglishQuestionContext())
            {
                return context.Tests.Include("SubTests").Where(x => string.IsNullOrEmpty(x.Extend1)).ToList();
            }
        }

        public void SynchorizedTests(IEnumerable<Test> tests)
        {
            using (var context = new EnglishQuestionContext())
            {
                foreach (var test in tests)
                {
                    context.Entry(test).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public void GenerateTemplateParagraphData()
        {
            var paragraphList = new List<Paragraph>();
            for (int i = 1; i <= 50; i++)
            {
                var paragraph = new Paragraph()
                {
                    Content = "Paragraph " + i,
                    TestLevel = "A,B,C,B1,B2",
                    Section = LevelSection.AR2.ToString(),
                    Purpose = QuestionPurpose.All.ToString(),
                    Level = QuestionLevel.Normal.ToString(),
                    Type = i % 3 == 0 ? "RPQA" : i % 3 == 1 ? "RPA" : "WPA",
                    TimeDone = 10,
                    Title = "Title " + i, 
                    Questions = new ObservableCollection<Question>()
                };
                for (int j = 1; j <= 10; j++)
                {
                    paragraph.Questions.Add(new Question()
                    {
                        Content = paragraph.Content + " Question " + j,
                        Level = paragraph.Level,
                        TestLevel = paragraph.TestLevel,
                        Purpose = "All",
                        Section = "AR2",
                        Type = paragraph.Type,
                        TimeDone = 1,
                        Title = "Question " + j,
                        Answers = new ObservableCollection<Answer>()
                        {
                            new Answer(){Content = "Answer A", IsAnswer = true},
                            new Answer(){Content = "Answer B", IsAnswer = false},
                            new Answer(){Content = "Answer C", IsAnswer = false},
                            new Answer(){Content = "Answer D", IsAnswer = false},
                        }
                    });
                }
                paragraphList.Add(paragraph);
            }

            for (int i = 1; i <= 50; i++)
            {
                var paragraph = new Paragraph()
                {
                    Content = "Paragraph " + i,
                    TestLevel = "A,B,C,B1,B2",
                    Section = LevelSection.AR3.ToString(),
                    Purpose = QuestionPurpose.All.ToString(),
                    Level = QuestionLevel.Normal.ToString(),
                    Type = i%2==0 ? "LQA" : "LQA1",
                    TimeDone = 10,
                    Title = "Title " + i,
                    Questions = new ObservableCollection<Question>()
                };
                for (int j = 1; j <= 10; j++)
                {
                    paragraph.Questions.Add(new Question()
                    {
                        Content = paragraph.Content + " Question " + i,
                        Level = paragraph.Level,
                        TestLevel = paragraph.TestLevel,
                        Purpose = "All",
                        Section = "AR3",
                        Type = paragraph.Type,
                        TimeDone = 1,
                        Title = "question " + j,
                        Answers = new ObservableCollection<Answer>()
                        {
                            new Answer(){Content = "Answer A", IsAnswer = true},
                            new Answer(){Content = "Answer B", IsAnswer = false},
                            new Answer(){Content = "Answer C", IsAnswer = false},
                            new Answer(){Content = "Answer D", IsAnswer = false},
                        }
                    });
                }
                paragraphList.Add(paragraph);
            }

            using (var context = new EnglishQuestionContext())
            {
                context.Paragraphs.AddRange(paragraphList);
                context.SaveChanges();
            }
        }

        public void GenerateTemplateQuestionData()
        {
            var questionList = new List<Question>();
            for (int i = 1; i <= 100; i++)
            {
                var question = new Question()
                {
                    Content = "Question RQA " + i,
                    Level = QuestionLevel.Normal.ToString(),
                    TestLevel = "A,B,C,B1,B2",
                    Purpose = "All",
                    Section = LevelSection.AR1.ToString(),
                    ParagraphId = 1,
                    Type = "RQA",
                    TimeDone = 2,
                    Title = "Question RQA " + i,
                    Answers = new ObservableCollection<Answer>()
                    {
                        new Answer(){Content = "Answer A", IsAnswer = true},
                        new Answer(){Content = "Answer B", IsAnswer = false},
                        new Answer(){Content = "Answer C", IsAnswer = false},
                        new Answer(){Content = "Answer D", IsAnswer = false},
                    }
                };
                questionList.Add(question);
            }
            for (int i = 1; i <= 100; i++)
            {
                var question = new Question()
                {
                    Content = "Question WQA " + i,
                    Level = QuestionLevel.Normal.ToString(),
                    TestLevel = "A,B,C",
                    Purpose = "All",
                    Section = LevelSection.AR4A.ToString(),
                    ParagraphId = 1,
                    Type = "WQA",
                    TimeDone = 2,
                    Title = "Question WQA " + i,
                    Answers = new ObservableCollection<Answer>()
                    {
                        new Answer(){Content = "Answer A", IsAnswer = true}
                    }
                };
                questionList.Add(question);
            }

            for (int i = 1; i <= 100; i++)
            {
                var question = new Question()
                {
                    Content = "Question WPA " + i,
                    Level = QuestionLevel.Normal.ToString(),
                    TestLevel = "A,B,C",
                    Purpose = "All",
                    Section = LevelSection.AR4B.ToString(),
                    ParagraphId = 1,
                    Type = "WQA",
                    TimeDone = 2,
                    Title = "Question WPA " + i,
                    Answers = new ObservableCollection<Answer>()
                    {
                        new Answer(){Content = "Answer A", IsAnswer = true}
                    }
                };
                questionList.Add(question);
            }


            using (var context = new EnglishQuestionContext())
            {
                context.Questions.AddRange(questionList);
                context.SaveChanges();
            }
        }
    }
}
