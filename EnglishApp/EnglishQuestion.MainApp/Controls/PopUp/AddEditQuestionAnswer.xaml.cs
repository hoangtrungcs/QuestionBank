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
    public partial class AddEditQuestionAnswer : Window
    {
        #region Properties

        /// <summary> Reading question answer page view model. </summary>
        public AddEditQuestionAnswerViewModel PageViewModel { get; private set; }

        #endregion

        public AddEditQuestionAnswer(Question question)
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
                        PageViewModel.Current.Title = editor.ContentText;
                        break;
                }
            };

            chkA.IsChecked = true;

            SetUIAnswers();
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (PageViewModel.Current.Answers.Any(x => x.IsAnswer && x.Content == string.Empty))
            {
                RadMessageBox.Show(AppCommonResource.CannotSelectEmptyAnswer);
                return;
            }

            if (string.IsNullOrEmpty(editor.ContentText))
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

        private void OnRadioAnswerChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            var radio = (RadioButton)sender;
            switch (radio.Name)
            {
                case "chkA":
                    ResetAnswers();
                    PageViewModel.Current.Answers[0].IsAnswer = true;
                    break;
                case "chkB":
                    ResetAnswers();
                    PageViewModel.Current.Answers[1].IsAnswer = true;
                    break;
                case "chkC":
                    ResetAnswers();
                    PageViewModel.Current.Answers[2].IsAnswer = true;
                    break;
                case "chkD":
                    ResetAnswers();
                    PageViewModel.Current.Answers[3].IsAnswer = true;
                    break;
            }
        }

        private void ResetAnswers()
        {
            PageViewModel.Current.Answers[0].IsAnswer = false;
            PageViewModel.Current.Answers[1].IsAnswer = false;
            PageViewModel.Current.Answers[2].IsAnswer = false;
            PageViewModel.Current.Answers[3].IsAnswer = false;
        }

        private void SetUIAnswers()
        {
            chkA.IsChecked = PageViewModel.Current.Answers[0].IsAnswer;
            chkB.IsChecked = PageViewModel.Current.Answers[1].IsAnswer;
            chkC.IsChecked = PageViewModel.Current.Answers[2].IsAnswer;
            chkD.IsChecked = PageViewModel.Current.Answers[3].IsAnswer;
        }
    }
}
