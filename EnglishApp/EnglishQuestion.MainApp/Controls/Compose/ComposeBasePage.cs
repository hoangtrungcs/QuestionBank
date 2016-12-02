using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Entity;
using EnglishQuestion.MainApp.ViewModels;
using EnglishQuestion.Service;
using mshtml;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EnglishQuestion.MainApp.Controls.Compose
{
    public class ComposeBasePage : UserControl
    {
        public Paragraph CreateParagraph(string testLevel, string section, string level, string purpose, string fileInfo, string type)
        {
            return new Paragraph()
            {
                Content = string.Empty,
                Level = level,
                Purpose = purpose,
                TestLevel = testLevel,
                Section = section,
                TimeDone = 1,
                FileInfo = fileInfo,
                Type = type,
                HasModify = true,
                Questions = new ObservableCollection<Question>()
            };
        }

        public Question CreateQuestion(string testLevel, string section, string level, string purpose, string type, bool isWriting = false)
        {
            var question = new Question()
            {
                Content = string.Empty,
                ParagraphId = 1,
                Level = level,
                CanMixAnswer = true,
                Purpose = purpose,
                TestLevel = testLevel,
                Section = section,
                TimeDone = 1,
                Type = type,
                Answers = new ObservableCollection<Answer>(),
                UniqueKey = Guid.NewGuid(),
                HasModify = true
            };

            var loop = isWriting ? 1 : 4;
            for (int i = 0; i < loop; i++)
            {
                question.Answers.Add(new Answer());
            }

            return question;
        }

        public IEnumerable<Paragraph> GetParagraph(string testLevel, string section)
        {
            return DbHelper.Instance.GetParagraph(testLevel, section);
        }

        public IEnumerable<Question> GetQuestion(string testLevel, string type)
        {
            return DbHelper.Instance.GetQuestion(testLevel, type);
        }

        public IEnumerable<Paragraph> SearchParagraphs(string testLevel, string section, SearchBaseVM<Paragraph> criteria)
        {
            var paragraps = DbHelper.Instance.SearchParagraphs(testLevel, section, criteria.Content.ToEmpty(), criteria.From, criteria.To);
            foreach (var p in paragraps)
            {
                foreach (var q in p.Questions)
                {
                    q.UniqueKey = Guid.NewGuid();
                }
            }

            return paragraps;
        } 

        public IEnumerable<Question> SearchQuestions(string testLevel, string section, SearchBaseVM<Question> criteria)
        {
            return DbHelper.Instance.SearchQuestion(testLevel, section, criteria.Content.ToEmpty(), criteria.From, criteria.To);
        } 

        protected void OnHtmlEditorKeyUp(object sender, KeyEventArgs e)
        {
            if (((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) && e.Key == Key.Q)
            {
                var editor = sender as HtmlEditorExtend.Views.HtmlEditor;
                editor.InsertBlankSeperate();
            }
        }

        protected void OnEditorDocumentReady(object sender, RoutedEventArgs e)
        {
            var editor = sender as HtmlEditorExtend.Views.HtmlEditor;
            (editor.Document.MsHtmlDocInterface as HTMLDocumentEvents2_Event).ondblclick += obj =>
            {
                var htmlEditor = new HtmlEditor.HtmlEditor(new HtmlEditorVM(editor.ContentHtml));
                var dialog = htmlEditor.ShowDialog();
                if (dialog.GetValueOrDefault(true))
                {
                    editor.BindingContent = htmlEditor.PageViewModel.HtmlContent;
                }

                return true;
            };
        }

        /// <summary>
        /// Auto row number
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RowLoadedEventArgs"/> instance containing the event data.</param>
        protected void OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            var gridview = (RadGridView)sender;
            int index = gridview.ItemContainerGenerator.IndexFromContainer(e.Row);
            if (e.Row.Item != null)
            {
                ((BaseEntity)e.Row.Item).RowNumber = index + 1;
            }
        }

        protected void ScrollToEndGridView(RadGridView gridView)
        {
            gridView.ScrollIntoViewAsync(gridView.Items[gridView.Items.Count - 1],
                                        (f => { (f as GridViewRow).IsSelected = true; })); // the callback method
        }
    }
}
