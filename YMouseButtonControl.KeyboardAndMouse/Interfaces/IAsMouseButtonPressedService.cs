﻿using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IAsMouseButtonPressedService
{
    void AsMouseButtonPressed(ISequencedMapping mapping, MouseButtonState state);
}