using Avalonia;
using Avalonia.ReactiveUI;
using Serilog;

namespace YMouseButtonControl;

internal static class Program
{
    public static void Main(string[] args)
    {
        using var log = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {SourceContext} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.File("YMouseButtonControl.log")
            .CreateLogger();
        Log.Logger = log;

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
