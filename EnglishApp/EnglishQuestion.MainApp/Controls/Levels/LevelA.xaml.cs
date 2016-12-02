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
    public partial class LevelA : LevelBasePage, ILevelBase
    {
        public TestLevel Level { get { return TestLevel.CLevelA; } }
        public DateTime Created { get; private set; }

        private readonly List<IComposeBase> m_composePages;

        public LevelA()
        {
            InitializeComponent();
            Created = DateTime.Now;

            m_composePages = new List<IComposeBase>();
            //InitializePage();
        }

        private void InitializePage()
        {
            // Reading
            m_composePages.Add(new ReadingQA(Level, LevelSection.AR1));
            m_composePages.Add(new ReadingPQA(Level, LevelSection.AR2));
            m_composePages.Add(new ReadingPA(Level, LevelSection.AR3));
            m_composePages.Add(new WritingQA(Level, LevelSection.AR4A));
            m_composePages.Add(new WritingQA(Level, LevelSection.AR4B));

            // Listening
            m_composePages.Add(new ListeningQA(Level, LevelSection.AL1));
            m_composePages.Add(new Listening1QA(Level, LevelSection.AL2));
            m_composePages.Add(new ListeningQA(Level, LevelSection.AL3));
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
                    case LevelSection.AR1:
                        page = new ReadingQA(Level, LevelSection.AR1);
                        break;
                    case LevelSection.AR2:
                        page = new ReadingPQA(Level, LevelSection.AR2);
                        break;
                    case LevelSection.AR3:
                        page = new ReadingPA(Level, LevelSection.AR3);
                        break;
                    case LevelSection.AR4A:
                        page = new WritingQA(Level, LevelSection.AR4A);
                        break;
                    case LevelSection.AR4B:
                        page = new WritingQA(Level, LevelSection.AR4B);
                        break;
                    case LevelSection.AL1:
                        page = new Listening1QA(Level, LevelSection.AL1);
                        break;
                    case LevelSection.AL2:
                        page = new ListeningQA(Level, LevelSection.AL2);
                        break;
                    case LevelSection.AL3:
                        page = new Listening1QA(Level, LevelSection.AL3);
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
