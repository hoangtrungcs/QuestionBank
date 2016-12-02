using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishQuestion.Entity
{
    public class Test : BaseEntity
    {
        public string No { get { return Get<string>(); } set { Set(value); } }
        public string Name { get { return Get<string>(); } set { Set(value); } }
        public string ClassNo { get { return Get<string>(); } set { Set(value); } }
        public string Level { get { return Get<string>(); } set { Set(value); } }   // Test Level: A B C B1 B2
        public int TotalTime { get { return Get<int>(); } set { Set(value); } }
        public int RealTestTime { get; set; }
        public int TotalQuestion { get { return Get<int>(); } set { Set(value); } }
        public int NumOfSubTest { get { return Get<int>(); } set { Set(value); } }
        public string ConfigStructure { get { return Get<string>(); } set { Set(value); } }
        public string Purpose { get { return Get<string>(); } set { Set(value); } }
        public bool IsChoice { get; set; }
        public DateTime TestDate { get { return Get<DateTime>(); } set { Set(value); } }

        public DateTime EndTestDate { get { return Get<DateTime>(); } set { Set(value); } }

        public bool IsGuess { get { return Get<bool>(); } set { Set(value); } }
        public ObservableCollection<SubTest> SubTests { get; set; }

        [NotMapped]
        public string ClassName { get { return Get<string>(); } set { Set(value); } }

        [NotMapped]
        public string TotalTimeDisplay => $"{TotalTime} minutes";

        [NotMapped]
        public string TestDateDisplay => TestDate.ToString("dd/MM/yyyy");

        public Test()
        {
            ConfigStructure = string.Empty;
        }
    }
}
