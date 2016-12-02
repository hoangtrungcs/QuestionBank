using System;
using EnglishQuestion.Entity;

namespace EnglishQuestion.MainApp.ViewModels
{
    public class SearchBaseVM<T> : NotifyPropertyChanged where T : BaseEntity
    {
        public string Content { get { return Get<string>(); } set { Set(value); } }
        public bool IsLevelA { get { return Get<bool>(); } set { Set(value); } }
        public bool IsLevelB { get { return Get<bool>(); } set { Set(value); } }
        public bool IsLevelC { get { return Get<bool>(); } set { Set(value); } }
        public bool IsLevelB1 { get { return Get<bool>(); } set { Set(value); } }
        public bool IsLevelB2 { get { return Get<bool>(); } set { Set(value); } }
        public DateTime From { get { return Get<DateTime>(); } set { Set(value); } }
        public DateTime To { get { return Get<DateTime>(); } set { Set(value); } }

        public SearchBaseVM()
        {
            Content = string.Empty;
            From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            To = From.AddMonths(1).AddDays(-1);
        }

        public void ClearSearch()
        {
            Content = string.Empty;
            From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            To = From.AddMonths(1).AddDays(-1);
        }
    }
}
