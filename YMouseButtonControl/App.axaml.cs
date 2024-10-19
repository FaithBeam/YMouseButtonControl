using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.ViewModels.AppViewModel;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.Views;
using YMouseButtonControl.DataAccess.Context;
using YMouseButtonControl.DependencyInjection;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace YMouseButtonControl;

public partial class App : Application
{
    public IServiceProvider? Container { get; private set; }
    private IBackgroundTasksRunner? _backgroundTasksRunner;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Init()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.UseMicrosoftDependencyResolver();
                services.AddScoped(_ => configuration);
                services.AddScoped<YMouseButtonControlDbContext>();

                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
                resolver.InitializeReactiveUI();

                Bootstrapper.Register(services);

                services.AddLogging(lb => lb.AddFile(configuration.GetSection("Logging")));

                services.AddSingleton<
                    IActivationForViewFetcher,
                    AvaloniaActivationForViewFetcher
                >();
                services.AddSingleton<IPropertyBindingHook, AutoDataTemplateBindingHook>();
            })
            .ConfigureLogging(x =>
            {
#if !DEBUG
                if (!configuration.GetSection("Logging").Exists())
                {
                    x.ClearProviders();
                }
#endif
            })
            .Build();
        Container = host.Services;
        Container.UseMicrosoftDependencyResolver();

        Container.GetRequiredService<YMouseButtonControlDbContext>().Init();

        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Init();
        var logger =
            Container?.GetRequiredService<ILogger<App>>()
            ?? throw new Exception("Error activating logger");

        try
        {
            DataContext = Container?.GetRequiredService<IAppViewModel>();
            _backgroundTasksRunner = Container?.GetRequiredService<IBackgroundTasksRunner>();
            var settingsService =
                Container?.GetRequiredService<ISettingsService>()
                ?? throw new Exception($"Error retrieving {nameof(ISettingsService)}");
            var startMinimized =
                settingsService.GetSetting("StartMinimized") as SettingBoolVm
                ?? throw new Exception("Error retrieving setting StartMinimized");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (!startMinimized.BoolValue)
                {
                    var mainWindow = (Window)Container.GetRequiredService<IMainWindow>();
                    mainWindow.DataContext = Container.GetRequiredService<IMainWindowViewModel>();
                    desktop.MainWindow = mainWindow;
                }

                desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                desktop.Exit += (sender, args) =>
                {
                    _backgroundTasksRunner?.Dispose();
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        catch (Exception e)
        {
            LogError(logger, e.Message, e.InnerException?.Message, e.StackTrace);
            throw;
        }
    }

    [LoggerMessage(LogLevel.Error, "{Message}\r{InnerException}\r{StackTrace}")]
    private static partial void LogError(
        ILogger logger,
        string message,
        string? innerException,
        string? stackTrace
    );
}
