using System.Collections.ObjectModel;
using System.Linq;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;

namespace EnglishQuestion.MainApp.ViewModels
{
    public class BaseVM<T> : NotifyPropertyChanged where T : BaseEntity
    {
        public ObservableCollection<T> ItemsSource { get; set; }
        public T Current { get { return Get<T>(); } set { Set(value); } }

        public SearchBaseVM<T> Search { get; private set; }
        public ObservableCollection<KeyValueDisplay> QuestionLevels { get; private set; }
        public ObservableCollection<KeyValueDisplay> QuestionPurposes { get; private set; }

        public BaseVM()
        {
            QuestionLevels = EnumHelper.GenerateLevels();
            QuestionPurposes = EnumHelper.GeneratePurposes();
            Search = new SearchBaseVM<T>();
            ItemsSource = new ObservableCollection<T>();
        }

        public void Insert(T item)
        {
            ItemsSource.Add(item);
            item.Action = ActionType.Insert;
            Current = item;
        }

        public void Save()
        {
            
        }

        public void Remove(int id)
        {
            var item = ItemsSource.FirstOrDefault(x => x.Id == id);
            Remove(item);
        }

        public void Remove(T item)
        {
            if (item == null) return;
            ItemsSource.Remove(item);
        }
    }
}
