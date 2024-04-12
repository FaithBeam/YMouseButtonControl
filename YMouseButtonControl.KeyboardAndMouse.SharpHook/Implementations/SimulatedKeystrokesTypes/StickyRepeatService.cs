using System.Threading;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class StickyRepeatService(ISimulateKeyService simulateKeyService) : IStickyRepeatService
{
    private Thread? _thread;
    private bool _shouldStop;
    private readonly object _lock = new();
    private const int RepeatRateMs = 33;

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
                simulateKeyService.TapKeys(mapping.Keys);
            }
        });

        _thread.Start();
    }
}
