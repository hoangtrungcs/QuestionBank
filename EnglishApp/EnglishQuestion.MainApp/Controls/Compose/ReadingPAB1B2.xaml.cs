using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.Controls.PopUp;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.Compose
{
    /// <summary>
    /// Interaction logic for ReadingPA.xaml
    /// </summary>
    public partial class ReadingPAB1B2 : ComposeBasePage, IComposeBase
    {
        public TestLevel Level { get; private set; }
        public LevelSection Section { get; private set; }

        private ReadingPQAVM m_pageViewModel;
        private int m_questionCount = 0;

        public ReadingPAB1B2(TestLevel level, LevelSection section)
        {
            InitializeComponent();
            Level = level;
            Section = section;

            m_pageViewModel = new ReadingPQAVM();
            cbbSettingLevel.ItemsSource = m_pageViewModel.QuestionLevels;
            cbbSettingPurpose.ItemsSource = m_pageViewModel.QuestionPurposes;

            DataContext = m_pageViewModel;

            ResetPage();

            CurrentPropertyChanged();

            //var paragraphs = GetParagraph(Level.ToString().GetSubTypeFromTestLevel(), QuestionType.RPAB1B2.ToString());
            //foreach (var paragraph in paragraphs)
            //{
            //    m_pageViewModel.ItemsSource.Add(paragraph);
            //}

            //DataContext = m_pageViewModel;
            //CurrentPropertyChanged();
        }

        private void OnAddQuestion(object sender, RoutedEventArgs e)
        {
            if (m_pageViewModel.Current == null)
            {
                RadMessageBox.Show(AppCommonResource.NoSelectedQuestion);
                return;
            }

            var question = CreateQuestion(m_pageViewModel.Current.TestLevel,
                                          m_pageViewModel.Current.Section,
                                          m_pageViewModel.Current.Level,
                                          m_pageViewModel.Current.Purpose,
                                          QuestionType.RPAB1B2.ToString());
            question.Action = ActionType.Insert;

            var addQuestion = new AddEditListeningQuestion(question)
            {
                Owner = Application.Current.MainWindow
            };

            addQuestion.ShowDialog();

            if (addQuestion.DialogResult == false)
            {
                return;
            }

            if (m_pageViewModel.Current == null)
            {
                m_pageViewModel.Current = new Paragraph();
            }

            if (m_pageViewModel.Current.Questions == null)
            {
                m_pageViewModel.Current.Questions = new ObservableCollection<Question>();
            }

            m_pageViewModel.Current.Questions.Add(addQuestion.PageViewModel.Current);
            m_pageViewModel.Current.HasModify = true;
        }

        private void OnGridViewSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            m_pageViewModel.Current = (Paragraph)dgvParagraphs.SelectedItem;
            CurrentPropertyChanged();
        }

        private void OnEditGridItem(object sender, RoutedEventArgs e)
        {
            var uniqueKey = (Guid)(sender as RadButton).CommandParameter;
            var editQuestion = new AddEditQuestionAnswer(m_pageViewModel.Current.Questions.First(q => q.UniqueKey == uniqueKey));
            editQuestion.Owner = Application.Current.MainWindow;
            editQuestion.ShowDialog();
        }

        private void OnDeleteGridItem(object sender, RoutedEventArgs e)
        {
            if (m_pageViewModel.Current.Questions.Count < m_questionCount)
            {
                RadMessageBox.Show(AppCommonResource.CannotDelete);
                return;
            }

            if (RadMessageBox.Show(Application.Current.MainWindow, AppCommonResource.ConfirmDeleteMessage, AppCommonResource.ConfirmCaption,
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            var uniqueKey = (Guid)(sender as RadButton).CommandParameter;
            m_pageViewModel.Current.Questions.Remove(m_pageViewModel.Current.Questions.First(q => q.UniqueKey == uniqueKey));
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            ResetPage();
        }

        #region Private method
        private void CurrentPropertyChanged()
        {
            if (m_pageViewModel.Current == null) return;

            m_pageViewModel.Current.PropertyChanged += (s, args) =>
            {
                m_pageViewModel.Current.HasModify = true;
                switch (args.PropertyName)
                {
                    case "Content":
                        m_questionCount = FormatContent();
                        GenerateQuestion(m_questionCount);
                        break;

                    case "TestLevel":
                    case "Purpose":
                    case "Level":
                        foreach (var question in m_pageViewModel.Current.Questions)
                        {
                            question.Level = m_pageViewModel.Current.Level;
                            question.Purpose = m_pageViewModel.Current.Purpose;
                            question.TestLevel = m_pageViewModel.Current.TestLevel;
                        }
                        break;
                }
            };
        }

        /// <summary>
        /// Formats the content.
        /// </summary>
        /// <returns></returns>
        private int FormatContent()
        {
            int count = 0;
            var format = new StringBuilder();
            var rgx = new Regex(string.Format("{0}{1}{2}", Regex.Escape("["), "\\d+", Regex.Escape("]")));
            var tmp = rgx.Replace(m_pageViewModel.Current.Content ?? string.Empty, Constants.QuestionKeyForBlank);
            for (int i = 0; i < tmp.Length; i++)
            {
                if ((i < tmp.Length - 2) && (string.Format("{0}{1}{2}", tmp[i], tmp[i + 1], tmp[i + 2]) == Constants.QuestionKeyForBlank))
                {
                    format.Append(string.Format(Constants.QuestionKeyNumerForBlank, ++count));
                    i += 2;
                }
                else
                {
                    format.Append(tmp[i]);
                }
            }

            m_pageViewModel.Current.Content = format.ToString();

            return count;
        }

        /// <summary>
        /// Generates the question.
        /// </summary>
        /// <param name="numOfQuestion">The number of question.</param>
        private void GenerateQuestion(int numOfQuestion)
        {
            m_pageViewModel.Current.Questions.Clear();

            for (int i = 1; i <= numOfQuestion; i++)
            {
                m_pageViewModel.Current.Questions.Add(new Question()
                {
                    Content = string.Format(Constants.QuestionKeyNumerForBlank, i),
                    Level = m_pageViewModel.Current.Level,
                    Purpose = m_pageViewModel.Current.Purpose,
                    Section = m_pageViewModel.Current.Section,
                    TestLevel = m_pageViewModel.Current.TestLevel,
                    Action = ActionType.Insert,
                    UniqueKey = Guid.NewGuid(),
                    Answers = new ObservableCollection<Answer>()
                    {
                        new Answer()
                    }
                });
            }
        }
        #endregion

        public void Insert()
        {
            m_pageViewModel.Insert(CreateParagraph(Level.ToString().GetSubTypeFromTestLevel(), Section.ToString(),
                QuestionLevel.Normal.ToString(), QuestionPurpose.All.ToString(), string.Empty, QuestionType.RPAB1B2.ToString()));
            dgvParagraphs.SelectedItem = m_pageViewModel.Current;

            ScrollToEndGridView(dgvParagraphs);
        }

        public void Save()
        {
            var errorNumbers = m_pageViewModel.ItemsSource.Where(x => string.IsNullOrEmpty(x.Content) || string.IsNullOrEmpty(x.Title)).Select(x => x.RowNumber);
            if (errorNumbers.Any())
            {
                var error = string.Format(AppCommonResource.EmptyContent, string.Join(", ", errorNumbers));
                RadMessageBox.Show(error, AppCommonResource.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Remove questions first
            var modifies = m_pageViewModel.ItemsSource.Where(x => x.HasModify);
            if (!modifies.Any()) return;

            foreach (var paragraph in modifies)
            {
                if (paragraph.Id > 1)
                {
                    DbHelper.Instance.DeleteQuestionsOfParagraph(paragraph.Id);
                }
            }

            DbHelper.Instance.SaveParagraph(modifies);
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

            DbHelper.Instance.DeleteParagraph(m_pageViewModel.Current);
            m_pageViewModel.ItemsSource.Remove(m_pageViewModel.Current);
            if (m_pageViewModel.ItemsSource.Count > 0)
            {
                m_pageViewModel.Current = m_pageViewModel.ItemsSource[0];
                dgvParagraphs.SelectedItem = m_pageViewModel.Current;
            }
        }

        public void Cancel()
        {
            ResetPage();
        }

        private void ResetPage()
        {
            var paragraphs = SearchParagraphs(Level.ToString().GetSubTypeFromTestLevel(),
                                Section.ToString(),
                                m_pageViewModel.Search);

            m_pageViewModel.ItemsSource.Clear();
            foreach (var paragraph in paragraphs)
            {
                m_pageViewModel.ItemsSource.Add(paragraph);
            }

            if (m_pageViewModel.ItemsSource.Count > 0)
            {
                m_pageViewModel.Current = m_pageViewModel.ItemsSource[0];
            }
        }
    }
}
