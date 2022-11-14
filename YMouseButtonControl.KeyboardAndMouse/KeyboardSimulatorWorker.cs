using System;
using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse;

public class KeyboardSimulatorWorker : IDisposable
{
    private readonly IMouseListener _mouseListener;
    private readonly IProfilesService _profilesService;
    private readonly IKeyboardSimulator _keyboardSimulator;
    private readonly IProcessMonitorService _processMonitorService;
    private readonly ICurrentProfileOperationsMediator _currentProfileOperationsMediator;
    private Dictionary<NewMouseButton, List<IButtonMapping>> _hotkeys;

    public KeyboardSimulatorWorker(IProfilesService profilesService, IMouseListener mouseListener,
        IKeyboardSimulator keyboardSimulator, IProcessMonitorService processMonitorService,
        ICurrentProfileOperationsMediator currentProfileOperationsMediator)
    {
        _profilesService = profilesService;
        _mouseListener = mouseListener;
        _keyboardSimulator = keyboardSimulator;
        _processMonitorService = processMonitorService;
        _currentProfileOperationsMediator = currentProfileOperationsMediator;
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

    private void SubscribeToEvents()
    {
        _mouseListener.OnMousePressedEventHandler += OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler += OnMouseWheel;
        _processMonitorService.OnProcessChangedEventHandler += OnProcessChanged;
        _profilesService.OnProfilesChangedEventHandler += OnProfilesChanged;
        _currentProfileOperationsMediator.CurrentProfileChanged += OnCurrentProfileChanged;
        _currentProfileOperationsMediator.CurrentProfileButtonMappingEdited += OnCurrentProfileEdited;
    }

    private void OnCurrentProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        BuildHotkeys();
    }

    private void OnCurrentProfileEdited(object sender, SelectedProfileEditedEventArgs e)
    {
        BuildHotkeys();
    }

    private void OnProfilesChanged(object sender, ProfilesChangedEventArgs e)
    {
        BuildHotkeys();
    }

    private void OnProcessChanged(object sender, ProcessChangedEventArgs e)
    {
        BuildHotkeys();
    }

    private void UnsubscribeFromEvents()
    {
        _mouseListener.OnMousePressedEventHandler -= OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler -= OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler -= OnMouseWheel;
        _processMonitorService.OnProcessChangedEventHandler -= OnProcessChanged;
        _profilesService.OnProfilesChangedEventHandler -= OnProfilesChanged;
    }

    private void OnMousePressed(object sender, NewMouseHookEventArgs e)
    {
        foreach (var buttonMapping in _hotkeys[e.Button])
        {
            switch (buttonMapping)
            {
                case SimulatedKeystrokes:
                    _keyboardSimulator.SimulatedKeystrokes(buttonMapping, true);
                    break;
            }
        }
    }

    private void OnMouseReleased(object sender, NewMouseHookEventArgs e)
    {
    }
    
    private void OnMouseWheel(object sender, NewMouseWheelEventArgs e)
    {
    }

    private void BuildHotkeys()
    {
        _hotkeys = new Dictionary<NewMouseButton, List<IButtonMapping>>
        {
            {NewMouseButton.Button1, new List<IButtonMapping>()},
            {NewMouseButton.Button2, new List<IButtonMapping>()},
            {NewMouseButton.Button3, new List<IButtonMapping>()},
            {NewMouseButton.Button4, new List<IButtonMapping>()},
            {NewMouseButton.Button5, new List<IButtonMapping>()}
        };
        foreach (var profile in _profilesService.GetProfiles())
        {
            if (profile.Process != "*" && !_processMonitorService.ProcessRunning(profile.Process))
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
}