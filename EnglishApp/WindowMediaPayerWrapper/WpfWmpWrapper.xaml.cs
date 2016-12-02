using System.Windows;
using System.Windows.Controls;

namespace WindowMediaPayerWrapper
{
    /// <summary>
    /// Interaction logic for WpfWmpWrapper.xaml
    /// </summary>
    public partial class WpfWmpWrapper : UserControl
    {
        public WpfWmpWrapper()
        {
            InitializeComponent();
            wmplayer.settings.autoStart = false;
        }

        #region Url Dependency Property

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(WpfWmpWrapper),
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnUrlChanged)));

        private static void OnUrlChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var player = (WpfWmpWrapper)sender;
            player.wmplayer.URL = e.NewValue.ToString();
        }

        #endregion
    }
}
