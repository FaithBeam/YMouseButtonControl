using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;

public class NewMouseHookDraggedEventArgs(short x, short y, YMouseButton button) : System.EventArgs
{
    public short X { get; } = x;
    public short Y { get; } = y;
    public YMouseButton Button { get; } = button;
}
