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
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.Generate
{
    /// <summary>
    /// Interaction logic for GenerateLevelA.xaml
    /// </summary>
    public partial class GenerateLevelA : GenerateBasePage
    {
        private GenerateBaseVM m_pageViewModel;
        private string[] DefaultSectionTitle =
        {
            MainResource.ATitleSection1,
            MainResource.ATitleSection2,
            MainResource.ATitleSection3,
            MainResource.ATitleSection4A,
            MainResource.ATitleSection4B,
            MainResource.ATitleListeningSection1,
            MainResource.ATitleListeningSection2,
            MainResource.ATitleListeningSection3
        };

        private string[] Sections =
        {
            LevelSection.AR1.ToString(),
            LevelSection.AR2.ToString(),
            LevelSection.AR3.ToString(),
            LevelSection.AR4A.ToString(),
            LevelSection.AR4B.ToString(),
            LevelSection.AL1.ToString(),
            LevelSection.AL2.ToString(),
            LevelSection.AL3.ToString()
        };

        public GenerateLevelA(GenerateBaseVM pageViewModel)
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
            m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                DefaultSectionTitle[2], 1, QuestionLevel.Normal.ToString(),
                10, Sections[2], true, QuestionType.RPA.ToString(), B1B2Configs));

            if (!m_pageViewModel.IsChoice)
            {
                // writing
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    DefaultSectionTitle[3], 5, QuestionLevel.Normal.ToString(),
                    10, Sections[3], false, QuestionType.WQA.ToString(), B1B2Configs));
                m_pageViewModel.ConfigLevels.Add(new GenerateConfigLevel(
                    DefaultSectionTitle[4], 5, QuestionLevel.Normal.ToString(),
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
                section4A.Visibility = Visibility.Collapsed;
                section4B.Visibility = Visibility.Collapsed;
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
            var button = (RadButton)e.OriginalSource;

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
            if (manual.DialogResult.GetValueOrDefault(true))
            {
                if (manual.SelectedParagraphMeta != null && manual.SelectedParagraphMeta.QuestionMeta.Count > 0)
                {
                    section = m_pageViewModel.ConfigLevels.First(x => x.Section == parameters[0]);
                    section.ParagraphMeta = manual.SelectedParagraphMeta;
                    section.NumOfQuestion = section.IsParagraph ? 1 : section.ParagraphMeta.QuestionMeta.Count;
                    section.TimeDone = section.ParagraphMeta.TimeDone;
                    section.IsManual = true;

                    button.Background = System.Windows.Media.Brushes.LightGreen;
                }
                else
                {
                    section = m_pageViewModel.ConfigLevels.First(x => x.Section == parameters[0]);
                    section.ParagraphMeta = null;
                    section.IsManual = false;
                    button.Background = System.Windows.Media.Brushes.Transparent;
                }
            }
        }
    }
}
