﻿using Avalonia;
using Avalonia.ReactiveUI;
using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DI;
using YMouseButtonControl.UI;

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

}