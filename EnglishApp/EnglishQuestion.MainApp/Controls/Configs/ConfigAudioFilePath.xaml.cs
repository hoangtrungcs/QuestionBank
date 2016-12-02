using System;
using System.Windows;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.Properties;
using EnglishQuestion.MainApp.TelerikMessageBox;
using UserControl = System.Windows.Controls.UserControl;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.MainApp.Controls.Levels;

namespace EnglishQuestion.MainApp.Controls.Configs
{
    /// <summary>
    /// Interaction logic for ConfigAudioFilePath.xaml
    /// </summary>
    public partial class ConfigAudioFilePath : UserControl, ILevelBase
    {
        public ConfigAudioFilePath()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(Settings.Default.AudioFilePath))
            {
                Settings.Default.AudioFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            txtAudioPath.Text = Settings.Default.AudioFilePath;
        }

        #region EventHandler
        private void OnBrowseAudioPath(object sender, RoutedEventArgs e)
        {
            txtAudioPath.Text = FileUtility.GetDirectory();
        }

        private void OnSaveAudioPath(object sender, RoutedEventArgs e)
        {
            if (txtAudioPath.Text == string.Empty)
            {
                RadMessageBox.Show(this, AppCommonResource.AudioPathEmptyMessage, AppCommonResource.ErrorCaption,
                                   MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Settings.Default.AudioFilePath = txtAudioPath.Text;
                Settings.Default.Save();
                RadMessageBox.Show(AppCommonResource.Successful);
            }

        }
        #endregion

        public TestLevel Level { get { return TestLevel.SAudioFilePath; } }
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
