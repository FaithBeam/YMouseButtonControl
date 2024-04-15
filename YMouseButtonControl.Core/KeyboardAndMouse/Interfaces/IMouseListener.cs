using System;
using YMouseButtonControl.Core.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface IMouseListener : IDisposable
{
    event EventHandler<NewMouseHookMoveEventArgs> OnMouseMovedEventHandler;
    event EventHandler<NewMouseHookEventArgs> OnMousePressedEventHandler;
    event EventHandler<NewMouseHookEventArgs> OnMouseReleasedEventHandler;
    event EventHandler<NewMouseWheelEventArgs> OnMouseWheelEventHandler;
    void Run();
}
