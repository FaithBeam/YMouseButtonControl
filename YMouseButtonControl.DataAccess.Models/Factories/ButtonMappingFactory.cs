using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Factories;

public static class ButtonMappingFactory
{
    private static readonly Dictionary<ButtonMappings, Func<IButtonMapping>> ButtonMappingDictionary = new()
    {
        { ButtonMappings.Nothing, () => new NothingMapping() },
        { ButtonMappings.SimulatedKeystrokes, () => new SimulatedKeystrokes() },
    };

    public static IEnumerable<IButtonMapping> GetButtonMappings()
    {
        return ButtonMappingDictionary.Select(x => x.Value());
    }

    public static IEnumerable<string> GetButtonMappingDescriptions()
    {
        return GetButtonMappings().Select(x => x.Description);
    }

    public static IButtonMapping Create(ButtonMappings key)
    {
        if (ButtonMappingDictionary.ContainsKey(key))
        {
            return ButtonMappingDictionary[key]();
        }

        throw new ArgumentException($"Not a valid key: {key}");
    }

    public static IButtonMapping Create(int index)
    {
        if (index > ButtonMappingDictionary.Count)
        {
            return ButtonMappingDictionary.ElementAt(index).Value();
        }

        throw new IndexOutOfRangeException($"Index out of range: {index}");
    }
}