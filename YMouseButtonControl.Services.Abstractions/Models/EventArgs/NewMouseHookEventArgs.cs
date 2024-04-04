using YMouseButtonControl.DataAccess.Models.Enums;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class NewMouseHookEventArgs(YMouseButton button, short x, short y, string? activeWindow)
    : System.EventArgs
{
    public YMouseButton Button { get; } = button;
    public short X { get; } = x;
    public short Y { get; } = y;
    public string? ActiveWindow { get; } = activeWindow;
}
