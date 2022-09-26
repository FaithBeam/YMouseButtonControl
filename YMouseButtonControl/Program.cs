using Avalonia;
using Avalonia.ReactiveUI;
using Splat;
using YMouseButtonControl.Core.DI;
using YMouseButtonControl.UI;

namespace YMouseButtonControl;

internal static class Program
{
    public static void Main(string[] args)
    {
        RegisterDependencies();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    
    private static void RegisterDependencies() =>
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

}