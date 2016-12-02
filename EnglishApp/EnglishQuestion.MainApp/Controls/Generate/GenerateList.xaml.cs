using System;
using System.Windows;
using System.Windows.Controls;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;
using EnglishQuestion.Entity;
using EnglishQuestion.LocalizeResource;
using EnglishQuestion.MainApp.Controls.Levels;
using EnglishQuestion.MainApp.TelerikMessageBox;
using EnglishQuestion.MainApp.Utility;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;
using mshtml;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.Controls.Generate
{
    /// <summary>
    /// Interaction logic for GenerateList.xaml
    /// </summary>
    public partial class GenerateList : UserControl, ILevelBase
    {
        public TestLevel Level { get; private set; }
        public DateTime Created { get { return DateTime.Now; } }

        private bool m_isChoice = false;

        private TestListVM m_pageViewModel;

        public GenerateList(TestLevel level, bool isChoice)
        {
            InitializeComponent();
            Level = level;
            m_isChoice = isChoice;

            m_pageViewModel = new TestListVM();
            DataContext = m_pageViewModel;
        }

        public void Insert()
        {
            try
            {
                var generate = new GenerateTest(Level, m_isChoice);
                generate.Owner = Application.Current.MainWindow;
                generate.ShowDialog();

                OnLoaded(null, null);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        public void Save()
        {
            var selectedItem = trvTest.SelectedItem;

            if (selectedItem is KeyValueDisplay)
            {
                var subTest = (SubTest)trvTest.SelectedContainer.ParentItem.Item;
                DbHelper.Instance.SaveSubTest(subTest);

                RadMessageBox.Show(AppCommonResource.Successful);
            }
            else
            {
                RadMessageBox.Show(AppCommonResource.NotSelectedSubTest);
            }
        }

        public void Delete()
        {
            
        }

        public void Cancel()
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            m_pageViewModel.LoadTest(Level.ToString(), m_isChoice);
            groupEditor.Visibility = Visibility.Collapsed;
            groupInfo.Visibility = Visibility.Collapsed;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var treeView = sender as RadTreeView;
            var selectedItem = treeView.SelectedItem;

            if (selectedItem is Test)
            {
                groupEditor.Visibility = Visibility.Collapsed;
                groupInfo.Visibility = Visibility.Visible;
                groupInfo.DataContext = selectedItem;
            }
            else if (selectedItem is KeyValueDisplay)
            {
                var type = ((KeyValueDisplay)selectedItem).Key;
                groupEditor.Visibility = Visibility.Visible;
                groupInfo.Visibility = Visibility.Collapsed;

                var subTest = (SubTest)treeView.SelectedContainer.ParentItem.Item;
                editor.BindingContent = (type == SubTestType.Writing.ToString())
                                        ? subTest.WritingTestContent
                                        : subTest.ListeningTestContent;
            }
        }

        private void Editor_OnDocumentReady(object sender, RoutedEventArgs e)
        {
            var editor = sender as HtmlEditorExtend.Views.HtmlEditor;
            (editor.Document.MsHtmlDocInterface as HTMLDocumentEvents2_Event).onfocusout += obj =>
            {
                var selectedItem = trvTest.SelectedItem;

                if (selectedItem is KeyValueDisplay)
                {
                    var type = ((KeyValueDisplay)selectedItem).Key;
                    var subTest = (SubTest)trvTest.SelectedContainer.ParentItem.Item;
                    if (type == SubTestType.Writing.ToString())
                    {
                        subTest.WritingTestContent = editor.ContentHtml;
                    }
                    else
                    {
                        subTest.ListeningTestContent = editor.ContentHtml;
                    }
                }
            };
        }

        private void OnViewResultClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = trvTest.SelectedItem;

            if (selectedItem is KeyValueDisplay)
            {
                var type = ((KeyValueDisplay)selectedItem).Key;
                var subTest = (SubTest)trvTest.SelectedContainer.ParentItem.Item;

                var testResult = XmlHelper.BuildTestResult(type == SubTestType.Writing.ToString()
                                             ? subTest.XmlWritingStructure : subTest.XmlListeningStructure);
                var htmlEditor = new HtmlEditor.HtmlEditor(new HtmlEditorVM(testResult));
                htmlEditor.Show();
            }
            else
            {
                RadMessageBox.Show(AppCommonResource.NotSelectedSubTest);
            }
        }
    }
}
