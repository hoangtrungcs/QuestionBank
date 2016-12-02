using EnglishQuestion.Entity;

namespace EnglishQuestion.MainApp.ViewModels
{
    public class HtmlEditorVM : NotifyPropertyChanged
    {
        public string HtmlContent { get { return Get<string>(); } set { Set(value); } }

        public HtmlEditorVM(string htmlContent)
        {
            HtmlContent = htmlContent;
        }
    }
}
