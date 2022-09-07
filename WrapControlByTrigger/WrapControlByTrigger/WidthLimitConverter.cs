using System;
using System.Globalization;
using System.Windows.Data;

namespace WrapControlByTrigger
{
    public class WidthLimitConverter : IValueConverter
    {
        public double WidthLimit { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                return width <= this.WidthLimit;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
