using System.Windows;

namespace HtmlEditorExtend.Extensions
{
    internal static class FrameworkElementExtension
    {
        /// <summary>
        /// Get the window container of framework element.
        /// </summary>
        public static Window GetParentWindow(this FrameworkElement element)
        {
            DependencyObject dp = element;
            while (dp != null)
            {
                var tp = LogicalTreeHelper.GetParent(dp);
                if (tp is Window) return tp as Window;

                dp = tp;
            }
            return null;
        }
    }
}
