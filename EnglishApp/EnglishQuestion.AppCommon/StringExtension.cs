/*
=========================================================================================================
  Module      : String extension (StringExtension.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System;
using EnglishQuestion.Common;

namespace EnglishQuestion.AppCommon
{
    /// <summary>
    /// StringExtension
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// To the empty.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string ToEmpty(this object source)
        {
            return (source == null) ? string.Empty : source.ToString();
        }

        /// <summary>
        /// Splits the and remove empty.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string[] SplitAndRemoveEmpty(this object source, string separator)
        {
            return source.ToEmpty().Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetSubTypeFromTestLevel(this string testLevel)
        {
            return testLevel.Replace(Constants.CTestLevel, string.Empty)
                            .Replace(Constants.GTestLevel, string.Empty)
                            .Replace(Constants.GcTestLevel, string.Empty);
        }
    }
}
