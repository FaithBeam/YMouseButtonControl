using System.Diagnostics;
using Splat;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.DependencyInjection;

public static class BackgroundTasksRunner
{
    private static IMouseListener? _mouseListener;
    private static KeyboardSimulatorWorker _keyboardSimulatorWorker;
    // private static IPayloadInjectorService _payloadInjectorService;
    private static ILowLevelMouseHookService _lowLevelMouseHookService;
    private static ICurrentWindowService _currentWindowService;
    
    public static void Start(IReadonlyDependencyResolver resolver)
    {
        _mouseListener = resolver.GetRequiredService<IMouseListener>();
        _keyboardSimulatorWorker = resolver.GetRequiredService<KeyboardSimulatorWorker>();
        _lowLevelMouseHookService = resolver.GetRequiredService<ILowLevelMouseHookService>();
        _currentWindowService = resolver.GetRequiredService<ICurrentWindowService>();

        _mouseListener.Run();
        _keyboardSimulatorWorker.Run();
        _lowLevelMouseHookService.Run();
        _currentWindowService.Run();
    }

    public static void Stop()
    {
        Trace.WriteLine("Stopping cws");
        _currentWindowService.Dispose();
        Trace.WriteLine("Stopping llmhs");
        _lowLevelMouseHookService.Dispose();
        Trace.WriteLine("Stopping ksw");
        _keyboardSimulatorWorker.Dispose();
        Trace.WriteLine("Stopping ml");
        _mouseListener?.Dispose();
        Trace.WriteLine("Stopped all?");
    }
}