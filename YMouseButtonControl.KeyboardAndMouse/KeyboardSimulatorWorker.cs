using System;
using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse;

public class KeyboardSimulatorWorker : IDisposable
{
    private readonly IMouseListener _mouseListener;
    private readonly IProfilesService _profilesService;
    private readonly IKeyboardSimulator _keyboardSimulator;
    private readonly IProcessMonitorService _processMonitorService;
    private Dictionary<NewMouseButton, List<IButtonMapping>> _hotkeys;

    public KeyboardSimulatorWorker(IProfilesService profilesService, IMouseListener mouseListener, IKeyboardSimulator keyboardSimulator, IProcessMonitorService processMonitorService)
    {
        _profilesService = profilesService;
        _mouseListener = mouseListener;
        _keyboardSimulator = keyboardSimulator;
        _processMonitorService = processMonitorService;
    }

    private void SubscribeToEvents()
    {
        _mouseListener.OnMousePressedEventHandler += OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        _processMonitorService.OnProcessChangedEventHandler += OnProcessChanged;
    }

    private void OnProcessChanged(object sender, ProcessChangedEventArgs e)
    {
        BuildHotkeys();
    }


    private void UnsubscribeFromEvents()
    {
        _mouseListener.OnMousePressedEventHandler -= OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler -= OnMouseReleased;
        _processMonitorService.OnProcessChangedEventHandler -= OnProcessChanged;
    }

    private void OnMousePressed(object sender, NewMouseHookEventArgs e)
    {
        foreach (var buttonMapping in _hotkeys[e.Button])
        {
            switch (buttonMapping)
            {
                case NothingMapping:
                    continue;
                case SimulatedKeystrokes:
                    _keyboardSimulator.SimulatedKeystrokes(buttonMapping, true);
                    break;
            }
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
            if (!_processMonitorService.ProcessRunning(profile.Process))
            {
                continue;
            }
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
        _processMonitorService?.Dispose();
    }
}