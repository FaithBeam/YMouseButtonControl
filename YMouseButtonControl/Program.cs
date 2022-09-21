using Avalonia;
using Avalonia.ReactiveUI;
using YMouseButtonControl.UI;

namespace YMouseButtonControl;

internal static class Program
{
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}