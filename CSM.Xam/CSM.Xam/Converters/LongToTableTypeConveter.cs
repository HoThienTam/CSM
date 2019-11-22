using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CSM.Xam.Converters
{
    class LongToTableTypeConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((long)value)
            {
                case 0:
                    return "\uf2fa";
                case 1:
                    return "\uf0c8";
                case 2:
                    return "\uf111";
            }
            return "\uf0c8";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
