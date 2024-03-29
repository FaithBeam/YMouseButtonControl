﻿using System;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class DuringMousePressAndReleaseService : IDuringMousePressAndReleaseService
{
    private readonly ISimulateKeyService _simulateKeyService;

    public DuringMousePressAndReleaseService(ISimulateKeyService simulateKeyService)
    {
        _simulateKeyService = simulateKeyService;
    }

    public void DuringMousePressAndRelease(ISequencedMapping mapping, MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed:
                _simulateKeyService.PressKeys(mapping.Sequence);
                break;
            case MouseButtonState.Released:
                _simulateKeyService.ReleaseKeys(mapping.Sequence);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}