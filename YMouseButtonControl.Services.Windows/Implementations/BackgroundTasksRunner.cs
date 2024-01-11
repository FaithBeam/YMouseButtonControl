using YMouseButtonControl.BackgroundTasks.Interfaces;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class BackgroundTasksRunner : IBackgroundTasksRunner
{
    private readonly IMouseListener _mouseListener;

    private readonly KeyboardSimulatorWorker _keyboardSimulatorWorker;

    // private  IPayloadInjectorService _payloadInjectorService;
    private readonly ILowLevelMouseHookService _lowLevelMouseHookService;
    private readonly ICurrentWindowService _currentWindowService;

    public BackgroundTasksRunner(IMouseListener mouseListener, KeyboardSimulatorWorker keyboardSimulatorWorker,
        ILowLevelMouseHookService lowLevelMouseHookService, ICurrentWindowService currentWindowService)
    {
        _mouseListener = mouseListener;
        _keyboardSimulatorWorker = keyboardSimulatorWorker;
        _lowLevelMouseHookService = lowLevelMouseHookService;
        _currentWindowService = currentWindowService;
    }

    public void Start()
    {
        _mouseListener.Run();
        _keyboardSimulatorWorker.Run();
        _lowLevelMouseHookService.Run();
        _currentWindowService.Run();
    }

    public void Stop()
    {
        _currentWindowService?.Dispose();
        _lowLevelMouseHookService?.Dispose();
        _keyboardSimulatorWorker?.Dispose();
        _mouseListener?.Dispose();
    }
}