using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.Controls.Compose;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.Levels
{
    /// <summary>
    /// Interaction logic for LevelA.xaml
    /// </summary>
    public partial class LevelB2 : LevelBasePage, ILevelBase
    {
        public TestLevel Level { get { return TestLevel.CLevelB2; } }
        public DateTime Created { get; private set; }

        private readonly List<IComposeBase> m_composePages;

        public LevelB2()
        {
            InitializeComponent();
            Created = DateTime.Now;

            m_composePages = new List<IComposeBase>();
        }

        private void OnSectionChecked(object sender, RoutedEventArgs e)
        {
            var section = (sender as RadRadioButton).Tag.ToEmpty();
            LevelSection levelSection;
            if (!Enum.TryParse(section, out levelSection))
            {
                throw new Exception(AppCommonResource.SectionNotFound);
            }

            ShowSelectedSection(levelSection);
        }

        private void ShowSelectedSection(LevelSection levelSection)
        {
            var page = m_composePages.FirstOrDefault(x => x.Section == levelSection);
            if (page == null)
            {
                switch (levelSection)
                {
                    // reading
                    case LevelSection.B1R1:
                        page = new ReadingQA(Level, LevelSection.B1R1);
                        break;
                    case LevelSection.B1R2:
                        page = new ReadingPAB1B2(Level, LevelSection.B1R2);
                        break;
                    case LevelSection.B1R3:
                        page = new ReadingPQAB1B2(Level, LevelSection.B1R3);
                        break;
                    case LevelSection.B1R4:
                        page = new ReadingPQA(Level, LevelSection.B1R4);
                        break;
                    case LevelSection.B1R5:
                        page = new ReadingPA(Level, LevelSection.B1R5);
                        break;
                    // writing
                    case LevelSection.B1W1:
                        page = new WritingQA(Level, LevelSection.B1W1);
                        break;
                    case LevelSection.B1W2:
                        page = new WritingQA(Level, LevelSection.B1W2);
                        break;
                    case LevelSection.B1W3:
                        page = new WritingQA(Level, LevelSection.B1W3);
                        break;
                    // listening
                    case LevelSection.B1L1:
                        page = new ListeningQA(Level, LevelSection.B1L1);
                        break;
                    case LevelSection.B1L2:
                        page = new ListeningQA(Level, LevelSection.B1L2);
                        break;
                    case LevelSection.B1L3:
                        page = new Listening1QA(Level, LevelSection.B1L3);
                        break;
                    case LevelSection.B1L4:
                        page = new Listening1QA(Level, LevelSection.B1L4);
                        break;
                }
                m_composePages.Add(page);
            }

            pageTransition.ShowPage(page as UserControl);
        }

        public void Insert()
        {
            if (pageTransition.CurrentPage == null) return;
            (pageTransition.CurrentPage as IComposeBase).Insert();
        }

        public void Save()
        {
            if (pageTransition.CurrentPage == null) return;
            (pageTransition.CurrentPage as IComposeBase).Save();
        }

        public void Delete()
        {
            if (pageTransition.CurrentPage == null) return;
            (pageTransition.CurrentPage as IComposeBase).Delete();
        }

        public void Cancel()
        {
            if (pageTransition.CurrentPage == null) return;
            (pageTransition.CurrentPage as IComposeBase).Cancel();
        }
    }
}
