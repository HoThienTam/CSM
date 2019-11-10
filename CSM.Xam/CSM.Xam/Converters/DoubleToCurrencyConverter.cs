using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace CSM.Xam.Converters
{
    public class DoubleToCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value).ToString("C");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueFromString = Regex.Replace(value.ToString(), @"\D", "");

            if (valueFromString.Length <= 0)
                return 0;

            double valueDouble;
            if (!double.TryParse(valueFromString, out valueDouble))
                return 0;

            if (valueDouble <= 0)
                return 0;

            return valueDouble;
        }
    }
}
