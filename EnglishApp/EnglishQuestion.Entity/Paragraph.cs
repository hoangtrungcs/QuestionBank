/*
=========================================================================================================
  Module      : Paragraph (Paragraph.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace EnglishQuestion.Entity
{
    /// <summary>
    /// Paragraph
    /// </summary>
    public class Paragraph : BaseEntity
    {
        public string Content { get { return Get<string>(); } set { Set(value); } }
        public string Level { get { return Get<string>(); } set { Set(value); } }
        public string Purpose { get { return Get<string>(); } set { Set(value); } }
        public string Section { get { return Get<string>(); } set { Set(value); } }
        public string TestLevel { get { return Get<string>(); } set { Set(value); } }
        public int TimeDone { get { return Get<int>(); } set { Set(value); } }
        public string Status { get { return Get<string>(); } set { Set(value); } }
        public string FileInfo { get { return Get<string>(); } set { Set(value); } }
        public ObservableCollection<Question> Questions { get; set; }

        public string Type { get; set; }
        public string Title { get { return Get<string>(); } set { Set(value); } }

        [NotMapped]
        public string FileName
        {
            get { return Path.GetFileName(FileInfo); }
        }

        [NotMapped]
        public bool HasModify { get; set; }
    }
}