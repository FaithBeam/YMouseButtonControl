using System;
using Riok.Mapperly.Abstractions;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.Mappings;

[Mapper]
public static partial class ButtonMappingMapper
{
    [MapDerivedType<DisabledMapping, DisabledMappingVm>]
    [MapDerivedType<NothingMapping, NothingMappingVm>]
    [MapDerivedType<SimulatedKeystroke, SimulatedKeystrokeVm>]
    [MapDerivedType<RightClick, RightClickVm>]
    public static partial BaseButtonMappingVm Map(ButtonMapping? buttonMapping);

    [MapDerivedType<DisabledMappingVm, DisabledMapping>]
    [MapDerivedType<NothingMappingVm, NothingMapping>]
    [MapDerivedType<SimulatedKeystrokeVm, SimulatedKeystroke>]
    [MapDerivedType<RightClickVm, RightClick>]
    public static partial ButtonMapping Map(BaseButtonMappingVm buttonMapping);

    [MapDerivedType<DisabledMappingVm, DisabledMapping>]
    [MapDerivedType<NothingMappingVm, NothingMapping>]
    [MapDerivedType<SimulatedKeystrokeVm, SimulatedKeystroke>]
    [MapDerivedType<RightClickVm, RightClick>]
    public static partial void Map(BaseButtonMappingVm src, ButtonMapping dst);

    public static BaseSimulatedKeystrokeTypeVm MapSimulatedKeystrokeType(
        SimulatedKeystrokeType simulatedKeystrokeType
    ) =>
        simulatedKeystrokeType switch
        {
            SimulatedKeystrokeType.AsMousePressedAndReleasedActionType =>
                new AsMousePressedAndReleasedActionTypeVm(),
            SimulatedKeystrokeType.DuringMouseActionType => new DuringMouseActionTypeVm(),
            SimulatedKeystrokeType.InAnotherThreadPressedActionType =>
                new InAnotherThreadPressedActionTypeVm(),
            SimulatedKeystrokeType.InAnotherThreadReleasedActionType =>
                new InAnotherThreadReleasedActionTypeVm(),
            SimulatedKeystrokeType.MouseButtonPressedActionType =>
                new MouseButtonPressedActionTypeVm(),
            SimulatedKeystrokeType.MouseButtonReleasedActionType =>
                new MouseButtonReleasedActionTypeVm(),
            SimulatedKeystrokeType.RepeatedlyWhileButtonDownActionType =>
                new RepeatedlyWhileButtonDownActionTypeVm(),
            SimulatedKeystrokeType.StickyHoldActionType => new StickyHoldActionTypeVm(),
            SimulatedKeystrokeType.StickyRepeatActionType => new StickyRepeatActionTypeVm(),
            _ => throw new ArgumentOutOfRangeException(
                nameof(simulatedKeystrokeType),
                simulatedKeystrokeType,
                null
            ),
        };

    public static SimulatedKeystrokeType? MapSimulatedKeystrokeTypeVm(
        BaseSimulatedKeystrokeTypeVm? baseSimulatedKeystrokeVm
    ) =>
        baseSimulatedKeystrokeVm switch
        {
            null => null,
            AsMousePressedAndReleasedActionTypeVm =>
                SimulatedKeystrokeType.AsMousePressedAndReleasedActionType,
            DuringMouseActionTypeVm => SimulatedKeystrokeType.DuringMouseActionType,
            InAnotherThreadPressedActionTypeVm =>
                SimulatedKeystrokeType.InAnotherThreadPressedActionType,
            InAnotherThreadReleasedActionTypeVm =>
                SimulatedKeystrokeType.InAnotherThreadReleasedActionType,
            MouseButtonPressedActionTypeVm => SimulatedKeystrokeType.MouseButtonPressedActionType,
            MouseButtonReleasedActionTypeVm => SimulatedKeystrokeType.MouseButtonReleasedActionType,
            RepeatedlyWhileButtonDownActionTypeVm =>
                SimulatedKeystrokeType.RepeatedlyWhileButtonDownActionType,
            StickyHoldActionTypeVm => SimulatedKeystrokeType.StickyHoldActionType,
            StickyRepeatActionTypeVm => SimulatedKeystrokeType.StickyRepeatActionType,
            _ => throw new ArgumentOutOfRangeException(),
        };
}
