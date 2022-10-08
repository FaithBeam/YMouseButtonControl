using System;
using System.Collections.Generic;
using System.Linq;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse;

public class KeyboardSimulatorWorker : IDisposable
{
    private readonly IMouseListener _mouseListener;
    private readonly IProfilesService _profilesService;
    private readonly IKeyboardSimulator _keyboardSimulator;
    private Dictionary<NewMouseButton, List<IButtonMapping>> _hotkeys;

    public KeyboardSimulatorWorker(IProfilesService profilesService, IMouseListener mouseListener, IKeyboardSimulator keyboardSimulator)
    {
        _profilesService = profilesService;
        _mouseListener = mouseListener;
        _keyboardSimulator = keyboardSimulator;
    }

    private void SubscribeToEvents()
    {
        _mouseListener.OnMousePressedEventHandler += OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
    }

    private void UnsubscribeFromEvents()
    {
        _mouseListener.OnMousePressedEventHandler -= OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler -= OnMouseReleased;
    }

    private void OnMousePressed(object sender, NewMouseHookEventArgs e)
    {
        foreach (var buttonMapping in _hotkeys[e.Button])
        {
            switch (buttonMapping)
            {
                case NothingMapping:
                    continue;
                case SimulatedKeystrokes when buttonMapping.State:
                    SimulatedKeystrokesReleased(buttonMapping.Keys.Reverse());
                    buttonMapping.State = false;
                    break;
                case SimulatedKeystrokes:
                    SimulatedKeystrokesPressed(buttonMapping.Keys);
                    buttonMapping.State = true;
                    break;
            }
        }
    }

    private void SimulatedKeystrokesReleased(IEnumerable<char> keys)
    {
        foreach (var c in keys)
        {
            _keyboardSimulator.SimulateKeyRelease(c.ToString());
        }
    }

    private void SimulatedKeystrokesPressed(string keys)
    {
        foreach (var c in keys)
        {
            _keyboardSimulator.SimulateKeyPress(c.ToString());
        }
    }

    private void OnMouseReleased(object sender, NewMouseHookEventArgs e)
    {
    }

    private void BuildHotkeys()
    {
        _hotkeys = new Dictionary<NewMouseButton, List<IButtonMapping>>
        {
            { NewMouseButton.Button1, new List<IButtonMapping>() },
            { NewMouseButton.Button2, new List<IButtonMapping>() },
            { NewMouseButton.Button3, new List<IButtonMapping>() },
            { NewMouseButton.Button4, new List<IButtonMapping>() },
            { NewMouseButton.Button5, new List<IButtonMapping>() }
        };
        foreach (var profile in _profilesService.GetProfiles())
        {
            _hotkeys[NewMouseButton.Button1].Add(profile.MouseButton1);
            _hotkeys[NewMouseButton.Button2].Add(profile.MouseButton2);
            _hotkeys[NewMouseButton.Button3].Add(profile.MouseButton3);
            _hotkeys[NewMouseButton.Button4].Add(profile.MouseButton4);
            _hotkeys[NewMouseButton.Button5].Add(profile.MouseButton5);
        }
    }

    public void Run()
    {
        BuildHotkeys();
        SubscribeToEvents();
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
        _mouseListener?.Dispose();
    }
}