using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedKeystrokesTypes;

public interface IRepeatedWhileButtonDownService
{
    void RepeatWhileDown(BaseButtonMappingVm mapping, MouseButtonState state);
}

public partial class RepeatedWhileButtonDownService(
    ILogger<RepeatedWhileButtonDownService> logger,
    IEventSimulatorService eventSimulatorService)
    : IRepeatedWhileButtonDownService
{
    private Thread? _thread;
    private bool _shouldStop;
    private readonly object _lock = new();
    private const int DefaultAutoRepeatDelay = 33;
    private static readonly Random Random = new();

    private static int GetThreadSleepTime(BaseButtonMappingVm mapping)
    {
        var delay = mapping.AutoRepeatDelay ?? DefaultAutoRepeatDelay;
        
        if (mapping.AutoRepeatRandomizeDelayEnabled is null ||
            !(bool)mapping.AutoRepeatRandomizeDelayEnabled)
        {
            return delay;
        }

        var randPercent = Random.NextDouble() * 0.1;
        var toAddOrSub = Math.Round(delay * randPercent);
        if (Random.Next(0, 2) == 0)
        {
            // Add
            return delay + (int)toAddOrSub;
        }

        // Subtract
        return delay - (int)toAddOrSub;
    }

    public void RepeatWhileDown(BaseButtonMappingVm mapping, MouseButtonState state)
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

                        var sleep = GetThreadSleepTime(mapping);
                        LogThreadSleepTime(logger, sleep);
                        Thread.Sleep(sleep);
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

    [LoggerMessage(LogLevel.Information, "Sleeping {Sleep} ms")]
    private static partial void LogThreadSleepTime(ILogger logger, int sleep);
}