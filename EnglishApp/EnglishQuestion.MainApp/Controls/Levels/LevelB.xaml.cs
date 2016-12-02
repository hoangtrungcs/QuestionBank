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
    public partial class LevelB : LevelBasePage, ILevelBase
    {
        public TestLevel Level { get { return TestLevel.CLevelB; } }
        public DateTime Created { get; private set; }

        private readonly List<IComposeBase> m_composePages;

        public LevelB()
        {
            InitializeComponent();
            Created = DateTime.Now;

            m_composePages = new List<IComposeBase>();
            //InitializePage();
        }

        private void InitializePage()
        {
            // Reading
            m_composePages.Add(new ReadingQA(Level, LevelSection.BR1));
            m_composePages.Add(new ReadingPQA(Level, LevelSection.BR2));
            m_composePages.Add(new WritingPA(Level, LevelSection.BR3));
            m_composePages.Add(new WritingQA(Level, LevelSection.BR4));
            m_composePages.Add(new WritingQA(Level, LevelSection.BR5));

            // Listening
            m_composePages.Add(new ListeningQA(Level, LevelSection.BL1));
            m_composePages.Add(new ListeningQA(Level, LevelSection.BL2));
            m_composePages.Add(new ListeningQA(Level, LevelSection.BL3));
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
                    case LevelSection.BR1:
                        page = new ReadingQA(Level, LevelSection.BR1);
                        break;
                    case LevelSection.BR2:
                        page = new ReadingPQA(Level, LevelSection.BR2);
                        break;
                    case LevelSection.BR3:
                        page = new WritingPA(Level, LevelSection.BR3);
                        break;
                    case LevelSection.BR4:
                        page = new WritingQA(Level, LevelSection.BR4);
                        break;
                    case LevelSection.BR5:
                        page = new WritingQA(Level, LevelSection.BR5);
                        break;
                    case LevelSection.BL1:
                        page = new Listening1QA(Level, LevelSection.BL1);
                        break;
                    case LevelSection.BL2:
                        page = new Listening1QA(Level, LevelSection.BL2);
                        break;
                    case LevelSection.BL3:
                        page = new Listening1QA(Level, LevelSection.BL3);
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
