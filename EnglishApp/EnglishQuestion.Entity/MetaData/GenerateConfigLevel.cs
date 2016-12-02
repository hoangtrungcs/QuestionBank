using System.Collections.Generic;
using System.Collections.ObjectModel;
using EnglishQuestion.AppCommon;

namespace EnglishQuestion.Entity.MetaData
{
    public class GenerateConfigLevel : NotifyPropertyChanged
    {
        public string Templatekey { get { return Get<string>(); } set { Set(value); } }
        public int NumOfQuestion { get { return Get<int>(); } set { Set(value); } }
        public string QuestionLevel { get { return Get<string>(); } set { Set(value); } }
        public int TimeDone { get { return Get<int>(); } set { Set(value); } }
        public string Section { get { return Get<string>(); } set { Set(value); } }
        public bool IsParagraph { get; set; }
        public bool IsManual { get; set; }
        public string Type { get; set; }

        public bool IsEditable { get { return !IsParagraph; } }
        public string CommandParameter { get { return string.Format("{0}_{1}_{2}", Section, Type, IsParagraph); } }

        public bool IsLitening { get; set; }

        // important: List question Id to generate
        public ParagraphMeta ParagraphMeta { get; set; }

        public ObservableCollection<KeyValueDisplay> QuestionLevels { get; private set; }

        public ObservableCollection<KeyValueDisplay> HeaderTemplates { get; private set; }

        public GenerateConfigLevel(string templateKey, int numOfQuestion, string questionLevel, int timeDone, string section, bool isParagraph, string type, IEnumerable<KeyValueDisplay> headerTemplates, bool isListening = false)
        {
            Templatekey = templateKey;
            NumOfQuestion = numOfQuestion;
            QuestionLevel = questionLevel;
            TimeDone = timeDone;
            Section = section;
            IsParagraph = isParagraph;
            Type = type;
            IsLitening = isListening;

            QuestionLevels = EnumHelper.GenerateLevels();

            HeaderTemplates = new ObservableCollection<KeyValueDisplay>();
            foreach (var config in headerTemplates)
            {
                HeaderTemplates.Add(config);
            }
        }
    }
}
