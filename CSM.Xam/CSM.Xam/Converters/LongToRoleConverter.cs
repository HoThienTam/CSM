using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CSM.Xam.Converters
{
    public class LongToRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((long)value)
            {
                case 0:
                    return "Nhân viên";
                case 1:
                    return "Quản lý";
            }
            return "Nhân viên";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
