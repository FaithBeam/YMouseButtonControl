using YMouseButtonControl.Services.Abstractions.Enums;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class NewMouseHookEventArgs : System.EventArgs
{
    public NewMouseButton Button { get; }

    public NewMouseHookEventArgs(NewMouseButton button)
    {
        Button = button;
    }
}