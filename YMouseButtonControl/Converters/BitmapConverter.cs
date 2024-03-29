using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace YMouseButtonControl.Converters;

public class BitmapConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string bmpPath)
        {
            return null;
        }

        return string.IsNullOrWhiteSpace(bmpPath) ? null : new Bitmap(bmpPath);
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
