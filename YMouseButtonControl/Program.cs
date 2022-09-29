using Avalonia;
using Avalonia.ReactiveUI;
using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.Core.Services;
using YMouseButtonControl.DependencyInjection;

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
        CheckForDefaultProfile();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
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
}