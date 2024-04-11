using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorShadesGenerator;

public class ColorToHexStringConverter : IValueConverter
{
    #region IValueConverter Implementation

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Color color) return DependencyProperty.UnsetValue;

        return color.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }

    #endregion
}