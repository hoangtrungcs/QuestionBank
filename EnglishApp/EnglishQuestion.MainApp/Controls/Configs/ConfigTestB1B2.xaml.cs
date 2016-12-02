using System;
using System.Windows;
using System.Windows.Controls;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.Controls.Levels;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.Configs
{
    /// <summary>
    /// Interaction logic for ConfigAudioFilePath.xaml
    /// </summary>
    public partial class ConfigTestB1B2 : UserControl, ILevelBase
    {
        private B1B2ConfigVm m_pageViewModel;

        public ConfigTestB1B2()
        {
            InitializeComponent();

            m_pageViewModel = new B1B2ConfigVm();

            DataContext = m_pageViewModel;
        }

        private void DgvQuestions_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var config = (B1B2ConfigValue)dgvConfig.SelectedItem;
            if (config == null) return;
            m_pageViewModel.Key = config.Key;
            m_pageViewModel.Value = config.Value;
        }

        private void OnAddConfigClick(object sender, RoutedEventArgs e)
        {
            if (m_pageViewModel.Key == string.Empty || m_pageViewModel.Value == string.Empty)
            {
                RadMessageBox.Show(AppCommonResource.KeyValueNotEmpty);
                return;
            }

            if (DbHelper.Instance.CheckExistConfig(m_pageViewModel.Key))
            {
                RadMessageBox.Show(AppCommonResource.ConfigExisted);
                return;
            }

            var config = new B1B2ConfigValue()
            {
                Key = m_pageViewModel.Key,
                Value = m_pageViewModel.Value
            };

            DbHelper.Instance.AddB1B2Config(config);
            m_pageViewModel.ItemsSource.Add(config);

            m_pageViewModel.Key = string.Empty;
            m_pageViewModel.Value = string.Empty;
        }

        private void OnSaveConfigClick(object sender, RoutedEventArgs e)
        {
            var config = (B1B2ConfigValue)dgvConfig.SelectedItem;

            if (config == null) return;

            if (config.Key != m_pageViewModel.Key && DbHelper.Instance.CheckExistConfig(m_pageViewModel.Key))
            {
                RadMessageBox.Show(AppCommonResource.ConfigExisted);
                return;
            }

            config.Key = m_pageViewModel.Key;
            config.Value = m_pageViewModel.Value;
            DbHelper.Instance.UpdateConfig(config);
        }

        private void OnRemoveConfigClick(object sender, RoutedEventArgs e)
        {
            var config = (B1B2ConfigValue)dgvConfig.SelectedItem;

            if (config != null)
            {
                DbHelper.Instance.DeleteB1B2Config(config);
                m_pageViewModel.ItemsSource.Remove(config);
            }
        }

        public TestLevel Level { get { return TestLevel.SB1B2; } }
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

        public DateTime Created { get { return DateTime.Now; } }
    }
}
