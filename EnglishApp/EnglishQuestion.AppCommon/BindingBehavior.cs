/*
=========================================================================================================
  Module      : Binding Behavior (BindingBehavior.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System.Collections.Generic;

namespace EnglishQuestion.AppCommon
{
    public class BindingBehavior
    {
        public static void UpdateSource(IEnumerable<ControlDependency> controlDependencies)
        {
            foreach (var item in controlDependencies)
            {
                var binding = item.Control.GetBindingExpression(item.Property);
                if (binding != null) binding.UpdateSource();
            }
        }

        public static void UpdateTarget(IEnumerable<ControlDependency> controlDependencies)
        {
            foreach (var item in controlDependencies)
            {
                var binding = item.Control.GetBindingExpression(item.Property);
                if (binding != null) binding.UpdateTarget();
            }
        }
    }
}