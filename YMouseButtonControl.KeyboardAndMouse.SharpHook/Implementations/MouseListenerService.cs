using System;
using System.Reactive.Linq;
using System.Threading;
using ReactiveUI;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using MouseButton = YMouseButtonControl.DataAccess.Models.Enums.MouseButton;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class MouseListener : ReactiveObject, IMouseListener
{
    private readonly IReactiveGlobalHook _hook;
    private Thread _thread;
    private short _x;
    private short _y;

    public MouseListener(IReactiveGlobalHook hook)
    {
        _hook = hook;

        _hook.MousePressed.Subscribe(ConvertMousePressedEvent);
        _hook.MouseReleased.Subscribe(ConvertMouseReleasedEvent);
        _hook.MouseWheel.Subscribe(ConvertMouseWheelEvent);
        _hook.MouseMoved
            .Throttle(TimeSpan.FromMilliseconds(5))
            .Subscribe(ConvertMouseMovedEvent);
    }

    public short X
    {
        get => _x;
        private set => this.RaiseAndSetIfChanged(ref _x, value);
    }

    public short Y
    {
        get => _y;
        private set => this.RaiseAndSetIfChanged(ref _y, value);
    }

    public event EventHandler<NewMouseHookEventArgs> OnMousePressedEventHandler;
    public event EventHandler<NewMouseHookEventArgs> OnMouseReleasedEventHandler;
    public event EventHandler<NewMouseWheelEventArgs> OnMouseWheelEventHandler;
    public event EventHandler<NewMouseMovedEventArgs> OnMouseMovedEventHandler;

    public void Run()
    {
        _thread = new Thread(_hook.Run);
        _thread.Start();
    }

    private void ConvertMouseMovedEvent(MouseHookEventArgs e)
    {
        // X = e.Data.X;
        // Y = e.Data.Y;
        // Trace.WriteLine($"X: {X}, Y: {Y}");
        var args = new NewMouseMovedEventArgs(new NewMouseMoved { X = e.Data.X, Y = e.Data.Y });
        var handler = OnMouseMovedEventHandler;
        handler?.Invoke(this, args);
    }

    private void ConvertMouseWheelEvent(MouseWheelHookEventArgs e)
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

    private void ConvertMousePressedEvent(MouseHookEventArgs e)
    {
        var args = new NewMouseHookEventArgs((MouseButton)(e.Data.Button - 1));
        OnMousePressed(args);
    }

    private void ConvertMouseReleasedEvent(MouseHookEventArgs e)
    {
        var args = new NewMouseHookEventArgs((MouseButton)(e.Data.Button - 1));
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

    public void Dispose()
    {
        _hook.Dispose();

        _thread.Join();
    }
}