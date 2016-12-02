using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Entity;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.MainApp.ViewModels;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.PopUp
{
    /// <summary>
    /// Interaction logic for AddEditQuestionAnswer.xaml
    /// </summary>
    public partial class AddEditListeningQuestion : Window
    {
        #region Properties

        /// <summary> Reading question answer page view model. </summary>
        public AddEditQuestionAnswerViewModel PageViewModel { get; private set; }

        #endregion

        public AddEditListeningQuestion(Question question)
        {
            InitializeComponent();
            PageViewModel = new AddEditQuestionAnswerViewModel();
            PageViewModel.Current = question;
            //cbbSettingLevel.ItemsSource = m_pageViewModel.QuestionLevels;
            //cbbSettingPurpose.ItemsSource = m_pageViewModel.QuestionPurposes;
            DataContext = PageViewModel;

            PageViewModel.Current.PropertyChanged += (s, args) =>
            {
                switch (args.PropertyName)
                {
                    case "Content":
                        PageViewModel.Current.Title = questionEditor.ContentText;
                        break;
                }
            };
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (PageViewModel.Current.Answers.Any(x => x.IsAnswer && x.Content == string.Empty))
            {
                RadMessageBox.Show(AppCommonResource.CannotSelectEmptyAnswer);
                return;
            }

            if (PageViewModel.Current.Answers.All(x => !x.IsAnswer))
            {
                RadMessageBox.Show(AppCommonResource.MustChooseAnswer, AppCommonResource.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(questionEditor.ContentText) || string.IsNullOrEmpty(answerEditor.ContentText))
            {
                RadMessageBox.Show(AppCommonResource.PopupQuestionEmpty);
                return;
            }

            DialogResult = true;

            var textBoxes = this.ChildrenOfType<TextBox>().Select(item => new ControlDependency()
            {
                Control = item,
                Property = TextBox.TextProperty
            });
            var radioButtons = this.ChildrenOfType<RadRadioButton>().Select(item => new ControlDependency()
            {
                Control = item,
                Property = ToggleButton.IsCheckedProperty
            });
            var checkBoxes = this.ChildrenOfType<CheckBox>().Select(item => new ControlDependency()
            {
                Control = item,
                Property = ToggleButton.IsCheckedProperty
            });

            BindingBehavior.UpdateSource(textBoxes);
            BindingBehavior.UpdateSource(radioButtons);
            BindingBehavior.UpdateSource(checkBoxes);

            Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
