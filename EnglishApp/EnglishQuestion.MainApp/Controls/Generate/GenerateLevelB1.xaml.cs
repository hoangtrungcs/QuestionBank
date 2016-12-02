using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity.MetaData;
using EnglishQuestion.MainApp.Controls.PopUp;
using EnglishQuestion.MainApp.ViewModels;

namespace EnglishQuestion.MainApp.Controls.Generate
{
    /// <summary>
    /// Interaction logic for GenerateLevelA.xaml
    /// </summary>
    public partial class GenerateLevelB1 : GenerateBasePage
    {
        private GenerateBaseVM m_pageViewModel;
        
        private string[] Sections =
        {
            LevelSection.B1R1.ToString(),
            LevelSection.B1R2.ToString(),
            LevelSection.B1R3.ToString(),
            LevelSection.B1R4.ToString(),
            LevelSection.B1R5.ToString(),
            LevelSection.B1W1.ToString(),
            LevelSection.B1W2.ToString(),
            LevelSection.B1W3.ToString(),
            LevelSection.B1L1.ToString(),
            LevelSection.B1L2.ToString(),
            LevelSection.B1L3.ToString(),
            LevelSection.B1L4.ToString()
        };

        public GenerateLevelB1(GenerateBaseVM pageViewModel)
        {
            InitializeComponent();
            m_pageViewModel = pageViewModel ?? new GenerateBaseVM();

            // reading
            m_pageViewModel.ConfigLevels = new ObservableCollection<GenerateConfigLevel>();
            m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                string.Empty, 10, QuestionLevel.Normal.ToString(),
                10, Sections[0], false, QuestionType.RQA.ToString(), B1B2Configs));
            m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                string.Empty, 1, QuestionLevel.Normal.ToString(),
                10, Sections[1], true, QuestionType.RPAB1B2.ToString(), B1B2Configs));
            m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                string.Empty, 1, QuestionLevel.Normal.ToString(),
                10, Sections[2], true, QuestionType.RPQAB1B2.ToString(), B1B2Configs));
            m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    string.Empty, 1, QuestionLevel.Normal.ToString(),
                    10, Sections[3], true, QuestionType.RPQA.ToString(), B1B2Configs));
            m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                string.Empty, 1, QuestionLevel.Normal.ToString(),
                10, Sections[4], true, QuestionType.RPA.ToString(), B1B2Configs));

            if (!m_pageViewModel.IsChoice)
            {
                // writing
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                string.Empty, 5, QuestionLevel.Normal.ToString(),
                10, Sections[5], false, QuestionType.WQA.ToString(), B1B2Configs));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                string.Empty, 5, QuestionLevel.Normal.ToString(),
                10, Sections[6], false, QuestionType.WQA.ToString(), B1B2Configs));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                string.Empty, 5, QuestionLevel.Normal.ToString(),
                10, Sections[7], false, QuestionType.WQA.ToString(), B1B2Configs));

                // Listening
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    string.Empty, 1, QuestionLevel.Normal.ToString(),
                    10, Sections[8], true, QuestionType.LQA.ToString(), B1B2Configs));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    string.Empty, 1, QuestionLevel.Normal.ToString(),
                    10, Sections[9], true, QuestionType.LQA.ToString(), B1B2Configs));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    string.Empty, 1, QuestionLevel.Normal.ToString(),
                    10, Sections[10], true, QuestionType.LQA1.ToString(), B1B2Configs));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    string.Empty, 1, QuestionLevel.Normal.ToString(),
                    10, Sections[11], true, QuestionType.LQA1.ToString(), B1B2Configs));
            }
            else
            {
                groupListening.Visibility = Visibility.Collapsed;
            }
            
            DataContext = m_pageViewModel;
        }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var parameters = e.Parameter.ToString().Split('_'); // Section_Type_IsParagraph

            var selectedIds = new List<int>();
            var section = m_pageViewModel.ConfigLevels.First(x => x.Section == parameters[0]);
            if (section.IsParagraph && (section.ParagraphMeta != null))
            {
                selectedIds.Add(section.ParagraphMeta.Id);
            }
            else if (section.ParagraphMeta != null && section.ParagraphMeta.QuestionMeta.Count > 0)
            {
                selectedIds = section.ParagraphMeta.QuestionMeta.Select(x => x.Id).ToList();
            }

            var manual = new SelectQuestionManual(m_pageViewModel.GenerateConfig.TestLevel.GetSubTypeFromTestLevel(), parameters[0], Convert.ToBoolean(parameters[2]), selectedIds);
            manual.ShowDialog();
            if (manual.DialogResult.GetValueOrDefault(true)
                && manual.SelectedParagraphMeta != null
                && manual.SelectedParagraphMeta.QuestionMeta.Count > 0)
            {
                section = m_pageViewModel.ConfigLevels.First(x => x.Section == parameters[0]);
                section.ParagraphMeta = manual.SelectedParagraphMeta;
                section.NumOfQuestion = section.IsParagraph ? 1 : section.ParagraphMeta.QuestionMeta.Count;
                section.TimeDone = section.ParagraphMeta.TimeDone;
                section.IsManual = true;
            }
        }
    }
}
