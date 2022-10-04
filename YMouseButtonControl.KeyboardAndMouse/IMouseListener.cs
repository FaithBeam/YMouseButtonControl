using System;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse;

public interface IMouseListener
{
    event EventHandler<NewMouseHookEventArgs> OnMouseClickedEventHandler;
    void Run();
}