using System;
using System.Collections.Generic;
using System.Linq;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.ViewModels.Factories;

public static class ButtonMappingFactory
{
    private static readonly Dictionary<string, Func<IButtonMapping>> StringMappings = new()
    {
        {"NothingMapping", () => new NothingMapping()},
        {"SimulatedKeystrokes", () => new SimulatedKeystrokes()},
    };
    
    private static readonly Dictionary<int, Func<IButtonMapping>> IntMappings = new()
    {
        {0, () => new NothingMapping()},
        {1, () => new SimulatedKeystrokes()},
    };

    public static IEnumerable<string> GetButtonMappingDescriptions()
    {
        return StringMappings.Select(x => x.Value().Description);
    }

    public static IButtonMapping Create(string key)
    {
        if (StringMappings.ContainsKey(key))
        {
            return StringMappings[key]();
        }

        throw new ArgumentException($"Not a valid key: {key}");
    }
    
    public static IButtonMapping Create(int key)
    {
        if (IntMappings.ContainsKey(key))
        {
            return IntMappings[key]();
        }

        throw new ArgumentException($"Not a valid key: {key}");
    }
}