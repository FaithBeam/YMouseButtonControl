using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using MouseButton = YMouseButtonControl.DataAccess.Models.Enums.MouseButton;

namespace YMouseButtonControl.KeyboardAndMouse;

public class KeyboardSimulatorWorker : IDisposable
{
    private readonly IMouseListener _mouseListener;
    private readonly IProfilesService _profilesService;
    private readonly IKeyboardSimulator _keyboardSimulator;
    private readonly IProcessMonitorService _processMonitorService;
    private readonly ICurrentWindowService _currentWindowService;
    private string _currentWindow = string.Empty;
    private readonly object _lockObj = new();

    public KeyboardSimulatorWorker(IProfilesService profilesService, IMouseListener mouseListener,
        IKeyboardSimulator keyboardSimulator, IProcessMonitorService processMonitorService,
        ICurrentWindowService currentWindowService)
    {
        _profilesService = profilesService;
        _mouseListener = mouseListener;
        _keyboardSimulator = keyboardSimulator;
        _processMonitorService = processMonitorService;
        _currentWindowService = currentWindowService;
    }

    public void Run()
    {
        SubscribeToEvents();
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
        _mouseListener?.Dispose();
        _processMonitorService?.Dispose();
        _currentWindowService.Dispose();
    }

    private void SubscribeToEvents()
    {
        _mouseListener.OnMousePressedEventHandler += OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler += OnMouseWheel;
        _currentWindowService.OnActiveWindowChangedEventHandler += OnActiveWindowChanged;
    }

    private void OnActiveWindowChanged(object sender, ActiveWindowChangedEventArgs e)
    {
        lock (_lockObj)
        {
            _currentWindow = e.ActiveWindow;
        }
    }

    private void UnsubscribeFromEvents()
    {
        _mouseListener.OnMousePressedEventHandler -= OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler -= OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler -= OnMouseWheel;
        _currentWindowService.OnActiveWindowChangedEventHandler -= OnActiveWindowChanged;
    }

    private void OnMousePressed(object sender, NewMouseHookEventArgs e)
    {
        foreach (var p in _profilesService.Profiles)
        {
            if (ShouldSkipProfile(p))
            {
                continue;
            }
            
            RouteMouseButton(e.Button, p);
        }
    }

    // Returns whether or not this profile should be skipped on mouse events
    private bool ShouldSkipProfile(Profile p)
    {
        if (p.Process != "*" && !_currentWindow.Contains(p.Process))
        {
            return true;
        }

        // If the profile's checkbox is checked in the profiles list
        if (!p.Checked)
        {
            return true;
        }

        return false;
    }

    private void RouteMouseButton(MouseButton button, Profile p)
    {
        switch (button)
        {
            case MouseButton.MouseButton1:
                RouteButtonMapping(p.MouseButton1, true);
                break;
            case MouseButton.MouseButton2:
                RouteButtonMapping(p.MouseButton2, true);
                break;
            case MouseButton.MouseButton3:
                RouteButtonMapping(p.MouseButton3, true);
                break;
            case MouseButton.MouseButton4:
                RouteButtonMapping(p.MouseButton4, true);
                break;
            case MouseButton.MouseButton5:
                RouteButtonMapping(p.MouseButton5, true);
                break;
            case MouseButton.MouseWheelUp:
                RouteButtonMapping(p.MouseWheelUp, true);
                break;
            case MouseButton.MouseWheelDown:
                RouteButtonMapping(p.MouseWheelDown, true);
                break;
            case MouseButton.MouseWheelLeft:
                RouteButtonMapping(p.MouseWheelLeft, true);
                break;
            case MouseButton.MouseWheelRight:
                RouteButtonMapping(p.MouseWheelRight, true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RouteButtonMapping(IButtonMapping mapping, bool pressed)
    {
        switch (mapping)
        {
            case SimulatedKeystrokes:
                _keyboardSimulator.SimulatedKeystrokes(mapping, pressed);
                break;
        }
    }

    private void OnMouseReleased(object sender, NewMouseHookEventArgs e)
    {
    }

    private void OnMouseWheel(object sender, NewMouseWheelEventArgs e)
    {
    }
}