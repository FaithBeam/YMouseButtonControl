using YMouseButtonControl.BackgroundTasks.Interfaces;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.Services.MacOS.Implementations;

public class BackgroundTasksRunner : IBackgroundTasksRunner
{
    private readonly IMouseListener _mouseListener;

    private readonly KeyboardSimulatorWorker _keyboardSimulatorWorker;

    public BackgroundTasksRunner(
        IMouseListener mouseListener,
        KeyboardSimulatorWorker keyboardSimulatorWorker
    )
    {
        _mouseListener = mouseListener;
        _keyboardSimulatorWorker = keyboardSimulatorWorker;
    }

    public void Start()
    {
        _mouseListener.Run();
        _keyboardSimulatorWorker.Run();
    }

    public void Stop()
    {
        _keyboardSimulatorWorker?.Dispose();
        _mouseListener?.Dispose();
    }
}
