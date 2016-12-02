using System.Windows;
using EnglishQuestion.MainApp.ViewModels;

namespace EnglishQuestion.MainApp.HtmlEditor
{
    /// <summary>
    /// Interaction logic for HtmlEditor.xaml
    /// </summary>
    public partial class HtmlEditor : Window
    {
        public HtmlEditorVM PageViewModel { get; private set; }

        public HtmlEditor(HtmlEditorVM viewModel) 
        {
            InitializeComponent();
            PageViewModel = viewModel ?? new HtmlEditorVM(string.Empty);
            DataContext = PageViewModel;
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (DialogResult.HasValue)
            {
                DialogResult = true;
            }
            Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (DialogResult.HasValue)
            {
                DialogResult = false;
            }
            Close();
        }
    }
}
