using Microsoft.Maui.Controls.Shapes;
using System.Globalization;

namespace Dollet.Conventers
{
    class StringToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                var converter = new PathGeometryConverter();
                return converter.ConvertFromInvariantString(stringValue);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
