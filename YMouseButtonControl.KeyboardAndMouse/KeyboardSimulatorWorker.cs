using System;
using Serilog;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse;

public class KeyboardSimulatorWorker(
    IProfilesService profilesService,
    IMouseListener mouseListener,
    ISkipProfileService skipProfileService,
    IAsMouseButtonPressedService asMouseButtonPressedService,
    IAsMouseButtonReleasedService asMouseButtonReleasedService,
    IDuringMousePressAndReleaseService duringMousePressAndReleaseService,
    IRepeatedWhileButtonDownService repeatedWhileButtonDownService,
    IStickyRepeatService stickyRepeatService,
    IStickyHoldService stickyHoldService,
    IRightClick rightClick
) : IDisposable
{
    private readonly ILogger _log = Log.Logger.ForContext<KeyboardSimulatorWorker>();

    public void Run()
    {
        SubscribeToEvents();
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        mouseListener.OnMousePressedEventHandler += OnMousePressed;
        mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        mouseListener.OnMouseWheelEventHandler += OnMouseWheel;
    }

    private void UnsubscribeFromEvents()
    {
        mouseListener.OnMousePressedEventHandler -= OnMousePressed;
        mouseListener.OnMouseReleasedEventHandler -= OnMouseReleased;
        mouseListener.OnMouseWheelEventHandler -= OnMouseWheel;
    }

    private void OnMousePressed(object? sender, NewMouseHookEventArgs e)
    {
        foreach (var p in profilesService.Profiles)
        {
            if (skipProfileService.ShouldSkipProfile(p))
            {
                _log.Information("Skipped {Profile}", p.Name);
                continue;
            }

            _log.Information("{Profile}, Route {Button}", p.Name, e.Button);
            RouteMouseButton(e.Button, p, MouseButtonState.Pressed);
        }
    }

    private void OnMouseReleased(object? sender, NewMouseHookEventArgs e)
    {
        foreach (var p in profilesService.Profiles)
        {
            if (skipProfileService.ShouldSkipProfile(p))
            {
                continue;
            }

            RouteMouseButton(e.Button, p, MouseButtonState.Released);
        }
    }

    private void RouteMouseButton(YMouseButton yMouseButton, Profile p, MouseButtonState state)
    {
        switch (yMouseButton)
        {
            case YMouseButton.MouseButton1:
                RouteButtonMapping(p.MouseButton1, state);
                break;
            case YMouseButton.MouseButton2:
                RouteButtonMapping(p.MouseButton2, state);
                break;
            case YMouseButton.MouseButton3:
                RouteButtonMapping(p.MouseButton3, state);
                break;
            case YMouseButton.MouseButton4:
                RouteButtonMapping(p.MouseButton4, state);
                break;
            case YMouseButton.MouseButton5:
                RouteButtonMapping(p.MouseButton5, state);
                break;
            case YMouseButton.MouseWheelUp:
                RouteButtonMapping(p.MouseWheelUp, state);
                break;
            case YMouseButton.MouseWheelDown:
                RouteButtonMapping(p.MouseWheelDown, state);
                break;
            case YMouseButton.MouseWheelLeft:
                RouteButtonMapping(p.MouseWheelLeft, state);
                break;
            case YMouseButton.MouseWheelRight:
                RouteButtonMapping(p.MouseWheelRight, state);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RouteButtonMapping(IButtonMapping mapping, MouseButtonState state)
    {
        switch (mapping)
        {
            case SimulatedKeystrokes:
                RouteSimulatedKeystrokesType(mapping, state);
                break;
            case RightClick:
                rightClick.SimulateRightClick(state);
                break;
        }
    }

    private void RouteSimulatedKeystrokesType(IButtonMapping buttonMapping, MouseButtonState state)
    {
        switch (buttonMapping.SimulatedKeystrokesType)
        {
            case MouseButtonPressedActionType:
                asMouseButtonPressedService.AsMouseButtonPressed(buttonMapping, state);
                break;
            case MouseButtonReleasedActionType:
                asMouseButtonReleasedService.AsMouseButtonReleased(buttonMapping, state);
                break;
            case DuringMouseActionType:
                duringMousePressAndReleaseService.DuringMousePressAndRelease(buttonMapping, state);
                break;
            case RepeatedlyWhileButtonDownActionType:
                repeatedWhileButtonDownService.RepeatWhileDown(buttonMapping, state);
                break;
            case StickyRepeatActionType:
                stickyRepeatService.StickyRepeat(buttonMapping, state);
                break;
            case StickyHoldActionType:
                stickyHoldService.StickyHold(buttonMapping, state);
                break;
        }
    }

    private void OnMouseWheel(object? sender, NewMouseWheelEventArgs e) { }
}
