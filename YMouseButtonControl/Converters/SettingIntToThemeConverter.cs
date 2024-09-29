using System;
using System.Globalization;
using Avalonia.Data.Converters;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Converters;

public class SettingIntToThemeConverter : IValueConverter
{
    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) => value is not int ? null : (ThemeEnum)value;

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) => value is not ThemeEnum ? null : (int)value;
}
