/*
=========================================================================================================
  Module      : Control And Dependency Struct (ControlDependency.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System.Windows;
using System.Windows.Controls;

namespace EnglishQuestion.AppCommon
{
    /// <summary>
    /// Control and dependency mapping
    /// </summary>
    public class ControlDependency
    {
        public Control Control { get; set; }
        public DependencyProperty Property { get; set; }
    }
}