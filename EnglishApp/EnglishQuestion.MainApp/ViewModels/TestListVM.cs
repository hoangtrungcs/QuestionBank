using System.Collections.ObjectModel;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Entity;
using EnglishQuestion.Service;

namespace EnglishQuestion.MainApp.ViewModels
{
    public class TestListVM : NotifyPropertyChanged
    {
        public ObservableCollection<Test> ItemsSource { get; set; }

        public TestListVM()
        {
            ItemsSource = new ObservableCollection<Test>();
        }

        public void LoadTest(string testLevel, bool isChoice)
        {
            ItemsSource.Clear();
            var tests = DbHelper.Instance.LoadTest(testLevel.GetSubTypeFromTestLevel(), isChoice);
            foreach (var test in tests)
            {
                ItemsSource.Add(test);
            }
        }
    }
}
