using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace StationControllerUi.Converters
{
    [ValueConversion(typeof(string), typeof(Brush))]
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string s)
            {
                try
                {
                    var color = (Color) ColorConverter.ConvertFromString(s);
                    return new SolidColorBrush(color);
                }
                catch
                {
                    return System.Windows.SystemColors.ControlLightBrush;
                }
            }
            return System.Windows.SystemColors.ControlLightBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color c)
            {
                return c.ToString();
            }
            return string.Empty;
        }
    }
}
