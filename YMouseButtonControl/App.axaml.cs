using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.ViewModels.AppViewModel;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.Views;
using YMouseButtonControl.DataAccess.Context;
using YMouseButtonControl.DependencyInjection;

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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.UseMicrosoftDependencyResolver();
                services.AddDbContext<YMouseButtonControlDbContext>(o =>
                {
                    o.UseSqlite(
                        configuration.GetConnectionString("YMouseButtonControlContext"),
                        sqliteOptionsAction: so =>
                            so.MigrationsAssembly("YMouseButtonControl.DataAccess")
                    );
                });
                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
                resolver.InitializeReactiveUI();

                services.AddAutoMapper(typeof(MappingProfile));
                Bootstrapper.Register(services);

                services.AddSingleton<
                    IActivationForViewFetcher,
                    AvaloniaActivationForViewFetcher
                >();
                services.AddSingleton<IPropertyBindingHook, AutoDataTemplateBindingHook>();
            })
            .Build();
        Container = host.Services;
        Container.UseMicrosoftDependencyResolver();
#if (!DEBUG)
        // ensure db is created
        Container.GetRequiredService<YMouseButtonControlDbContext>().Database.Migrate();
#endif
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
            settingsService.GetSetting("StartMinimized") as SettingBoolVm
            ?? throw new Exception($"Error retrieving setting StartMinimized");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (!startMinimized.Value)
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
