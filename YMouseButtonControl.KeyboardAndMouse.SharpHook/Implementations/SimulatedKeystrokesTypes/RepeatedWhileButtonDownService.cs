using System;
using System.Threading;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class RepeatedWhileButtonDownService : IRepeatedWhileButtonDownService
{
    private readonly ISimulateKeyService _simulateKeyService;
    private Thread _thread;
    private bool _shouldStop;
    private object _lock = new();
    private const int _repeatRateMs = 33;

    public RepeatedWhileButtonDownService(ISimulateKeyService simulateKeyService)
    {
        _simulateKeyService = simulateKeyService;
    }

    public void RepeatWhileDown(IButtonMapping mapping, MouseButtonState state)
    {
        if (state == MouseButtonState.Pressed)
        {
            if (_thread is not null && _thread.IsAlive)
            {
                throw new Exception("Thread entered bad state");
            }

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
                    Thread.Sleep(_repeatRateMs);
                    _simulateKeyService.TapKeys(mapping.Keys);
                }
            });

            _thread.Start();
        }
        else if (state == MouseButtonState.Released)
        {
            lock (_lock)
            {
                _shouldStop = true;
            }
            _thread.Join();
            lock (_lock)
            {
                _shouldStop = false;
            }
        }
    }
}
