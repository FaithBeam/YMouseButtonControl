using Avalonia;
using Avalonia.ReactiveUI;
using Serilog;
using Serilog.Templates;
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
        using var log = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {SourceContext} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.File("YMouseButtonControl.log")
            .CreateLogger();
        Log.Logger = log;

        var dataAccessConfig = new DataAccessConfiguration { UseInMemoryDatabase = false };
        RegisterDependencies(dataAccessConfig);

        using var backgroundTasksRunner =
            Locator.Current.GetRequiredService<IBackgroundTasksRunner>();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    private static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace().UseReactiveUI();

    private static void RegisterDependencies(DataAccessConfiguration dataAccessConfiguration) =>
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current, dataAccessConfiguration);
}
