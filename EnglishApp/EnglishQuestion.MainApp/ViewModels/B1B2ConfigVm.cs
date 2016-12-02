using System.Collections.ObjectModel;
using EnglishQuestion.Entity;
using EnglishQuestion.Service;

namespace EnglishQuestion.MainApp.ViewModels
{
    public class B1B2ConfigVm : NotifyPropertyChanged
    {
        public ObservableCollection<B1B2ConfigValue> ItemsSource { get; set; }

        public string Key { get { return Get<string>(); } set { Set(value); } }
        public string Value { get { return Get<string>(); } set { Set(value); } }

        public B1B2ConfigVm()
        {
            Key = string.Empty;
            Value = string.Empty;

            ItemsSource = new ObservableCollection<B1B2ConfigValue>();

            var configs = DbHelper.Instance.GetB1B2ConfigValues();
            foreach (var config in configs)
            {
                ItemsSource.Add(config);
            }
        }
    }
}
