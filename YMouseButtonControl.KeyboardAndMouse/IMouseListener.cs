using System;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse;

public interface IMouseListener : IDisposable
{
    event EventHandler<NewMouseHookEventArgs> OnMousePressedEventHandler;
    event EventHandler<NewMouseHookEventArgs> OnMouseReleasedEventHandler;
    event EventHandler<NewMouseWheelEventArgs> OnMouseWheelEventHandler;
    void Run();
}