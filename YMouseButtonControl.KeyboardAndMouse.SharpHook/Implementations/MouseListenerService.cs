using System;
using System.Threading;
using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using MouseButton = YMouseButtonControl.DataAccess.Models.Enums.MouseButton;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class MouseListener : IMouseListener
{
    private readonly IGlobalHook _hook;
    private Thread? _thread;

    public MouseListener(IGlobalHook hook)
    {
        _hook = hook;

        SubscribeToEvents();
    }

    public event EventHandler<NewMouseHookEventArgs>? OnMousePressedEventHandler;
    public event EventHandler<NewMouseHookEventArgs>? OnMouseReleasedEventHandler;
    public event EventHandler<NewMouseWheelEventArgs>? OnMouseWheelEventHandler;

    public void Run()
    {
        _thread = new Thread(() =>
        {
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
        var args = new NewMouseHookEventArgs((MouseButton)(e.Data.Button - 1));
        OnMousePressed(args);
    }

    private void ConvertMouseReleasedEvent(object? sender, MouseHookEventArgs e)
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
