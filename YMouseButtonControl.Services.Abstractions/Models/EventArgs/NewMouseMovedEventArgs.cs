namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class NewMouseMovedEventArgs : System.EventArgs
{
    public NewMouseMoved Moved { get; }

    public NewMouseMovedEventArgs(NewMouseMoved moved)
    {
        Moved = moved;
    }
}