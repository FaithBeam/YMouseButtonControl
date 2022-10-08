﻿using System;
using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook;

public class MouseListener : IMouseListener
{
    private readonly TaskPoolGlobalHook _hook;

    public MouseListener(TaskPoolGlobalHook hook)
    {
        _hook = hook;
        
        SubscribeToEvents();
    }

    public event EventHandler<NewMouseHookEventArgs> OnMousePressedEventHandler;
    public event EventHandler<NewMouseHookEventArgs> OnMouseReleasedEventHandler;
    public event EventHandler<NewMouseWheelEventArgs> OnMouseWheelEventHandler;
    
    public void Run() => _hook.Run();

    private void ConvertMouseWheelEvent(object sender, MouseWheelHookEventArgs e)
    {
        switch (e.Data.Direction)
        {
            case MouseWheelScrollDirection.VerticalDirection when e.Data.Rotation > 0:
                OnMouseWheel(new NewMouseWheelEventArgs(WheelScrollDirection.VerticalUp));
                break;
            case MouseWheelScrollDirection.VerticalDirection when e.Data.Rotation < 0:
                OnMouseWheel(new NewMouseWheelEventArgs(WheelScrollDirection.VerticalDown));
                break;
            case MouseWheelScrollDirection.VerticalDirection:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
            case MouseWheelScrollDirection.HorizontalDirection when e.Data.Rotation > 0:
                OnMouseWheel(new NewMouseWheelEventArgs(WheelScrollDirection.HorizontalRight));
                break;
            case MouseWheelScrollDirection.HorizontalDirection when e.Data.Rotation < 0:
                OnMouseWheel(new NewMouseWheelEventArgs(WheelScrollDirection.HorizontalLeft));
                break;
            case MouseWheelScrollDirection.HorizontalDirection:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
            default:
                throw new ArgumentOutOfRangeException($"{e.Data.Direction}\t{e.Data.Rotation}");
        }
    }

    private void ConvertMousePressedEvent(object sender, MouseHookEventArgs e)
    {
        var args = new NewMouseHookEventArgs((NewMouseButton)e.Data.Button);
        OnMousePressed(args);
    }

    private void ConvertMouseReleasedEvent(object sender, MouseHookEventArgs e)
    {
        var args = new NewMouseHookEventArgs((NewMouseButton)e.Data.Button);
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
    }
}