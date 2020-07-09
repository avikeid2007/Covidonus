using System;
using Windows.UI.Xaml.Data;

namespace Uno.Material.Converters
{
    public class FromOpenToCustomValueConverter : IValueConverter
    {
        public object ValueIfOpen { get; set; }

        public object ValueIfClosed { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                return ValueIfOpen;
            }

            return ValueIfClosed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
