using System.Threading;
using System.Threading.Tasks;
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
    private static IMouseListener? _mouseListener;
    private static KeyboardSimulatorWorker _keyboardSimulatorWorker;
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
        CheckForDefaultProfile();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
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

    private static void CheckForDefaultProfile()
    {
        var defaultProfileService = Locator.Current.GetRequiredService<ICheckDefaultProfileService>();
        defaultProfileService.CheckDefaultProfile();
    }

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