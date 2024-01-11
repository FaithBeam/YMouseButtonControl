using System;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IMouseListener : IDisposable
{
    short X { get; }
    short Y { get; }
    event EventHandler<NewMouseHookEventArgs> OnMousePressedEventHandler;
    event EventHandler<NewMouseHookEventArgs> OnMouseReleasedEventHandler;
    event EventHandler<NewMouseWheelEventArgs> OnMouseWheelEventHandler;
    event EventHandler<NewMouseMovedEventArgs> OnMouseMovedEventHandler;
    void Run();
}