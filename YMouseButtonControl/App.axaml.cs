using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.Core;
using YMouseButtonControl.Core.Profiles.Implementations;
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Core.ViewModels.Interfaces;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.Views;
using YMouseButtonControl.DependencyInjection;
using YMouseButtonControl.Views;

namespace YMouseButtonControl;

public class App : Application
{
    public IServiceProvider? Container { get; private set; }
    private IBackgroundTasksRunner? _backgroundTasksRunner;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Init()
    {
        var dataAccessConfig = new DataAccessConfiguration { UseInMemoryDatabase = false };
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.UseMicrosoftDependencyResolver();
                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
                resolver.InitializeReactiveUI();

                Bootstrapper.Register(services, dataAccessConfig);

                services.AddSingleton<
                    IActivationForViewFetcher,
                    AvaloniaActivationForViewFetcher
                >();
                services.AddSingleton<IPropertyBindingHook, AutoDataTemplateBindingHook>();
            })
            .Build();
        Container = host.Services;
        Container.UseMicrosoftDependencyResolver();
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Init();
        DataContext = Container?.GetRequiredService<IAppViewModel>();
        _backgroundTasksRunner = Container?.GetRequiredService<IBackgroundTasksRunner>();
        var settingsService =
            Container?.GetRequiredService<ISettingsService>()
            ?? throw new Exception($"Error retrieving {nameof(ISettingsService)}");
        var startMinimized =
            settingsService.GetSetting("StartMinimized")
            ?? throw new Exception($"Error retrieving setting StartMinimized");
        var startMinimizedValue = bool.Parse(startMinimized.Value ?? "false");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (!startMinimizedValue)
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
}
