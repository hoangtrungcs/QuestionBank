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
    public partial class LevelC : LevelBasePage, ILevelBase
    {
        public TestLevel Level { get { return TestLevel.CLevelC; } }
        public DateTime Created { get; private set; }

        private readonly List<IComposeBase> m_composePages;

        public LevelC()
        {
            InitializeComponent();
            Created = DateTime.Now;

            m_composePages = new List<IComposeBase>();
            //InitializePage();
        }

        private void InitializePage()
        {
            // Reading
            m_composePages.Add(new ReadingQA(Level, LevelSection.CR1));
            m_composePages.Add(new ReadingPQA(Level, LevelSection.CR2));
            m_composePages.Add(new WritingPA(Level, LevelSection.CW1A));
            m_composePages.Add(new WritingQA(Level, LevelSection.CW1B));
            m_composePages.Add(new WritingQA(Level, LevelSection.CW1C));

            // Listening
            m_composePages.Add(new ListeningQA(Level, LevelSection.CL1));
            m_composePages.Add(new ListeningQA(Level, LevelSection.CL2));
            m_composePages.Add(new ListeningQA(Level, LevelSection.CL3));
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
                    case LevelSection.CR1:
                        page = new ReadingQA(Level, levelSection);
                        break;
                    case LevelSection.CR2:
                        page = new ReadingPQA(Level, levelSection);
                        break;
                    case LevelSection.CW1A:
                        page = new WritingPA(Level, levelSection);
                        break;
                    case LevelSection.CW1B:
                        page = new WritingQA(Level, levelSection);
                        break;
                    case LevelSection.CW1C:
                        page = new WritingQA(Level, levelSection);
                        break;
                    case LevelSection.CL1:
                        page = new Listening1QA(Level, levelSection);
                        break;
                    case LevelSection.CL2:
                        page = new Listening1QA(Level, levelSection);
                        break;
                    case LevelSection.CL3:
                        page = new Listening1QA(Level, levelSection);
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
