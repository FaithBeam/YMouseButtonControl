using System.Threading;
using Microsoft.Extensions.Logging;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedKeystrokesTypes;

public interface IStickyRepeatService
{
    void StickyRepeat(BaseButtonMappingVm mapping, MouseButtonState state);
}

public partial class StickyRepeatService(
    ILogger<StickyRepeatService> logger,
    IEventSimulatorService eventSimulatorService
) : IStickyRepeatService
{
    private Thread? _thread;
    private bool _shouldStop;
    private readonly object _lock = new();
    private const int RepeatRateMs = 33;
    private CancellationTokenSource? _cts;

    public void StickyRepeat(BaseButtonMappingVm mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Pressed)
        {
            return;
        }

        if (_thread is null)
        {
            StartThread(mapping);
        }
        else if (_thread.IsAlive)
        {
            lock (_lock)
            {
                LogCancellationRequested(logger);
                _cts?.Cancel();
                _shouldStop = true;
            }
            _thread.Join();
            lock (_lock)
            {
                _shouldStop = false;
            }
        }
        else
        {
            StartThread(mapping);
        }
    }

    private void StartThread(BaseButtonMappingVm mapping)
    {
        _cts = new CancellationTokenSource();
        _thread = new Thread(() =>
        {
            while (true)
            {
                lock (_lock)
                {
                    if (_shouldStop)
                    {
                        LogCancellationRequested(logger);
                        _cts.Cancel();
                        break;
                    }
                }

                eventSimulatorService.TapKeys(mapping.Keys, RepeatRateMs, _cts.Token);
            }
        });

        _thread.Start();
    }

    [LoggerMessage(LogLevel.Information, "=====CANCELLATION REQUESTED=======")]
    private static partial void LogCancellationRequested(ILogger logger);
}
