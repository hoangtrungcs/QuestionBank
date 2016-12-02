/*
=========================================================================================================
  Module      : Question (Question.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishQuestion.Entity
{
    /// <summary>
    /// Question
    /// </summary>
    public class Question : BaseEntity
    {
        public string Content { get { return Get<string>(); } set { Set(value); } }
        public string Level { get { return Get<string>(); } set { Set(value); } }
        public string Purpose { get { return Get<string>(); } set { Set(value); } }
        public string Section { get { return Get<string>(); } set { Set(value); } } // This is type of question
        public string TestLevel { get { return Get<string>(); } set { Set(value); } }
        public int TimeDone { get { return Get<int>(); } set { Set(value); } }
        public bool CanMixAnswer { get { return Get<bool>(); } set { Set(value); } }
        public string Status { get { return Get<string>(); } set { Set(value); } }
        public ObservableCollection<Answer> Answers { get; set; }
        public int ParagraphId { get; set; }
        public Paragraph Paragraph { get; set; }

        public string Type { get; set; }
        public string Title { get { return Get<string>(); } set { Set(value); } }

        [NotMapped]
        public Guid UniqueKey { get; set; }

        [NotMapped]
        public bool HasModify { get; set; }

        public Question()
        {
            CanMixAnswer = true;
        }
    }
}