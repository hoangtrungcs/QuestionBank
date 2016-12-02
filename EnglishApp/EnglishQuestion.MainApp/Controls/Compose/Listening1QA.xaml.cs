using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.Controls.PopUp;
using EnglishQuestion.MainApp.Properties;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.Compose
{
    /// <summary>
    /// Interaction logic for ListeningQA.xaml
    /// </summary>
    public partial class Listening1QA : ComposeBasePage, IComposeBase
    {
        public TestLevel Level { get; private set; }
        public LevelSection Section { get; private set; }

        private ListeningQAVM m_pageViewModel;

        public Listening1QA(TestLevel level, LevelSection section)
        {
            InitializeComponent();
            Level = level;
            Section = section;

            m_pageViewModel = new ListeningQAVM();
            cbbSettingLevel.ItemsSource = m_pageViewModel.QuestionLevels;
            cbbSettingPurpose.ItemsSource = m_pageViewModel.QuestionPurposes;

            DataContext = m_pageViewModel;

            ResetPage();

            SelectedParagraphPropertyChanged();

            //var paragraphs = GetParagraph(Level.ToString().GetSubTypeFromTestLevel(), QuestionType.LQA1.ToString());
            //foreach (var paragraph in paragraphs)
            //{
            //    m_pageViewModel.ItemsSource.Add(paragraph);
            //}

            //DataContext = m_pageViewModel;

            //SelectedParagraphPropertyChanged();

            //if (m_pageViewModel.ItemsSource.Count > 0)
            //    m_pageViewModel.Current = m_pageViewModel.ItemsSource[0];
        }

        private void OnGridViewSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            m_pageViewModel.Current = (Paragraph)dgvParagraphs.SelectedItem;
            SelectedParagraphPropertyChanged();
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
                                          QuestionType.LQA1.ToString(),
                                          true);
            question.Action = ActionType.Insert;
            question.Answers[0].IsAnswer = true;

            var addQuestion = new AddEditListeningQuestion(question)
            {
                Owner = Application.Current.MainWindow
            };

            addQuestion.ShowDialog();

            if (addQuestion.DialogResult == false)
            {
                return;
            }

            if (m_pageViewModel.Current.Questions == null)
            {
                m_pageViewModel.Current.Questions = new ObservableCollection<Question>();
            }

            m_pageViewModel.Current.Questions.Add(addQuestion.PageViewModel.Current);
            m_pageViewModel.Current.HasModify = true;
        }

        private void OnEditGridItem(object sender, RoutedEventArgs e)
        {
            var uniqueKey = (Guid)(sender as RadButton).CommandParameter;
            var editQuestion = new AddEditListeningQuestion(m_pageViewModel.Current.Questions.First(q => q.UniqueKey == uniqueKey));
            editQuestion.Owner = Application.Current.MainWindow;
            editQuestion.ShowDialog();
        }

        private void OnDeleteGridItem(object sender, RoutedEventArgs e)
        {
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

        public void Insert()
        {
            var audioPath = FileUtility.GetFile();
            if (audioPath == string.Empty) return;

            // Copy file to default audio path
            if (!Directory.Exists(Settings.Default.AudioFilePath))
            {
                if (RadMessageBox.Show(AppCommonResource.CreateAudioDirectoryMessage, AppCommonResource.ConfirmCaption,
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }

                Directory.CreateDirectory(Settings.Default.AudioFilePath);
            }

            string desFile = Path.Combine(Settings.Default.AudioFilePath, Path.GetFileName(audioPath).ToEmpty());
            try
            {
                File.Copy(audioPath, desFile);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, AppCommonResource.ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            m_pageViewModel.Insert(CreateParagraph(Level.ToString().GetSubTypeFromTestLevel(), Section.ToString(),
                                   QuestionLevel.Normal.ToString(), QuestionPurpose.All.ToString(), desFile, QuestionType.LQA1.ToString()));
            dgvParagraphs.SelectedItem = m_pageViewModel.Current;

            ScrollToEndGridView(dgvParagraphs);
        }

        public void Save()
        {
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

            try
            {
                File.Delete(m_pageViewModel.Current.FileInfo);
            }
            catch (Exception)
            {
                // ignored
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

        #region Private method
        private void SelectedParagraphPropertyChanged()
        {
            if (m_pageViewModel.Current == null) return;

            m_pageViewModel.Current.PropertyChanged += (s, args) =>
            {
                m_pageViewModel.Current.HasModify = true;
                switch (args.PropertyName)
                {
                    case "SubType":
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
        #endregion
    }
}
