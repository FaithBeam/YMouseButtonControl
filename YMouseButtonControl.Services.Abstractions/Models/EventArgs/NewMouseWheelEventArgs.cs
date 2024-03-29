using YMouseButtonControl.Services.Abstractions.Enums;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class NewMouseWheelEventArgs
{
    public WheelScrollDirection Direction { get; }

    public NewMouseWheelEventArgs(WheelScrollDirection direction)
    {
        Direction = direction;
    }
}
