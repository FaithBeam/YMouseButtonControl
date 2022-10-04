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
        hook.MouseClicked += ConvertEvent;
    }

    public event EventHandler<NewMouseHookEventArgs> OnMouseClickedEventHandler;
    
    public void Run() => _hook.Run();

    private void ConvertEvent(object? sender, MouseHookEventArgs mouseHookEventArgs)
    {
        var args = new NewMouseHookEventArgs((NewMouseButton)mouseHookEventArgs.Data.Button);
        OnMouseClicked(args);
    }

    private void OnMouseClicked(NewMouseHookEventArgs e)
    {
        var handler = OnMouseClickedEventHandler;
        handler?.Invoke(this, e);
    }
}