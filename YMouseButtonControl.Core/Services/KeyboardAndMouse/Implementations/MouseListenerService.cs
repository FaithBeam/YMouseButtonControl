﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Serilog;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.Processes;
using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;

public interface IMouseListener : IDisposable
{
    IObservable<NewMouseHookMoveEventArgs> OnMouseMovedChanged { get; }
    IObservable<NewMouseHookEventArgs> OnMousePressedChanged { get; }
    IObservable<NewMouseHookEventArgs> OnMouseReleasedChanged { get; }
    IObservable<NewMouseWheelEventArgs> OnMouseWheelChanged { get; }
    void Run();
}

/// <summary>
/// Wrapper around sharphook for listening to mouse events
/// Converts mouse events to NewMouseHookEventArgs
/// </summary>
public class MouseListener : IMouseListener
{
    private readonly IReactiveGlobalHook _hook;
    private readonly IProfilesService _profilesService;
    private readonly ICurrentWindowService _currentWindowService;
    private readonly ILogger _log = Log.Logger.ForContext<MouseListener>();
    private Thread? _thread;
    private readonly IDisposable? _mouseMovedDisposable;
    private readonly IDisposable? _mousePressedDisposable;
    private readonly IDisposable? _mouseReleasedDisposable;
    private readonly IDisposable? _mouseWheelDisposable;
    private readonly Subject<NewMouseHookEventArgs> _mousePressedSubject;
    private readonly Subject<NewMouseHookEventArgs> _mouseReleasedSubject;
    private readonly Subject<NewMouseHookMoveEventArgs> _mouseMovedSubject;
    private readonly Subject<NewMouseWheelEventArgs> _mouseWheelSubject;

    public MouseListener(
        IReactiveGlobalHook hook,
        IProfilesService profilesService,
        ICurrentWindowService currentWindowService
    )
    {
        _hook = hook;
        _profilesService = profilesService;
        _currentWindowService = currentWindowService;
        _mousePressedSubject = new Subject<NewMouseHookEventArgs>();
        _mouseReleasedSubject = new Subject<NewMouseHookEventArgs>();
        _mouseMovedSubject = new Subject<NewMouseHookMoveEventArgs>();
        _mouseWheelSubject = new Subject<NewMouseWheelEventArgs>();

        _mouseMovedDisposable = _hook
            .MouseMoved.Sample(TimeSpan.FromMilliseconds(100))
            .Subscribe(ConvertMouseMovedEvent);
        _mousePressedDisposable = _hook.MousePressed.Subscribe(ConvertMousePressedEvent);
        _mouseReleasedDisposable = _hook.MouseReleased.Subscribe(ConvertMouseReleasedEvent);
        _mouseWheelDisposable = _hook.MouseWheel.Subscribe(ConvertMouseWheelEvent);
    }

    public IObservable<NewMouseHookEventArgs> OnMousePressedChanged =>
        _mousePressedSubject.AsObservable();
    public IObservable<NewMouseHookEventArgs> OnMouseReleasedChanged =>
        _mouseReleasedSubject.AsObservable();
    public IObservable<NewMouseHookMoveEventArgs> OnMouseMovedChanged =>
        _mouseMovedSubject.AsObservable();
    public IObservable<NewMouseWheelEventArgs> OnMouseWheelChanged =>
        _mouseWheelSubject.AsObservable();

    public void Run()
    {
        _thread = new Thread(() =>
        {
            _log.Information("Starting mouse listener");
            _hook.Run();
        });
        _thread.Start();
    }

    /// <summary>
    /// Whether to suppress the original mouse event from propagating
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private bool ShouldSuppressEvent(NewMouseHookEventArgs args) =>
        args.Button switch
        {
            YMouseButton.MouseButton1 => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseButton1.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            YMouseButton.MouseButton2 => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseButton2.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            YMouseButton.MouseButton3 => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseButton3.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            YMouseButton.MouseButton4 => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseButton4.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            YMouseButton.MouseButton5 => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseButton5.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            YMouseButton.MouseWheelUp => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseWheelUp.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            YMouseButton.MouseWheelDown => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseWheelDown.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            YMouseButton.MouseWheelLeft => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseWheelLeft.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            YMouseButton.MouseWheelRight => _profilesService.Profiles.Any(p =>
                p is { Checked: true, MouseWheelRight.BlockOriginalMouseInput: true }
                && (args.ActiveWindow?.Contains(p.Process) ?? false)
            ),
            _ => throw new ArgumentOutOfRangeException(),
        };

    private void ConvertMouseWheelEvent(MouseWheelHookEventArgs e)
    {
        if (e is null)
        {
            return;
        }
        switch (e.Data.Direction)
        {
            case MouseWheelScrollDirection.Vertical when e.Data.Rotation > 0:
                _mouseWheelSubject.OnNext(
                    new NewMouseWheelEventArgs(WheelScrollDirection.VerticalUp)
                );
                break;
            case MouseWheelScrollDirection.Vertical when e.Data.Rotation < 0:
                _mouseWheelSubject.OnNext(
                    new NewMouseWheelEventArgs(WheelScrollDirection.VerticalDown)
                );
                break;
            case MouseWheelScrollDirection.Vertical:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
            case MouseWheelScrollDirection.Horizontal when e.Data.Rotation > 0:
                _mouseWheelSubject.OnNext(
                    new NewMouseWheelEventArgs(WheelScrollDirection.HorizontalRight)
                );
                break;
            case MouseWheelScrollDirection.Horizontal when e.Data.Rotation < 0:
                _mouseWheelSubject.OnNext(
                    new NewMouseWheelEventArgs(WheelScrollDirection.HorizontalLeft)
                );
                break;
            case MouseWheelScrollDirection.Horizontal:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
            default:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
        }
    }

    private void ConvertMouseReleasedEvent(MouseHookEventArgs e)
    {
        if (e is null)
        {
            return;
        }
        _log.Information("Translate release {Button}", e.Data.Button);
        _log.Information("ACTIVE WINDOW {Foreground}", _currentWindowService.ForegroundWindow);
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
        else
        {
            _log.Information("Not suppressing {Button}: Release", e.Data.Button);
        }
        _mouseReleasedSubject.OnNext(args);
    }

    private void ConvertMousePressedEvent(MouseHookEventArgs e)
    {
        if (e is null)
        {
            return;
        }
        _log.Information("Translate press {Button}", e.Data.Button);
        _log.Information("ACTIVE WINDOW {Foreground}", _currentWindowService.ForegroundWindow);

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
        else
        {
            _log.Information("Not suppressing {Button}: Press", e.Data.Button);
        }
        _mousePressedSubject.OnNext(args);
    }

    private void ConvertMouseMovedEvent(MouseHookEventArgs e)
    {
        if (e is null)
        {
            return;
        }
        _mouseMovedSubject.OnNext(new NewMouseHookMoveEventArgs(e.Data.X, e.Data.Y));
    }

    public void Dispose()
    {
        _mouseMovedDisposable?.Dispose();
        _mousePressedDisposable?.Dispose();
        _mouseReleasedDisposable?.Dispose();
        _mouseWheelDisposable?.Dispose();

        _hook.Dispose();

        _thread?.Join();
    }
}
