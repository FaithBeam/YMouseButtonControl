using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using MouseButton = YMouseButtonControl.DataAccess.Models.Enums.MouseButton;

namespace YMouseButtonControl.KeyboardAndMouse;

public class KeyboardSimulatorWorker : IDisposable
{
    private readonly IMouseListener _mouseListener;
    private readonly IProfilesService _profilesService;
    private readonly IKeyboardSimulator _keyboardSimulator;
    private readonly IProcessMonitorService _processMonitorService;
    private Dictionary<MouseButton, List<IButtonMapping>> _hotkeys;

    public KeyboardSimulatorWorker(IProfilesService profilesService, IMouseListener mouseListener,
        IKeyboardSimulator keyboardSimulator, IProcessMonitorService processMonitorService)
    {
        _profilesService = profilesService;
        _mouseListener = mouseListener;
        _keyboardSimulator = keyboardSimulator;
        _processMonitorService = processMonitorService;
        this
            .WhenAnyValue(x => x._profilesService.CurrentProfile)
            .Subscribe(OnCurrentProfileChanged);
        this
            .WhenAnyValue(x => x._profilesService.CurrentProfile.MouseButton1)
            .Subscribe(x => BuildHotkeys(x, MouseButton.MouseButton1));
        this
            .WhenAnyValue(x => x._profilesService.CurrentProfile.MouseButton2)
            .Subscribe(x => BuildHotkeys(x, MouseButton.MouseButton2));
        this
            .WhenAnyValue(x => x._profilesService.CurrentProfile.MouseButton3)
            .Subscribe(x => BuildHotkeys(x, MouseButton.MouseButton3));
        this
            .WhenAnyValue(x => x._profilesService.CurrentProfile.MouseButton4)
            .Subscribe(x => BuildHotkeys(x, MouseButton.MouseButton4));
        this
            .WhenAnyValue(x => x._profilesService.CurrentProfile.MouseButton5)
            .Subscribe(x => BuildHotkeys(x, MouseButton.MouseButton5));
        this
            .WhenAnyValue(x => x._profilesService.Profiles)
            .Subscribe(OnProfilesChanged);
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
    }

    private void OnCurrentProfileChanged(Profile newProfile)
    {
        BuildHotkeys();
    }

    private void OnProfilesChanged(AvaloniaList<Profile> profiles)
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

    private void BuildHotkeys(IButtonMapping mapping, MouseButton button)
    {
        _hotkeys[button] = button switch
        {
            MouseButton.MouseButton1 => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseButton1).ToList(),
            MouseButton.MouseButton2 => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseButton2).ToList(),
            MouseButton.MouseButton3 => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseButton3).ToList(),
            MouseButton.MouseButton4 => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseButton4).ToList(),
            MouseButton.MouseButton5 => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseButton5).ToList(),
            MouseButton.MouseWheelUp => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseWheelUp).ToList(),
            MouseButton.MouseWheelDown => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseWheelDown).ToList(),
            MouseButton.MouseWheelLeft => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseWheelLeft).ToList(),
            MouseButton.MouseWheelRight => _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)).Select(x => x.MouseWheelRight).ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }
    
    private void BuildHotkeys()
    {
        _hotkeys = new Dictionary<MouseButton, List<IButtonMapping>>
        {
            {MouseButton.MouseButton1, new List<IButtonMapping>()},
            {MouseButton.MouseButton2, new List<IButtonMapping>()},
            {MouseButton.MouseButton3, new List<IButtonMapping>()},
            {MouseButton.MouseButton4, new List<IButtonMapping>()},
            {MouseButton.MouseButton5, new List<IButtonMapping>()},
            {MouseButton.MouseWheelUp, new List<IButtonMapping>()},
            {MouseButton.MouseWheelDown, new List<IButtonMapping>()},
            {MouseButton.MouseWheelLeft, new List<IButtonMapping>()},
            {MouseButton.MouseWheelRight, new List<IButtonMapping>()}
        };
        foreach (var profile in _profilesService.Profiles.Where(profile => profile.Process == "*" || _processMonitorService.ProcessRunning(profile.Process)))
        {
            _hotkeys[MouseButton.MouseButton1].Add(profile.MouseButton1);
            _hotkeys[MouseButton.MouseButton2].Add(profile.MouseButton2);
            _hotkeys[MouseButton.MouseButton3].Add(profile.MouseButton3);
            _hotkeys[MouseButton.MouseButton4].Add(profile.MouseButton4);
            _hotkeys[MouseButton.MouseButton5].Add(profile.MouseButton5);
            _hotkeys[MouseButton.MouseWheelUp].Add(profile.MouseWheelUp);
            _hotkeys[MouseButton.MouseWheelDown].Add(profile.MouseWheelDown);
            _hotkeys[MouseButton.MouseWheelLeft].Add(profile.MouseWheelLeft);
            _hotkeys[MouseButton.MouseWheelRight].Add(profile.MouseWheelRight);
        }
    }
}