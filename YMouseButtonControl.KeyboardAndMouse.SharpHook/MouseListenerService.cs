using System;
using SharpHook;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook;

public class MouseListener : IMouseListener
{
    private TaskPoolGlobalHook _hook;

    public MouseListener(TaskPoolGlobalHook hook)
    {
        _hook = hook;
        hook.MousePressed += ConvertMousePressedEvent;
        hook.MouseReleased += ConvertMouseReleasedEvent;
    }

    public event EventHandler<NewMouseHookEventArgs> OnMousePressedEventHandler;
    public event EventHandler<NewMouseHookEventArgs> OnMouseReleasedEventHandler;
    
    public void Run() => _hook.Run();

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
}