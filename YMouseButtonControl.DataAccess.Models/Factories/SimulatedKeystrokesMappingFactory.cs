using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Factories;

public static class SimulatedKeystrokesMappingFactory
{
    private static readonly Dictionary<string, Func<ISimulatedKeystrokesType>> _dictionary = new()
    {
        { "StickyHold", () => new StickyHoldActionType() }
    };

    public static IEnumerable<ISimulatedKeystrokesType> GetSimulatedKeystrokesTypes()
    {
        return _dictionary.Select(x => x.Value());
    }

    public static IEnumerable<string> GetSimulatedKeystrokesDescriptions()
    {
        return GetSimulatedKeystrokesTypes().Select(x => x.Description);
    }
}