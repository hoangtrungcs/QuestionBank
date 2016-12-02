using System.Linq;
using System.Windows;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.Compose
{
    /// <summary>
    /// Interaction logic for WritingQA.xaml
    /// </summary>
    public partial class WritingQA : ComposeBasePage, IComposeBase
    {
        public TestLevel Level { get; private set; }
        public LevelSection Section { get; private set; }

        private WritingQAVM m_pageViewModel;

        public WritingQA(TestLevel level, LevelSection section)
        {
            InitializeComponent();
            Level = level;
            Section = section;

            m_pageViewModel = new WritingQAVM();
            cbbSettingLevel.ItemsSource = m_pageViewModel.QuestionLevels;
            cbbSettingPurpose.ItemsSource = m_pageViewModel.QuestionPurposes;

            DataContext = m_pageViewModel;

            ResetPage();

            CurrentPropertyChanged();

            //var paragraphs = GetQuestion(Level.ToString().GetSubTypeFromTestLevel(), QuestionType.WQA.ToString());
            //foreach (var paragraph in paragraphs)
            //{
            //    m_pageViewModel.ItemsSource.Add(paragraph);
            //}

            //DataContext = m_pageViewModel;

            //CurrentPropertyChanged();
        }

        private void OnGridViewSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            m_pageViewModel.Current = (Question)dgvQuestions.SelectedItem;
            CurrentPropertyChanged();
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            ResetPage();
        }

        private void CurrentPropertyChanged()
        {
            if (m_pageViewModel.Current == null) return;

            m_pageViewModel.Current.PropertyChanged += (s, args) =>
            {
                m_pageViewModel.Current.HasModify = true;
                switch (args.PropertyName)
                {
                    case "Content":
                        m_pageViewModel.Current.Title = editor.ContentText;
                        break;
                }
            };
        }

        public void Insert()
        {
            m_pageViewModel.Insert(CreateQuestion(Level.ToString().GetSubTypeFromTestLevel(), Section.ToString(),
                QuestionLevel.Normal.ToString(), QuestionPurpose.All.ToString(), QuestionType.WQA.ToString(), true));
            dgvQuestions.SelectedItem = m_pageViewModel.Current;

            ScrollToEndGridView(dgvQuestions);
        }

        public void Save()
        {
            var errorNumbers = m_pageViewModel.ItemsSource.Where(x => string.IsNullOrEmpty(x.Content)).Select(x => x.RowNumber);
            if (errorNumbers.Any())
            {
                var error = string.Format(AppCommonResource.EmptyContent, string.Join(", ", errorNumbers));
                RadMessageBox.Show(error, AppCommonResource.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (m_pageViewModel.ItemsSource.Any(x => x.Answers.Any(y => string.IsNullOrEmpty(y.Content))))
            {
                RadMessageBox.Show(AppCommonResource.CannotSelectEmptyAnswer, AppCommonResource.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DbHelper.Instance.SaveQuestion(m_pageViewModel.ItemsSource.Where(x => x.HasModify));
            RadMessageBox.Show(AppCommonResource.Successful, AppCommonResource.SussessCaption);
        }

        public void Delete()
        {
            if (m_pageViewModel.Current == null)
            {
                RadMessageBox.Show(AppCommonResource.NoSelectedQuestion);
                return;
            }
            if (RadMessageBox.Show(AppCommonResource.ConfirmDelete, AppCommonResource.ConfirmCaption,
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            DbHelper.Instance.DeleteQuestion(m_pageViewModel.Current);
            m_pageViewModel.ItemsSource.Remove(m_pageViewModel.Current);
            if (m_pageViewModel.ItemsSource.Count > 0)
            {
                m_pageViewModel.Current = m_pageViewModel.ItemsSource[0];
                dgvQuestions.SelectedItem = m_pageViewModel.Current;
            }
        }

        public void Cancel()
        {
            ResetPage();
        }

        private void ResetPage()
        {
            var questions = SearchQuestions(Level.ToString().GetSubTypeFromTestLevel(),
                                Section.ToString(),
                                m_pageViewModel.Search);

            m_pageViewModel.ItemsSource.Clear();
            foreach (var question in questions)
            {
                m_pageViewModel.ItemsSource.Add(question);
            }

            if (m_pageViewModel.ItemsSource.Count > 0)
            {
                m_pageViewModel.Current = m_pageViewModel.ItemsSource[0];
            }
        }
    }
}
