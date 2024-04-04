﻿using YMouseButtonControl.BackgroundTasks.Interfaces;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.Services.Windows.Implementations;

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

        _mouseListener.Run();
        _keyboardSimulatorWorker.Run();
    }

    public void Dispose()
    {
        _keyboardSimulatorWorker.Dispose();
        _mouseListener.Dispose();
    }
}
