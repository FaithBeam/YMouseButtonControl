using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Factories;

public static class SimulatedKeystrokeTypesMappingFactory
{
    private static readonly List<Func<ISimulatedKeystrokesType>> SimulatedKeystrokeTypesList = new()
    {
        () => new MouseButtonPressedActionType(),
        () => new MouseButtonReleasedActionType(),
        () => new DuringMouseActionType(),
        () => new InAnotherThreadPressedActionType(),
        () => new InAnotherThreadReleasedActionType(),
        () => new RepeatedlyWhileButtonDownActionType(),
        () => new StickyRepeatActionType(),
        () => new StickyHoldActionType(),
        () => new AsMousePressedAndReleasedActionType(),
    };

    public static IEnumerable<ISimulatedKeystrokesType> GetSimulatedKeystrokesTypes()
    {
        return SimulatedKeystrokeTypesList.Select(x => x());
    }
}