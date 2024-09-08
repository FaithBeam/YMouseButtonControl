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
using YMouseButtonControl.Core.Services.BackgroundTasks;
using YMouseButtonControl.Core.ViewModels.Implementations;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.DependencyInjection;
using YMouseButtonControl.Views;

namespace YMouseButtonControl;

public class App : Application
{
    public IServiceProvider? Container { get; private set; }
    private IBackgroundTasksRunner? _backgroundTasksRunner;

    public App()
    {
        if (OperatingSystem.IsWindows())
        {
            DataContext = new AppViewModel(new Services.Windows.Services.StartupInstallerService());
        }
        else if (OperatingSystem.IsMacOS())
        {
            DataContext = new AppViewModel(new Services.MacOS.Services.StartupInstallerService());
        }
        else if (OperatingSystem.IsLinux())
        {
            DataContext = new AppViewModel(new Services.Linux.Services.StartupInstallerService());
        }
        else
        {
            throw new Exception(
                $"Unsupported operating system: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}"
            );
        }
    }

    public override void Initialize()
    {
        Init();
        _backgroundTasksRunner = Container?.GetRequiredService<IBackgroundTasksRunner>();
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
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Container?.GetRequiredService<IMainWindowViewModel>(),
            };
            // Prevent the application from exiting and hide the window when the user presses the X button
            desktop.MainWindow.Closing += (s, e) =>
            {
                ((Window)s!).Hide();
                e.Cancel = true;
            };

            desktop.Exit += (sender, args) =>
            {
                _backgroundTasksRunner?.Dispose();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
