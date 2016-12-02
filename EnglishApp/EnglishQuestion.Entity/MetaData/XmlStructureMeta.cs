using System.Collections.Generic;

namespace EnglishQuestion.Entity.MetaData
{
    public class BaseMeta : NotifyPropertyChanged
    {
        public bool IsSelected { get { return Get<bool>(); } set { Set(value); } }
        public int Id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public int TimeDone { get; set; }
    }

    public class ParagraphMeta : BaseMeta
    {
        public List<QuestionMeta> QuestionMeta { get; set; }
        public bool IsListening { get; set; }

        public string AudioFilePath { get; set; }

        public ParagraphMeta()
        {
            QuestionMeta = new List<QuestionMeta>();
        }
    }

    public class QuestionMeta : BaseMeta
    {
        public List<AnswerMeta> AnswerMeta { get; set; }

        public bool CanMixAnswers { get; set; }

        public QuestionMeta()
        {
            AnswerMeta = new List<AnswerMeta>();
        }
    }

    public class AnswerMeta
    {
        public int Id { get; set; }
        public bool IsAnswer { get; set; }
    }
}
