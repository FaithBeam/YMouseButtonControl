using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Microsoft.Extensions.Logging;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.CurrentWindow;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.Profiles;
using static YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.Profiles.ListProfiles;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;

public interface IMouseListener : IDisposable
{
    IObservable<NewMouseHookMoveEventArgs> OnMouseMovedChanged { get; }
    IObservable<NewMouseHookDraggedEventArgs> OnMouseDragged { get; }
    IObservable<NewMouseHookEventArgs> OnMousePressedChanged { get; }
    IObservable<NewMouseHookEventArgs> OnMouseReleasedChanged { get; }
    IObservable<NewMouseWheelEventArgs> OnMouseWheelChanged { get; }
    void Run();
}

/// <summary>
/// Wrapper around sharphook for listening to mouse events
/// Converts mouse events to NewMouseHookEventArgs
/// </summary>
public partial class MouseListenerService : IMouseListener
{
    private readonly ILogger<MouseListenerService> _logger;
    private readonly IReactiveGlobalHook _hook;
    private readonly IGetCurrentWindow _currentWindowService;
    private Thread? _thread;
    private readonly IDisposable? _mouseMovedDisposable;
    private readonly IDisposable? _mousePressedDisposable;
    private readonly IDisposable? _mouseReleasedDisposable;
    private readonly IDisposable? _mouseWheelDisposable;
    private readonly IDisposable? _mouseDragDisposable;
    private readonly Subject<NewMouseHookEventArgs> _mousePressedSubject;
    private readonly Subject<NewMouseHookEventArgs> _mouseReleasedSubject;
    private readonly Subject<NewMouseHookMoveEventArgs> _mouseMovedSubject;
    private readonly Subject<NewMouseHookDraggedEventArgs> _mouseDraggedSubject;
    private readonly Subject<NewMouseWheelEventArgs> _mouseWheelSubject;
    private readonly ReadOnlyCollection<ProfileForMouseListener> _profiles;

    public MouseListenerService(
        ILogger<MouseListenerService> logger,
        IReactiveGlobalHook hook,
        ListProfiles.Handler listProfilesHandler,
        IGetCurrentWindow currentWindowService
    )
    {
        _logger = logger;
        _hook = hook;
        _currentWindowService = currentWindowService;
        _mousePressedSubject = new Subject<NewMouseHookEventArgs>();
        _mouseReleasedSubject = new Subject<NewMouseHookEventArgs>();
        _mouseMovedSubject = new Subject<NewMouseHookMoveEventArgs>();
        _mouseDraggedSubject = new Subject<NewMouseHookDraggedEventArgs>();
        _mouseWheelSubject = new Subject<NewMouseWheelEventArgs>();
        _profiles = listProfilesHandler.Execute();

        _mouseMovedDisposable = _hook
            .MouseMoved.Sample(TimeSpan.FromMilliseconds(100))
            .Where(x => !x.IsEventSimulated)
            .Subscribe(ConvertMouseMovedEvent);
        _mouseDragDisposable = _hook
            .MouseDragged.Sample(TimeSpan.FromMilliseconds(100))
            .Where(x => !x.IsEventSimulated)
            .Subscribe(ConvertMouseDraggedEvent);
        _mousePressedDisposable = _hook
            .MousePressed.Where(x => !x.IsEventSimulated)
            .Subscribe(ConvertMousePressedEvent);
        _mouseReleasedDisposable = _hook
            .MouseReleased.Where(x => !x.IsEventSimulated)
            .Subscribe(ConvertMouseReleasedEvent);
        _mouseWheelDisposable = _hook
            .MouseWheel.Where(x => !x.IsEventSimulated)
            .Subscribe(ConvertMouseWheelEvent);
    }

    public IObservable<NewMouseHookEventArgs> OnMousePressedChanged =>
        _mousePressedSubject.AsObservable();
    public IObservable<NewMouseHookEventArgs> OnMouseReleasedChanged =>
        _mouseReleasedSubject.AsObservable();
    public IObservable<NewMouseHookMoveEventArgs> OnMouseMovedChanged =>
        _mouseMovedSubject.AsObservable();

    public IObservable<NewMouseHookDraggedEventArgs> OnMouseDragged =>
        _mouseDraggedSubject.AsObservable();
    public IObservable<NewMouseWheelEventArgs> OnMouseWheelChanged =>
        _mouseWheelSubject.AsObservable();

