﻿using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DependencyInjection;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl;

internal static class Program
{
    private static IMouseListener? _mouseListener;
    private static KeyboardSimulatorWorker _keyboardSimulatorWorker;
    // private static IPayloadInjectorService _payloadInjectorService;
    private static ILowLevelMouseHookService _lowLevelMouseHookService;
    
    public static void Main(string[] args)
    {
        var dataAccessConfig = new DataAccessConfiguration
        {
            UseInMemoryDatabase = false
        };
        RegisterDependencies(dataAccessConfig);
        
        var t = new Thread(StartMouseListening);
        t.Start();
        
        var t2 = new Thread(StartKeyboardSimulator);
        t2.Start();

        _lowLevelMouseHookService = Locator.Current.GetRequiredService<ILowLevelMouseHookService>();
        
        // _payloadInjectorService = Locator.Current.GetRequiredService<IPayloadInjectorService>();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        
        _lowLevelMouseHookService.Dispose();
        StopKeyboardSimulator();
        t2.Join();
        StopMouseListening();
        t.Join();
    }

    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    
    private static void RegisterDependencies(DataAccessConfiguration dataAccessConfiguration) =>
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current, dataAccessConfiguration);

    private static void StartKeyboardSimulator()
    {
        _keyboardSimulatorWorker = Locator.Current.GetRequiredService<KeyboardSimulatorWorker>();
        _keyboardSimulatorWorker.Run();
    }

    private static void StopKeyboardSimulator()
    {
        _keyboardSimulatorWorker.Dispose();
    }

    private static void StopMouseListening()
    {
        _mouseListener?.Dispose();
    }

    private static void StartMouseListening()
    {
        _mouseListener = Locator.Current.GetRequiredService<IMouseListener>();
        _mouseListener.Run();
    }
}