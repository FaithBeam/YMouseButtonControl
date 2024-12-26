using System;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.Mappings;

public static class ButtonMappingMapper
{
    public static BaseButtonMappingVm MapToViewModel(ButtonMapping buttonMapping)
    {
        return buttonMapping switch
        {
            DisabledMapping disabledMapping => MapDisabledMapping(disabledMapping),
            NothingMapping nothingMapping => MapNothingMapping(nothingMapping),
            SimulatedKeystroke simulatedKeystroke => MapSimulatedKeystroke(simulatedKeystroke),
            RightClick rightClick => MapRightClick(rightClick),
            _ => throw new InvalidOperationException("Unknown button mapping type"),
        };
    }

    private static DisabledMappingVm MapDisabledMapping(DisabledMapping disabledMapping) =>
        new()
        {
            Id = disabledMapping.Id,
            ProfileId = disabledMapping.ProfileId,
            MouseButton = disabledMapping.MouseButton,
            Keys = disabledMapping.Keys,
            AutoRepeatDelay = disabledMapping.AutoRepeatDelay,
            AutoRepeatRandomizeDelayEnabled = disabledMapping.AutoRepeatRandomizeDelayEnabled,
            BlockOriginalMouseInput = disabledMapping.BlockOriginalMouseInput,
            Selected = disabledMapping.Selected,
        };

    private static NothingMappingVm MapNothingMapping(NothingMapping nothingMapping) =>
        new()
        {
            Id = nothingMapping.Id,
            ProfileId = nothingMapping.ProfileId,
            MouseButton = nothingMapping.MouseButton,
            Keys = nothingMapping.Keys,
            AutoRepeatDelay = nothingMapping.AutoRepeatDelay,
            AutoRepeatRandomizeDelayEnabled = nothingMapping.AutoRepeatRandomizeDelayEnabled,
            BlockOriginalMouseInput = nothingMapping.BlockOriginalMouseInput,
            Selected = nothingMapping.Selected,
        };

    private static SimulatedKeystrokeVm MapSimulatedKeystroke(SimulatedKeystroke simulatedKeystroke)
    {
        var viewModel = new SimulatedKeystrokeVm
        {
            Id = simulatedKeystroke.Id,
            ProfileId = simulatedKeystroke.ProfileId,
            MouseButton = simulatedKeystroke.MouseButton,
            Keys = simulatedKeystroke.Keys,
            AutoRepeatDelay = simulatedKeystroke.AutoRepeatDelay,
            AutoRepeatRandomizeDelayEnabled = simulatedKeystroke.AutoRepeatRandomizeDelayEnabled,
            BlockOriginalMouseInput = simulatedKeystroke.BlockOriginalMouseInput,
            Selected = simulatedKeystroke.Selected,
        };

        if (simulatedKeystroke.SimulatedKeystrokeType != null)
        {
            viewModel.SimulatedKeystrokeType = MapSimulatedKeystrokeType(
                simulatedKeystroke.SimulatedKeystrokeType
            );
        }

        return viewModel;
    }

    private static RightClickVm MapRightClick(RightClick rightClick) =>
        new()
        {
            Id = rightClick.Id,
            ProfileId = rightClick.ProfileId,
            MouseButton = rightClick.MouseButton,
            Keys = rightClick.Keys,
            AutoRepeatDelay = rightClick.AutoRepeatDelay,
            AutoRepeatRandomizeDelayEnabled = rightClick.AutoRepeatRandomizeDelayEnabled,
            BlockOriginalMouseInput = rightClick.BlockOriginalMouseInput,
            Selected = rightClick.Selected,
        };

    public static ButtonMapping MapToEntity(BaseButtonMappingVm buttonMappingVm)
    {
        return buttonMappingVm switch
        {
            DisabledMappingVm disabledMappingVm => MapDisabledMappingVm(disabledMappingVm),
            NothingMappingVm nothingMappingVm => MapNothingMappingVm(nothingMappingVm),
            SimulatedKeystrokeVm simulatedKeystrokeVm => MapSimulatedKeystrokeVm(
                simulatedKeystrokeVm
            ),
            RightClickVm rightClickVm => MapRightClickVm(rightClickVm),
            _ => throw new InvalidOperationException("Unknown button mapping VM type"),
        };
    }

    private static DisabledMapping MapDisabledMappingVm(DisabledMappingVm disabledMappingVm) =>
        new()
        {
            Id = disabledMappingVm.Id,
            ProfileId = disabledMappingVm.ProfileId,
            MouseButton = disabledMappingVm.MouseButton,
            Keys = disabledMappingVm.Keys,
            AutoRepeatDelay = disabledMappingVm.AutoRepeatDelay,
            AutoRepeatRandomizeDelayEnabled = disabledMappingVm.AutoRepeatRandomizeDelayEnabled,
            BlockOriginalMouseInput = disabledMappingVm.BlockOriginalMouseInput,
            Selected = disabledMappingVm.Selected,
        };

    private static NothingMapping MapNothingMappingVm(NothingMappingVm nothingMappingVm) =>
        new()
        {
            Id = nothingMappingVm.Id,
            ProfileId = nothingMappingVm.ProfileId,
            MouseButton = nothingMappingVm.MouseButton,
            Keys = nothingMappingVm.Keys,
            AutoRepeatDelay = nothingMappingVm.AutoRepeatDelay,
            AutoRepeatRandomizeDelayEnabled = nothingMappingVm.AutoRepeatRandomizeDelayEnabled,
            BlockOriginalMouseInput = nothingMappingVm.BlockOriginalMouseInput,
            Selected = nothingMappingVm.Selected,
        };

    private static SimulatedKeystroke MapSimulatedKeystrokeVm(
        SimulatedKeystrokeVm simulatedKeystrokeVm
    )
    {
        var entity = new SimulatedKeystroke
        {
            Id = simulatedKeystrokeVm.Id,
            ProfileId = simulatedKeystrokeVm.ProfileId,
            MouseButton = simulatedKeystrokeVm.MouseButton,
            Keys = simulatedKeystrokeVm.Keys,
            AutoRepeatDelay = simulatedKeystrokeVm.AutoRepeatDelay,
            AutoRepeatRandomizeDelayEnabled = simulatedKeystrokeVm.AutoRepeatRandomizeDelayEnabled,
            BlockOriginalMouseInput = simulatedKeystrokeVm.BlockOriginalMouseInput,
            Selected = simulatedKeystrokeVm.Selected,
        };

        // Assuming BaseSimulatedKeystrokeTypeVm has a method or constructor that can map SimulatedKeystrokeType to it
        if (simulatedKeystrokeVm.SimulatedKeystrokeType != null)
        {
            entity.SimulatedKeystrokeType = MapSimulatedKeystrokeTypeVm(
                simulatedKeystrokeVm.SimulatedKeystrokeType
            );
        }

        return entity;
    }

    private static RightClick MapRightClickVm(RightClickVm rightClickVm) =>
        new()
        {
            Id = rightClickVm.Id,
            ProfileId = rightClickVm.ProfileId,
            MouseButton = rightClickVm.MouseButton,
            Keys = rightClickVm.Keys,
            AutoRepeatDelay = rightClickVm.AutoRepeatDelay,
            AutoRepeatRandomizeDelayEnabled = rightClickVm.AutoRepeatRandomizeDelayEnabled,
            BlockOriginalMouseInput = rightClickVm.BlockOriginalMouseInput,
            Selected = rightClickVm.Selected,
        };

    public static BaseSimulatedKeystrokeTypeVm MapSimulatedKeystrokeType(
        SimulatedKeystrokeType? simulatedKeystrokeType
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
