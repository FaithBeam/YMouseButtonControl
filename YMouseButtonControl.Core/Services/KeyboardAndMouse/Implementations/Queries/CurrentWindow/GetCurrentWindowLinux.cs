namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.CurrentWindow;

public class GetCurrentWindowLinux : IGetCurrentWindow
{
    public string ForegroundWindow => "*";
}
