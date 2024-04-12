﻿using System;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class DuringMousePressAndReleaseService(ISimulateKeyService simulateKeyService)
    : IDuringMousePressAndReleaseService
{
    public void DuringMousePressAndRelease(IButtonMapping mapping, MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed:
                simulateKeyService.PressKeys(mapping.Keys);
                break;
            case MouseButtonState.Released:
                simulateKeyService.ReleaseKeys(mapping.Keys);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
