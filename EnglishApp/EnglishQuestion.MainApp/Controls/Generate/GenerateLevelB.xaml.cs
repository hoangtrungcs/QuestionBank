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
    public partial class GenerateLevelB : GenerateBasePage
    {
        private GenerateBaseVM m_pageViewModel;
        private string[] DefaultSectionTitle =
        {
            MainResource.BSectionA,
            MainResource.BSectionB,
            MainResource.BSectionC,
            MainResource.BSectionD,
            MainResource.BSectionE,
            MainResource.ATitleListeningSection1,
            MainResource.ATitleListeningSection2,
            MainResource.ATitleListeningSection3
        };

        private string[] Sections =
        {
            LevelSection.BR1.ToString(),
            LevelSection.BR2.ToString(),
            LevelSection.BR3.ToString(),
            LevelSection.BR4.ToString(),
            LevelSection.BR5.ToString(),
            LevelSection.BL1.ToString(),
            LevelSection.BL2.ToString(),
            LevelSection.BL3.ToString()
        };

        public GenerateLevelB(GenerateBaseVM pageViewModel)
        {
            InitializeComponent();
            m_pageViewModel = pageViewModel ?? new GenerateBaseVM();

            // Create config sections
            m_pageViewModel.ConfigLevels = new ObservableCollection<GenerateConfigLevel>();
            m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                DefaultSectionTitle[0], 10, QuestionLevel.Normal.ToString(),
                10, Sections[0], false, QuestionType.RQA.ToString(), B1B2Configs));
            m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                DefaultSectionTitle[1], 1, QuestionLevel.Normal.ToString(),
                10, Sections[1], true, QuestionType.RPQA.ToString(), B1B2Configs));

            if (!pageViewModel.IsChoice)
            {
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    DefaultSectionTitle[2], 1, QuestionLevel.Normal.ToString(),
                    10, Sections[2], true, QuestionType.WPA.ToString(), B1B2Configs));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    DefaultSectionTitle[3], 10, QuestionLevel.Normal.ToString(),
                    10, Sections[3], false, QuestionType.WQA.ToString(), B1B2Configs));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    DefaultSectionTitle[4], 10, QuestionLevel.Normal.ToString(),
                    10, Sections[4], false, QuestionType.WQA.ToString(), B1B2Configs));

                // Listening
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    DefaultSectionTitle[5], 1, QuestionLevel.Normal.ToString(),
                    10, Sections[5], true, QuestionType.LQA1.ToString(), B1B2Configs, true));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    DefaultSectionTitle[6], 1, QuestionLevel.Normal.ToString(),
                    10, Sections[6], true, QuestionType.LQA.ToString(), B1B2Configs, true));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    DefaultSectionTitle[7], 1, QuestionLevel.Normal.ToString(),
                    10, Sections[7], true, QuestionType.LQA1.ToString(), B1B2Configs, true));
            }
            else
            {
                section3.Visibility = section4.Visibility = section5.Visibility = Visibility.Collapsed;
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
                section.IsManual = true;
            }
        }
    }
}
