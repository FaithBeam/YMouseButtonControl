using System;
using Serilog;
using YMouseButtonControl.Core.DataAccess.Models.Enums;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.Profiles.Interfaces;
using YMouseButtonControl.Core.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Core.KeyboardAndMouse;

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
        // mouseListener.OnMouseMovedEventHandler += OnMouseMoved;
        mouseListener.OnMousePressedEventHandler += OnMousePressed;
        mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        mouseListener.OnMouseWheelEventHandler += OnMouseWheel;
    }

    // private void OnMouseMoved(object? sender, NewMouseHookMoveEventArgs e)
    // {
    //     _log.Information("{X}:{Y}", e.X, e.Y);
    // }

    private void UnsubscribeFromEvents()
    {
        // mouseListener.OnMouseMovedEventHandler -= OnMouseMoved;
        mouseListener.OnMousePressedEventHandler -= OnMousePressed;
        mouseListener.OnMouseReleasedEventHandler -= OnMouseReleased;
        mouseListener.OnMouseWheelEventHandler -= OnMouseWheel;
    }

    private void OnMousePressed(object? sender, NewMouseHookEventArgs e)
    {
        foreach (var p in profilesService.Profiles)
        {
            if (skipProfileService.ShouldSkipProfile(p, e))
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
            if (skipProfileService.ShouldSkipProfile(p, e))
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
