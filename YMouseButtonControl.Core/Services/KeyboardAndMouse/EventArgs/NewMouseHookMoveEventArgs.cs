namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;

public class NewMouseHookMoveEventArgs(short x, short y) : System.EventArgs
{
    public short X { get; } = x;
    public short Y { get; } = y;
}
