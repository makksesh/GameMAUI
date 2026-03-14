using System.Globalization;

namespace MobileApp.Converters;

public class EqualConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => values.Length == 2 && values[0]?.ToString() == values[1]?.ToString();

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class NotEqualConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => values.Length == 2 && values[0]?.ToString() != values[1]?.ToString();

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
