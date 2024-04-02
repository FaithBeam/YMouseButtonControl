using YMouseButtonControl.DataAccess.Models.Enums;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class NewMouseHookEventArgs : System.EventArgs
{
    public YMouseButton Button { get; }

    public NewMouseHookEventArgs(YMouseButton button)
    {
        Button = button;
    }
}
