﻿using System.Threading;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class StickyRepeatService : IStickyRepeatService
{
    private readonly ISimulateKeyService _simulateKeyService;
    private Thread _thread;
    private bool _shouldStop;
    private readonly object _lock = new ();
    private const int _repeatRateMs = 33;

    public StickyRepeatService(ISimulateKeyService simulateKeyService)
    {
        _simulateKeyService = simulateKeyService;
    }

    public void StickyRepeat(ISequencedMapping mapping, MouseButtonState state)
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

    private void StartThread(ISequencedMapping mapping)
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
                
                Thread.Sleep(_repeatRateMs);
                _simulateKeyService.TapKeys(mapping.Sequence);
            }
        });
            
        _thread.Start();
    }
}