using System.Collections;
using System.Globalization;

namespace HamibotRemoteControl.Converters
{
    /// <summary>
    /// 判断List是否为空
    /// </summary>
    internal class CollectionIsEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            return value is ICollection { Count: 0 };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
