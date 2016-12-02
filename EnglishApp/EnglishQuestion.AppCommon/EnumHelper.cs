/*
=========================================================================================================
  Module      : Enum Helper (EnumHelper.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System.Collections.ObjectModel;
using EnglishQuestion.Common;
using EnglishQuestion.LocalizeResource;

namespace EnglishQuestion.AppCommon
{
    /// <summary>
    /// Enum Helper
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// Generates the levels.
        /// </summary>
        /// <returns>Generate display list for question level</returns>
        public static ObservableCollection<KeyValueDisplay> GenerateLevels()
        {
            return new ObservableCollection<KeyValueDisplay>()
            {
                new KeyValueDisplay(){ Key = QuestionLevel.Easy.ToString(), Value = AppCommonResource.Easy },
                new KeyValueDisplay(){ Key = QuestionLevel.Normal.ToString(), Value = AppCommonResource.Normal },
                new KeyValueDisplay(){ Key = QuestionLevel.Hard.ToString(), Value = AppCommonResource.Hard }
            };
        }

        /// <summary>
        /// Generates the purpose.
        /// </summary>
        /// <returns>Generate display list for question purpose</returns>
        public static ObservableCollection<KeyValueDisplay> GeneratePurposes()
        {
            return new ObservableCollection<KeyValueDisplay>()
            {
                new KeyValueDisplay(){ Key = QuestionPurpose.All.ToString(), Value = AppCommonResource.All },
                new KeyValueDisplay(){ Key = QuestionPurpose.Overview.ToString(), Value = AppCommonResource.Overview },
                new KeyValueDisplay(){ Key = QuestionPurpose.Review.ToString(), Value = AppCommonResource.Review },
                new KeyValueDisplay(){ Key = QuestionPurpose.Test.ToString(), Value = AppCommonResource.Test },
                new KeyValueDisplay(){ Key = QuestionPurpose.Competition.ToString(), Value = AppCommonResource.Competition }
            };
        }

        public static ObservableCollection<KeyValueDisplay> GenerateTestLevels()
        {
            return new ObservableCollection<KeyValueDisplay>()
            {
                new KeyValueDisplay(){ Key = TestLevel.GLevelA.ToString(), Value = AppCommonResource.A },
                new KeyValueDisplay(){ Key = TestLevel.GLevelB.ToString(), Value = AppCommonResource.B },
                new KeyValueDisplay(){ Key = TestLevel.GLevelC.ToString(), Value = AppCommonResource.C },
                new KeyValueDisplay(){ Key = TestLevel.GLevelB1.ToString(), Value = AppCommonResource.B1 },
                new KeyValueDisplay(){ Key = TestLevel.GLevelB2.ToString(), Value = AppCommonResource.B2 }
            };
        } 
    }
}