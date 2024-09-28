using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;

public class NewMouseWheelEventArgs(WheelScrollDirection direction)
{
    public WheelScrollDirection Direction { get; } = direction;
}
