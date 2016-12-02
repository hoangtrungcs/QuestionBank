using System;
using System.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls
{
    /// <summary>
    /// Interaction logic for Blank.xaml
    /// </summary>
    public partial class Blank : UserControl
    {
        #region Properties

        /// <summary> Gets the type. </summary>
        public DateTime LastActived { get; set; }
        /// <summary> Reading question answer page view model. </summary>

        #endregion
        public Blank()
        {
            InitializeComponent();
            LastActived = DateTime.MinValue;
        }
    }
}
