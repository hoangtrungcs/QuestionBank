/*
=========================================================================================================
  Module      : Base entity (BaseEntity.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnglishQuestion.Common;

namespace EnglishQuestion.Entity
{
    /// <summary>
    /// Base entity
    /// </summary>
    public class BaseEntity : NotifyPropertyChanged, ICloneable
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Guid to synchronize
        /// </summary>
        public string Extend1 { get; set; }
        public string Extend2 { get; set; }
        public string Extend3 { get; set; }
        public string Extend4 { get; set; }
        public string Extend5 { get; set; }

        [NotMapped]
        public ActionType Action { get; set; }

        [NotMapped]
        public int RowNumber { get { return Get<int>(); } set { Set(value); } }

        public BaseEntity()
        {
            CreatedDate = UpdatedDate = DateTime.Now;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
