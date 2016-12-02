using System;
using System.Windows.Forms;
using EnglishQuestion.LocalizeResource;

namespace EnglishQuestion.AppCommon
{
    public class FileUtility
    {
        public static string GetDirectory()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = AppCommonResource.OpenAudioPath;
                dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                dialog.ShowNewFolderButton = false;
                return (dialog.ShowDialog() == DialogResult.OK) ? dialog.SelectedPath : string.Empty;
            }
        }

        public static string GetFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                dialog.Filter = "";
                dialog.RestoreDirectory = true;
                return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : string.Empty;
            }
        }
    }
}
