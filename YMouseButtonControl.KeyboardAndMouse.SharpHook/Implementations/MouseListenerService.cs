using System;
using System.Linq;
using System.Threading;
using Serilog;
using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

/// <summary>
/// Wrapper around sharphook for listening to mouse events
/// Converts mouse events to NewMouseHookEventArgs
/// </summary>
public class MouseListener : IMouseListener
{
    private readonly IGlobalHook _hook;
    private readonly IProfilesService _profilesService;
    private readonly ICurrentWindowService _currentWindowService;
    private readonly ILogger _log = Log.Logger.ForContext<MouseListener>();
    private Thread? _thread;

    public MouseListener(
        IGlobalHook hook,
        IProfilesService profilesService,
        ICurrentWindowService currentWindowService
    )
    {
        _hook = hook;
        _profilesService = profilesService;
        _currentWindowService = currentWindowService;

        SubscribeToEvents();
    }

    public event EventHandler<NewMouseHookEventArgs>? OnMousePressedEventHandler;
    public event EventHandler<NewMouseHookEventArgs>? OnMouseReleasedEventHandler;
    public event EventHandler<NewMouseWheelEventArgs>? OnMouseWheelEventHandler;

    public void Run()
    {
        _thread = new Thread(() =>
        {
            _log.Information("Starting mouse lister");
            _hook.Run();
        });
        _thread.Start();
    }

    private void ConvertMouseWheelEvent(object? sender, MouseWheelHookEventArgs e)
    {
        switch (e.Data.Direction)
        {
            case MouseWheelScrollDirection.Vertical when e.Data.Rotation > 0:
                OnMouseWheel(new NewMouseWheelEventArgs(WheelScrollDirection.VerticalUp));
                break;
            case MouseWheelScrollDirection.Vertical when e.Data.Rotation < 0:
                OnMouseWheel(new NewMouseWheelEventArgs(WheelScrollDirection.VerticalDown));
                break;
            case MouseWheelScrollDirection.Vertical:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
            case MouseWheelScrollDirection.Horizontal when e.Data.Rotation > 0:
                OnMouseWheel(new NewMouseWheelEventArgs(WheelScrollDirection.HorizontalRight));
                break;
            case MouseWheelScrollDirection.Horizontal when e.Data.Rotation < 0:
                OnMouseWheel(new NewMouseWheelEventArgs(WheelScrollDirection.HorizontalLeft));
                break;
            case MouseWheelScrollDirection.Horizontal:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
            default:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
        }
    }

    private void ConvertMousePressedEvent(object? sender, MouseHookEventArgs e)
    {
        _log.Information("Translate press {Button}", e.Data.Button);

        var args = new NewMouseHookEventArgs(
            (YMouseButton)e.Data.Button,
            e.Data.X,
            e.Data.Y,
            _currentWindowService.ForegroundWindow
        );
        if (ShouldSuppressEvent(args))
        {
            _log.Information("Suppressing {Button}: Press", e.Data.Button);
            e.SuppressEvent = true;
        }
        OnMousePressed(args);
    }

    private void ConvertMouseReleasedEvent(object? sender, MouseHookEventArgs e)
    {
        _log.Information("Translate release {Button}", e.Data.Button);
        var args = new NewMouseHookEventArgs(
            (YMouseButton)e.Data.Button,
            e.Data.X,
            e.Data.Y,
            _currentWindowService.ForegroundWindow
        );
        if (ShouldSuppressEvent(args))
        {
            _log.Information("Suppressing {Button}: Release", e.Data.Button);
            e.SuppressEvent = true;
        }
        OnMouseReleased(args);
    }

    private void OnMouseReleased(NewMouseHookEventArgs e)
    {
        var handler = OnMouseReleasedEventHandler;
        handler?.Invoke(this, e);
    }

    private void OnMousePressed(NewMouseHookEventArgs e)
    {
        var handler = OnMousePressedEventHandler;
        handler?.Invoke(this, e);
    }

    private void OnMouseWheel(NewMouseWheelEventArgs e)
    {
        var handler = OnMouseWheelEventHandler;
        handler?.Invoke(this, e);
    }

    private bool ShouldSuppressEvent(NewMouseHookEventArgs e) =>
        e.Button switch
        {
            YMouseButton.MouseButton1
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseButton1.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            YMouseButton.MouseButton2
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseButton2.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            YMouseButton.MouseButton3
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseButton3.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            YMouseButton.MouseButton4
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseButton4.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            YMouseButton.MouseButton5
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseButton5.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            YMouseButton.MouseWheelUp
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseWheelUp.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            YMouseButton.MouseWheelDown
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseWheelDown.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            YMouseButton.MouseWheelLeft
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseWheelLeft.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            YMouseButton.MouseWheelRight
                => _profilesService.Profiles.Any(p =>
                    p is { Checked: true, MouseWheelRight.MouseButtonDisabled: true }
                    && (e.ActiveWindow?.Contains(p.Process) ?? false)
                ),
            _ => throw new ArgumentOutOfRangeException()
        };

    private void SubscribeToEvents()
    {
        _hook.MousePressed += ConvertMousePressedEvent;
        _hook.MouseReleased += ConvertMouseReleasedEvent;
        _hook.MouseWheel += ConvertMouseWheelEvent;
    }

    private void UnsubscribeFromEvents()
    {
        _hook.MousePressed -= ConvertMousePressedEvent;
        _hook.MouseReleased -= ConvertMouseReleasedEvent;
        _hook.MouseWheel -= ConvertMouseWheelEvent;
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();

        _hook.Dispose();

        _thread?.Join();
    }
}
