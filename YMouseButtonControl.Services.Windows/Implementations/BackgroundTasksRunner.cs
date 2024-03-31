using YMouseButtonControl.BackgroundTasks.Interfaces;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class BackgroundTasksRunner : IBackgroundTasksRunner
{
    private readonly IMouseListener _mouseListener;
    private readonly KeyboardSimulatorWorker _keyboardSimulatorWorker;
    private readonly ICurrentWindowService _currentWindowService;

    public BackgroundTasksRunner(
        IMouseListener mouseListener,
        KeyboardSimulatorWorker keyboardSimulatorWorker,
        ICurrentWindowService currentWindowService
    )
    {
        _mouseListener = mouseListener;
        _keyboardSimulatorWorker = keyboardSimulatorWorker;
        _currentWindowService = currentWindowService;

        _mouseListener.Run();
        _keyboardSimulatorWorker.Run();
        _currentWindowService.Run();
    }

    public void Dispose()
    {
        _currentWindowService.Dispose();
        _keyboardSimulatorWorker.Dispose();
        _mouseListener.Dispose();
    }
}
