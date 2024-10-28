using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace YMouseButtonControl;

public static class Program
{
    public static IServiceProvider? Container { get; set; }

    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    private static AppBuilder BuildAvaloniaApp() =>
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
