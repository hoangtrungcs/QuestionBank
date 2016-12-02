using System;
using System.Collections.ObjectModel;
using EnglishQuestion.AppCommon;

namespace EnglishQuestion.Entity.MetaData
{
    public class GenerateConfig : NotifyPropertyChanged
    {
        public string TestName { get { return Get<string>(); } set { Set(value); } }
        public string ClassNo { get { return Get<string>(); } set { Set(value); } }
        public string TestLevel { get { return Get<string>(); } set { Set(value); } }
        public int TotalTime { get { return Get<int>(); } set { Set(value); } }
        public int TotalQuestion { get { return Get<int>(); } set { Set(value); } }
        public int NumOfSubTests { get { return Get<int>(); } set { Set(value); } }
        public string Purpose { get { return Get<string>(); } set { Set(value); } }
        public DateTime TestDate { get { return Get<DateTime>(); } set { Set(value); } }
        public bool IsGuess { get { return Get<bool>(); } set { Set(value); } }

        public ObservableCollection<KeyValueDisplay> TestLevels { get; private set; }
        public ObservableCollection<KeyValueDisplay> Classes { get; private set; }
        public ObservableCollection<KeyValueDisplay> Purposes { get; private set; }

        public GenerateConfig()
        {
            TestLevels = EnumHelper.GenerateTestLevels();
            Classes = new ObservableCollection<KeyValueDisplay>();
            Purposes = EnumHelper.GeneratePurposes();
            TestDate = DateTime.Now;
        }
    }
}
