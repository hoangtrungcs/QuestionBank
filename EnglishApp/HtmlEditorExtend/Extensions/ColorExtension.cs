using System.Windows.Media;

namespace HtmlEditorExtend.Extensions
{
    /// <summary>
    /// 提供 System.Drawing.Color 和 Color 之间的转换方法。
    /// To provide methods to handle convertion between System.Drawing.Color and Color.
    /// </summary>
    internal static class ColorExtension
    {
        /// <summary>
        /// 将 System.Drawing.Color 转换到 Color。
        /// Convert System.Drawing.Color to Color.
        /// </summary>
        public static Color ColorConvert(this System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// 将 Color 转换到 System.Drawing.Color。
        /// Convert Color to System.Drawing.Color.
        /// </summary>
        public static System.Drawing.Color ColorConvert(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// 判断 System.Drawing.Color 和 Color 是否表示相同的颜色。
        /// </summary>
        public static bool ColorEqual(this System.Drawing.Color drawingColor, Color mediaColor)
        {
            return (drawingColor.A == mediaColor.A
                    && drawingColor.R == mediaColor.R
                    && drawingColor.G == mediaColor.G
                    && drawingColor.B == mediaColor.B);
        }

        /// <summary>
        /// 判断 Color 和 System.Drawing.Color 是否表示相同的颜色。
        /// </summary>
        public static bool ColorEqual(this Color mediaColor, System.Drawing.Color drawingColor)
        {
            return (drawingColor.A == mediaColor.A
                    && drawingColor.R == mediaColor.R
                    && drawingColor.G == mediaColor.G
                    && drawingColor.B == mediaColor.B);
        }

        /// <summary>
        /// 将表达式转换到 Color。
        /// Convert an expression to Color.
        /// </summary>
        public static Color ConvertToColor(string value)
        {
            return (Color)ColorConverter.ConvertFromString(value);
            //int r = 0, g = 0, b = 0;
            //if (value.StartsWith("#"))
            //{
            //    int v = Convert.ToInt32(value.Substring(1), 16);
            //    r = (v >> 16) & 255; g = (v >> 8) & 255; b = v & 255;
            //}
            //return Color.FromRgb(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }

        public static readonly Color DefaultBackColor = System.Windows.SystemColors.WindowColor;

        public static readonly Color DefaultForeColor = System.Windows.SystemColors.WindowTextColor;
    }
}
