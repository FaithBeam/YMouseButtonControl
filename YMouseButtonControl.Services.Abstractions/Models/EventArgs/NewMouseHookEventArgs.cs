using YMouseButtonControl.DataAccess.Models.Enums;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class NewMouseHookEventArgs : System.EventArgs
{
    public MouseButton Button { get; }

    public NewMouseHookEventArgs(MouseButton button)
    {
        Button = button;
    }
}