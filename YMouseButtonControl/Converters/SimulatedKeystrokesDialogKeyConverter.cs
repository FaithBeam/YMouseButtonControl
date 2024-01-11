using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace YMouseButtonControl.Converters;

public class SimulatedKeystrokesDialogKeyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Dictionary<string, string> dict)
        {
            return null;
        }

        if (targetType.IsAssignableTo(typeof(IEnumerable)))
        {
            return dict.Keys.Select(x => x);
        }
        
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}