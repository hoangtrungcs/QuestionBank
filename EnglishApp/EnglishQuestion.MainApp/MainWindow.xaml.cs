/*
=========================================================================================================
  Module      : Main window (MainWindow.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.Controls.Configs;
using EnglishQuestion.MainApp.Controls.Generate;
using EnglishQuestion.MainApp.Controls.Levels;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.Service;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RadRibbonWindow
    {
        #region Properties

        private readonly List<ILevelBase> m_levelPage;
        private TestLevel m_currentTestsLevel;

        #endregion

        public MainWindow()
        {
            //DbHelper.Instance.GenerateTemplateQuestionData();
            //DbHelper.Instance.GenerateTemplateParagraphData();
            InitializeComponent();
            m_levelPage = new List<ILevelBase>();
            m_currentTestsLevel = TestLevel.None;
            this.DataContext = this;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //m_levelPage.Add(new LevelA());
            //m_levelPage.Add(new LevelB());
            //m_levelPage.Add(new LevelC());
            //m_levelPage.Add(new LevelB1());
            //m_levelPage.Add(new LevelB2());
        }

        private void OnRibbonButtonClick(object sender, RoutedEventArgs e)
        {
            var name = (sender as RadRibbonButton).Name;
            switch (name)
            {
                case "btnAdd":
                    Insert();
                    break;

                case "btnSave":
                    Save();
                    break;

                case "btnCancel":
                    Cancel();
                    break;

                case "btnDelete":
                    Delete();
                    break;

                case "btnCloseCurrentPage":
                    break;
            }
        }

        private void OnLevelClick(object sender, RoutedEventArgs e)
        {
            var level = (sender as RadRibbonRadioButton).Tag.ToEmpty();
            TestLevel testLevel = TestLevel.None;
            if (!Enum.TryParse(level, out testLevel))
            {
                throw new Exception(AppCommonResource.LevelNotFound);
            }

            if (m_currentTestsLevel == testLevel)
            {
                return;
            }

            m_currentTestsLevel = testLevel;

            ShowSelectedLevelPage(testLevel);
        }

        private void OnUpToServerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var syncTests = DbHelper.Instance.SynchronizeTest();

                foreach (var test in syncTests)
                {
                    test.Extend1 = Guid.NewGuid().ToString();
                }

                DbServerHelper.Instance.SynchronizeTest(syncTests);
                //DbHelper.Instance.SynchorizedTests(syncTests);

                RadMessageBox.Show("Successful!");
            }
            catch(Exception ex)
            {
                RadMessageBox.Show("Error when synchronize tests, please check server connection or contact with admin!");
            }
            
        }

        #region Private function

        private void ShowSelectedLevelPage(TestLevel level)
        {
            var levelPage = m_levelPage.FirstOrDefault(x => x.Level == level);
            if (levelPage == null)
            {
                switch (level)
                {
                    case TestLevel.CLevelA:
                        levelPage = new LevelA();
                        break;
                    case TestLevel.CLevelB:
                        levelPage = new LevelB();
                        break;
                    case TestLevel.CLevelC:
                        levelPage = new LevelC();
                        break;
                    case TestLevel.CLevelB1:
                        levelPage = new LevelB1();
                        break;
                    case TestLevel.CLevelB2:
                        levelPage = new LevelB2();
                        break;
                    case TestLevel.GLevelA:
                    case TestLevel.GLevelB:
                    case TestLevel.GLevelC:
                    case TestLevel.GLevelB1:
                    case TestLevel.GLevelB2:
                        levelPage = new GenerateList(level, false);
                        break;
                    case TestLevel.GcLevelA:
                    case TestLevel.GcLevelB:
                    case TestLevel.GcLevelC:
                    case TestLevel.GcLevelB1:
                    case TestLevel.GcLevelB2:
                        levelPage = new GenerateList(level, true);
                        break;
                    case TestLevel.SAudioFilePath:
                        levelPage = new ConfigAudioFilePath();
                        break;
                    case TestLevel.SB1B2:
                        levelPage = new ConfigTestB1B2();
                        break;
                }
                m_levelPage.Add(levelPage);
            }

            pageTransition.ShowPage(levelPage as UserControl);
        }

        private void Insert()
        {
            var levelBase = pageTransition.CurrentPage as ILevelBase;
            if (levelBase != null) levelBase.Insert();
        }

        private void Save()
        {
            var levelBase = pageTransition.CurrentPage as ILevelBase;
            if (levelBase != null) levelBase.Save();
        }

        private void Cancel()
        {
            var levelBase = pageTransition.CurrentPage as ILevelBase;
            if (levelBase != null) levelBase.Cancel();
        }

        private void Delete()
        {
            var levelBase = pageTransition.CurrentPage as ILevelBase;
            if (levelBase != null) levelBase.Delete();
        }

        #endregion

        #region Public function

        #endregion
    }
}
