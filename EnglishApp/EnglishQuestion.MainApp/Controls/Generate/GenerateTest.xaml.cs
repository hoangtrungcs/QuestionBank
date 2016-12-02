using EnglishQuestion.Common;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.Generate
{
    /// <summary>
    /// Interaction logic for GenerateTest.xaml
    /// </summary>
    public partial class GenerateTest : RadWindow
    {
        private TestLevel m_level;

        public GenerateTest(TestLevel level, bool isChoice)
        {
            InitializeComponent();
            m_level = level;
            container.GenerateConfigLevel(level, isChoice);
        }
    }
}
