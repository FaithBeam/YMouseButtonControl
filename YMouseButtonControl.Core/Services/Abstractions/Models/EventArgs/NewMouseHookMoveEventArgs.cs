namespace YMouseButtonControl.Core.Services.Abstractions.Models.EventArgs;

public class NewMouseHookMoveEventArgs(short x, short y) : System.EventArgs
{
    public short X { get; } = x;
    public short Y { get; } = y;
}
