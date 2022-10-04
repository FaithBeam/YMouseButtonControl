using System.Threading;
using Avalonia;
using Avalonia.ReactiveUI;
using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DependencyInjection;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl;

internal static class Program
{
    public static void Main(string[] args)
    {
        var dataAccessConfig = new DataAccessConfiguration
        {
            UseInMemoryDatabase = false
        };
        RegisterDependencies(dataAccessConfig);
        var t = new Thread(StartMouseListening);
        t.Start();
        CheckForDefaultProfile();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        t.Join();
    }

    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    
    private static void RegisterDependencies(DataAccessConfiguration dataAccessConfiguration) =>
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current, dataAccessConfiguration);

    private static void CheckForDefaultProfile()
    {
        var defaultProfileService = Locator.Current.GetRequiredService<ICheckDefaultProfileService>();
        defaultProfileService.CheckDefaultProfile();
    }

    private static void StartMouseListening()
    {
        var ml = Locator.Current.GetRequiredService<IMouseListener>();
        ml.Run();
    }
}