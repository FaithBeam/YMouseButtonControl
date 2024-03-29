using Avalonia;
using Avalonia.ReactiveUI;
using Splat;
using YMouseButtonControl.BackgroundTasks.Interfaces;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DependencyInjection;
using YMouseButtonControl.Services.Windows.Implementations;

namespace YMouseButtonControl;

internal static class Program
{
    public static void Main(string[] args)
    {
        var dataAccessConfig = new DataAccessConfiguration { UseInMemoryDatabase = false };
        RegisterDependencies(dataAccessConfig);

        var backgroundTasksRunner = Locator.Current.GetRequiredService<IBackgroundTasksRunner>();
        backgroundTasksRunner.Start();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        backgroundTasksRunner.Stop();
    }

    private static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace().UseReactiveUI();

    private static void RegisterDependencies(DataAccessConfiguration dataAccessConfiguration) =>
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current, dataAccessConfiguration);
}
