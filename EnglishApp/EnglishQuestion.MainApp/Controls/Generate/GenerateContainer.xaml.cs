using System;
using System.Windows;
using System.Windows.Controls;
using EnglishQuestion.Common;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.MainApp.Utility;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;

namespace EnglishQuestion.MainApp.Controls.Generate
{
    /// <summary>
    /// Interaction logic for GenerateContainer.xaml
    /// </summary>
    public partial class GenerateContainer : GenerateBasePage, IGenerateBase
    {
        public TestLevel Level { get; private set; }
        public DateTime Created { get; private set; }

        private GenerateBaseVM m_pageViewModel;

        public GenerateContainer()
        {
            InitializeComponent();
            Created = DateTime.Now;

            m_pageViewModel = new GenerateBaseVM();
            DataContext = m_pageViewModel;
        }

        public void Insert()
        {
            
        }

        public void Save()
        {
        }

        public void Delete()
        {
            
        }

        public void Cancel()
        {
        }

        public void GenerateConfigLevel(TestLevel level, bool isChoice)
        {
            m_pageViewModel.IsChoice = isChoice;
            m_pageViewModel.GenerateConfig.TestLevel = level.ToString();
            generateLevelContainer.Children.Add(CreateGenerateLevel(level, m_pageViewModel));
        }

        private void OnGenerateTestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var test = GenerateHelper.GenerateTest(m_pageViewModel);
                var diff = Math.Abs(test.TotalTime - test.RealTestTime);
                var isSaveTest = true;
                if (diff > 15)
                {
                    if (RadMessageBox.Show(string.Format(AppCommonResource.DateDiffTime, test.TotalTime, test.RealTestTime, diff),
                        AppCommonResource.ConfirmCaption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                        MessageBoxResult.No)
                    {
                        isSaveTest = false;
                    }
                }

                if (isSaveTest)
                {
                    DbHelper.Instance.SaveTest(test);
                    RadMessageBox.Show(AppCommonResource.Successful, AppCommonResource.SussessCaption, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                ((GenerateTest)(((Grid)(this.Parent)).Parent)).Close();
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }
    }
}
