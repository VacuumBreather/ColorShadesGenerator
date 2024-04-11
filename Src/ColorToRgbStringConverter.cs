using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorShadesGenerator;

public class ColorToRgbStringConverter : IValueConverter
{
    #region IValueConverter Implementation

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Color color) return DependencyProperty.UnsetValue;

        return $"rgb({color.R}, {color.G}, {color.B})";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }

    #endregion
}