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
    private readonly object _lock = new ();
    private const int RepeatRateMs = 33;

    public RepeatedWhileButtonDownService(ISimulateKeyService simulateKeyService)
    {
        _simulateKeyService = simulateKeyService;
    }

    public void RepeatWhileDown(ISequencedMapping mapping, MouseButtonState state)
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
                    Thread.Sleep(RepeatRateMs);
                    _simulateKeyService.TapKeys(mapping.Sequence);
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