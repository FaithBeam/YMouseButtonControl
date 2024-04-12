using YMouseButtonControl.Services.Abstractions.Enums;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class NewMouseWheelEventArgs(WheelScrollDirection direction)
{
    public WheelScrollDirection Direction { get; } = direction;
}