    public void Run()
    {
        _thread = new Thread(() =>
        {
            LogStartup(_logger);
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
            YMouseButton.MouseButton1 => _profiles.Any(p =>
                p is { Checked: true, MouseButton1.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            YMouseButton.MouseButton2 => _profiles.Any(p =>
                p is { Checked: true, MouseButton2.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            YMouseButton.MouseButton3 => _profiles.Any(p =>
                p is { Checked: true, MouseButton3.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            YMouseButton.MouseButton4 => _profiles.Any(p =>
                p is { Checked: true, MouseButton4.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            YMouseButton.MouseButton5 => _profiles.Any(p =>
                p is { Checked: true, MouseButton5.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            YMouseButton.MouseWheelUp => _profiles.Any(p =>
                p is { Checked: true, MouseWheelUp.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            YMouseButton.MouseWheelDown => _profiles.Any(p =>
                p is { Checked: true, MouseWheelDown.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            YMouseButton.MouseWheelLeft => _profiles.Any(p =>
                p is { Checked: true, MouseWheelLeft.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            YMouseButton.MouseWheelRight => _profiles.Any(p =>
                p is { Checked: true, MouseWheelRight.BlockOriginalMouseInput: true }
                && (p.Process == "*" || (args.ActiveWindow?.Contains(p.Process) ?? false))
            ),
            _ => throw new ArgumentOutOfRangeException(),
        };

    private void ConvertMouseDraggedEvent(MouseHookEventArgs e)
    {
        if (
            e is null
            || e.RawEvent.Mask != ModifierMask.Button1
            || e.RawEvent.Mask != ModifierMask.Button2
            || e.RawEvent.Mask != ModifierMask.Button3
            || e.RawEvent.Mask != ModifierMask.Button4
            || e.RawEvent.Mask != ModifierMask.Button5
        )
        {
            return;
        }
        _mouseDraggedSubject.OnNext(
            new NewMouseHookDraggedEventArgs(
                e.Data.X,
                e.Data.Y,
                e.RawEvent.Mask switch
                {
                    ModifierMask.Button1 => YMouseButton.MouseButton1,
                    ModifierMask.Button2 => YMouseButton.MouseButton2,
                    ModifierMask.Button3 => YMouseButton.MouseButton3,
                    ModifierMask.Button4 => YMouseButton.MouseButton4,
                    ModifierMask.Button5 => YMouseButton.MouseButton5,
                    _ => throw new ArgumentOutOfRangeException(),
                }
            )
        );
    }

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

        LogTranslateRelease(_logger, e.Data.Button);
        LogActiveWindow(_logger, _currentWindowService.ForegroundWindow);
        var args = new NewMouseHookEventArgs(
            (YMouseButton)e.Data.Button,
            e.Data.X,
            e.Data.Y,
            _currentWindowService.ForegroundWindow
        );
        if (ShouldSuppressEvent(args))
        {
            LogSuppressingButtonRelease(_logger, e.Data.Button);
            e.SuppressEvent = true;
        }
        else
        {
            LogNotSuppressingButtonRelease(_logger, e.Data.Button);
        }
        _mouseReleasedSubject.OnNext(args);
    }

    private void ConvertMousePressedEvent(MouseHookEventArgs e)
    {
        if (e is null)
        {
            return;
        }
        LogTranslateButton(_logger, e.Data.Button);
        LogActiveWindow(_logger, _currentWindowService.ForegroundWindow);

        var args = new NewMouseHookEventArgs(
            (YMouseButton)e.Data.Button,
            e.Data.X,
            e.Data.Y,
            _currentWindowService.ForegroundWindow
        );
        if (ShouldSuppressEvent(args))
        {
            LogSuppressingButtonRelease(_logger, e.Data.Button);
            e.SuppressEvent = true;
        }
        else
        {
            LogNotSuppressingButtonRelease(_logger, e.Data.Button);
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
        _mouseDragDisposable?.Dispose();
        _mousePressedDisposable?.Dispose();
        _mouseReleasedDisposable?.Dispose();
        _mouseWheelDisposable?.Dispose();

        _hook.Dispose();

        _thread?.Join();
    }

    [LoggerMessage(LogLevel.Information, "Translate press {Button}")]
    private static partial void LogTranslateButton(ILogger logger, MouseButton button);

    [LoggerMessage(LogLevel.Information, "Suppressing {Button}: Press")]
    private static partial void LogSuppressingButtonPress(ILogger logger, MouseButton button);

    [LoggerMessage(LogLevel.Information, "Not suppressing {Button}: Press")]
    private static partial void LogNotSuppressingButtonPress(ILogger logger, MouseButton button);

    [LoggerMessage(LogLevel.Information, "Not suppressing {Button}: Release")]
    public static partial void LogNotSuppressingButtonRelease(ILogger logger, MouseButton button);

    [LoggerMessage(LogLevel.Information, "Suppressing {Button}: Release")]
    public static partial void LogSuppressingButtonRelease(ILogger logger, MouseButton button);

    [LoggerMessage(LogLevel.Information, "ACTIVE WINDOW {Foreground}")]
    private static partial void LogActiveWindow(ILogger logger, string foreground);

    [LoggerMessage(LogLevel.Information, "Translate release {Button}")]
    private static partial void LogTranslateRelease(ILogger logger, MouseButton button);

    [LoggerMessage(LogLevel.Information, "Starting mouse listener")]
    private static partial void LogStartup(ILogger logger);
}
