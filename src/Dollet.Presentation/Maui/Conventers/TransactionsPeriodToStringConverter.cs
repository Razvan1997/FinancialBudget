using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Dollet.Core.Enums;

namespace Dollet.Conventers
{
    public class TransactionsPeriodToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string periodString && Enum.TryParse(periodString, out TransactionsPeriod period))
            {
                return period switch
                {
                    TransactionsPeriod.Daily => "Zilnic",
                    TransactionsPeriod.Weekly => "Săptămânal",
                    TransactionsPeriod.Monthly => "Lunar",
                    _ => periodString
                };
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "Zilnic" => TransactionsPeriod.Daily,
                "Săptămânal" => TransactionsPeriod.Weekly,
                "Lunar" => TransactionsPeriod.Monthly,
                _ => throw new InvalidOperationException("Valoarea nu poate fi convertită.")
            };
        }
    }
}
