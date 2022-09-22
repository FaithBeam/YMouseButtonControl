using System;
using System.Collections.Generic;
using YMouseButtonControl.Core.Models;

namespace YMouseButtonControl.Core.Factories;

public static class ButtonMappingFactory
{
    private static Dictionary<int, Type> _buttonMappings = new()
    {
        {2, typeof(SimulatedKeystrokes)}
    };

    public static List<IButtonMapping> GetMappings()
    {
        
    }
}