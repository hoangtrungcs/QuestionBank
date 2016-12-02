using System.IO;
using System.Windows;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.AppCommon;
using EnglishQuestion.LocalizeResource;

namespace EnglishQuestion.MainApp.Controls.Register
{
    /// <summary>
    /// Interaction logic for LicenseAgreement.xaml
    /// </summary>
    public partial class LicenseAgreement : Window
    {
        public bool IsRegistered { get; set; }

        public LicenseAgreement()
        {
            InitializeComponent();
            txtKey.Text = LicenseKeyHelper.Value();
        }

        private void OnCopy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtKey.Text);
        }

        private void OnRegister(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicense.Text))
            {
                RadMessageBox.Show(AppCommonResource.LicenseFailMessage);
                return;
            }
            if (!LicenseKeyHelper.IsGenuine(txtLicense.Text))
            {
                RadMessageBox.Show(AppCommonResource.LicenseFailMessage);
                return;
            }

            RadMessageBox.Show(AppCommonResource.RegisterSuccess);
            File.WriteAllText(Properties.Settings.Default.LicenseFilePath, txtLicense.Text);
            IsRegistered = true;
            Close();
        }
    }
}
