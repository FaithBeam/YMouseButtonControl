using System;
using System.Threading;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class RepeatedWhileButtonDownService(IEventSimulatorService eventSimulatorService)
    : IRepeatedWhileButtonDownService
{
    private Thread? _thread;
    private bool _shouldStop;
    private readonly object _lock = new();
    private const int RepeatRateMs = 33;

    public void RepeatWhileDown(IButtonMapping mapping, MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed when _thread is not null && _thread.IsAlive:
                throw new Exception("Thread entered bad state");
            case MouseButtonState.Pressed:
                _thread = new Thread(() =>
                {
                    while (true)
                    {
                        lock (_lock)
                        {
                            if (_shouldStop)
                            {
                                break;
                            }
                        }
                        Thread.Sleep(RepeatRateMs);
                        eventSimulatorService.TapKeys(mapping.Keys);
                    }
                });

                _thread.Start();
                break;
            case MouseButtonState.Released:
            {
                lock (_lock)
                {
                    _shouldStop = true;
                }
                _thread?.Join();
                lock (_lock)
                {
                    _shouldStop = false;
                }

                break;
            }
        }
    }
}
