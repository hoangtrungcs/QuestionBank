using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EnglishQuestion.Entity.MetaData;
using EnglishQuestion.Service;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.PopUp
{
    /// <summary>
    /// Interaction logic for SelectQuestionManual.xaml
    /// </summary>
    public partial class SelectQuestionManual : RadWindow
    {
        public ParagraphMeta SelectedParagraphMeta { get; set; }

        private bool m_isParagraph;
        private string m_testLevel;
        private string m_section;
        private IEnumerable<int> m_selectedIds;

        public SelectQuestionManual(string testLevel, string section, bool isParagraph, IEnumerable<int> selectedIds)
        {
            InitializeComponent();
            m_isParagraph = isParagraph;
            m_testLevel = testLevel;
            m_section = section;
            m_selectedIds = selectedIds;

            this.Owner = Application.Current.MainWindow;
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            if (m_isParagraph)
            {
                SelectedParagraphMeta = (dgvGrid.ItemsSource as List<ParagraphMeta>).FirstOrDefault(x => x.IsSelected);
            }
            else
            {
                var questionMeta = (dgvGrid.ItemsSource as List<QuestionMeta>).Where(x => x.IsSelected).ToList();
                SelectedParagraphMeta = new ParagraphMeta()
                {
                    Id = 0,
                    TimeDone = questionMeta.Sum(x => x.TimeDone),
                    QuestionMeta = questionMeta
                };
            }

            Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (m_isParagraph)
            {
                var paragraphMeta = DbHelper.Instance.GetParagraphMetaByManual(m_testLevel, m_section);
                foreach (var id in m_selectedIds)
                {
                    var paragraph = paragraphMeta.FirstOrDefault(x => x.Id == id);
                    if (paragraph != null)
                    {
                        paragraph.IsSelected = true;
                    }
                }
                dgvGrid.ItemsSource = paragraphMeta;
            }
            else
            {
                var questionMeta = DbHelper.Instance.GetQuestionMetaByManual(m_testLevel, m_section);
                foreach (var id in m_selectedIds)
                {
                    var question = questionMeta.FirstOrDefault(x => x.Id == id);
                    if (question != null)
                    {
                        question.IsSelected = true;
                    }
                }
                dgvGrid.ItemsSource = questionMeta;
            }
        }
    }
}
