using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace YMouseButtonControl.Converters;

public class SettingValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string)
        {
            return null;
        }

        if (value is not string valStr)
        {
            return null;
        }
        return bool.Parse(valStr);
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        return value?.ToString();
    }
}
