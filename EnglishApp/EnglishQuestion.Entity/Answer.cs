/*
=========================================================================================================
  Module      : Answer (Answer.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

namespace EnglishQuestion.Entity
{
    /// <summary>
    /// Answer
    /// </summary>
    public class Answer : BaseEntity
    {
        public string Content { get { return Get<string>(); } set { Set(value); } }
        public bool IsAnswer { get { return Get<bool>(); } set { Set(value); } }
        public int QuestionId { get { return Get<int>(); } set { Set(value); } }
        public Question Question { get; set; }
    }
}