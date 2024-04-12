using YMouseButtonControl.Core.Services.Abstractions.Enums;

namespace YMouseButtonControl.Core.Services.Abstractions.Models.EventArgs;

public class NewMouseWheelEventArgs(WheelScrollDirection direction)
{
    public WheelScrollDirection Direction { get; } = direction;
}
