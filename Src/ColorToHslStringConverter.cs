using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorShadesGenerator;

public class ColorToHslStringConverter : IValueConverter
{
    #region IValueConverter Implementation

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Color color) return DependencyProperty.UnsetValue;

        var hslColor = HslColor.FromColor(color);

        return $"hsl({hslColor.H:F0}, {hslColor.S * 100.0:F0}%, {hslColor.L * 100.0:F0}%)";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }

    #endregion
}