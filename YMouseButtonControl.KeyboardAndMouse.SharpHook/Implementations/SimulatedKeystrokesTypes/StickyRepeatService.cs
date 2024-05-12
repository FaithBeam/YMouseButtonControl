using System.Threading;
using Serilog;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class StickyRepeatService(IEventSimulatorService eventSimulatorService)
    : IStickyRepeatService
{
    private Thread? _thread;
    private bool _shouldStop;
    private readonly object _lock = new();
    private const int RepeatRateMs = 33;
    private CancellationTokenSource? _cts;
    private readonly ILogger _log = Log.Logger.ForContext<StickyRepeatService>();

    public void StickyRepeat(IButtonMapping mapping, MouseButtonState state)
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
                _log.Information("=====CANCELATION REQUESTED=======");
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

    private void StartThread(IButtonMapping mapping)
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
                        _log.Information("=====CANCELATION REQUESTED=======");
                        _cts.Cancel();
                        break;
                    }
                }

                eventSimulatorService.TapKeys(mapping.Keys, RepeatRateMs, _cts.Token);
            }
        });

        _thread.Start();
    }
}
