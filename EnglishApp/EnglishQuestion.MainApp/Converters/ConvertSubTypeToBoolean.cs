/*
=========================================================================================================
  Module      : Convert SubType To Boolean (ConvertSubTypeToBoolean.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using EnglishQuestion.AppCommon;
using EnglishQuestion.Common;

namespace EnglishQuestion.MainApp.Converters
{
    /// <summary>
    /// Convert SubType To Boolean
    /// </summary>
    public class ConvertSubTypeToBoolean : IValueConverter
    {
        private string m_subType;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            m_subType = value.ToEmpty();
            return value.SplitAndRemoveEmpty(Constants.CommaSign).Contains(parameter.ToEmpty());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var subTypes = m_subType.SplitAndRemoveEmpty(Constants.CommaSign).ToList();
            if ((bool)value)
            {
                subTypes.Add(parameter.ToEmpty());
            }
            else
            {
                subTypes.Remove(parameter.ToEmpty());
            }
            subTypes = subTypes.Distinct().OrderBy(x => x).ToList();

            return string.Join(Constants.CommaSign, subTypes);
        }
    }
}