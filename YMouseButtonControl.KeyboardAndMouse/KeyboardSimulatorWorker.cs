using System;
using Serilog;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse;

public class KeyboardSimulatorWorker : IDisposable
{
    private readonly IMouseListener _mouseListener;
    private readonly IProfilesService _profilesService;
    private readonly IRouteMouseButtonService _routeMouseButtonService;
    private readonly ISkipProfileService _skipProfileService;
    private readonly ILogger _log = Log.Logger.ForContext<KeyboardSimulatorWorker>();

    public KeyboardSimulatorWorker(
        IProfilesService profilesService,
        IMouseListener mouseListener,
        IRouteMouseButtonService routeMouseButtonService,
        ISkipProfileService skipProfileService
    )
    {
        _profilesService = profilesService;
        _mouseListener = mouseListener;
        _routeMouseButtonService = routeMouseButtonService;
        _skipProfileService = skipProfileService;
    }

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
        _mouseListener.OnMousePressedEventHandler += OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler += OnMouseWheel;
    }

    private void UnsubscribeFromEvents()
    {
        _mouseListener.OnMousePressedEventHandler -= OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler -= OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler -= OnMouseWheel;
    }

    private void OnMousePressed(object? sender, NewMouseHookEventArgs e)
    {
        foreach (var p in _profilesService.Profiles)
        {
            if (_skipProfileService.ShouldSkipProfile(p))
            {
                _log.Information("Skipped {Profile}", p.Name);
                continue;
            }

            _log.Information("{Profile}, Route {Button}", p.Name, e.Button);
            _routeMouseButtonService.Route(e.Button, p, MouseButtonState.Pressed);
        }
    }

    private void OnMouseReleased(object? sender, NewMouseHookEventArgs e)
    {
        foreach (var p in _profilesService.Profiles)
        {
            if (_skipProfileService.ShouldSkipProfile(p))
            {
                continue;
            }

            _routeMouseButtonService.Route(e.Button, p, MouseButtonState.Released);
        }
    }

    private void OnMouseWheel(object? sender, NewMouseWheelEventArgs e) { }
}
