using System.Collections.ObjectModel;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Entity;
using EnglishQuestion.Entity.MetaData;
using EnglishQuestion.Service;

namespace EnglishQuestion.MainApp.ViewModels
{
    public class GenerateBaseVM : NotifyPropertyChanged
    {
        public GenerateConfig GenerateConfig { get { return Get<GenerateConfig>(); } set { Set(value); } }
        public ObservableCollection<GenerateConfigLevel> ConfigLevels { get; set; }

        public bool IsChoice { get; set; }

        public GenerateBaseVM()
        {
            GenerateConfig = new GenerateConfig();
            var classes = DbHelper.Instance.GetClasses();
            foreach (var item in classes)
            {
                GenerateConfig.Classes.Add(new KeyValueDisplay()
                {
                    Key = item.ClassNo,
                    Value = item.ClassName
                });
            }
        }
    }
}
