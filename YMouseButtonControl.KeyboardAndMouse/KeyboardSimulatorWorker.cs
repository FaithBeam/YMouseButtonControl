using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
    private readonly ICurrentWindowService _currentWindowService;
    private Dictionary<MouseButton, List<IButtonMapping>> _hotkeys;
    private string _currentWindow = string.Empty;
    private readonly object _lockObj = new();

    public KeyboardSimulatorWorker(IProfilesService profilesService, IMouseListener mouseListener,
        IKeyboardSimulator keyboardSimulator, IProcessMonitorService processMonitorService,
        ICurrentWindowService currentWindowService)
    {
        _profilesService = profilesService;
        _mouseListener = mouseListener;
        _keyboardSimulator = keyboardSimulator;
        _processMonitorService = processMonitorService;
        _currentWindowService = currentWindowService;
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
        _currentWindowService.Dispose();
    }

    private void SubscribeToEvents()
    {
        _mouseListener.OnMousePressedEventHandler += OnMousePressed;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler += OnMouseWheel;
        _processMonitorService.OnProcessCreatedEventHandler += OnProcessChanged;
        _processMonitorService.OnProcessDeletedEventHandler += OnProcessChanged;
        _currentWindowService.OnActiveWindowChangedEventHandler += OnActiveWindowChanged;
    }

    private void OnActiveWindowChanged(object sender, ActiveWindowChangedEventArgs e)
    {
        lock (_lockObj)
        {
            _currentWindow = e.ActiveWindow;
            BuildHotkeys();
        }
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
        _processMonitorService.OnProcessCreatedEventHandler -= OnProcessChanged;
        _processMonitorService.OnProcessDeletedEventHandler -= OnProcessChanged;
        _currentWindowService.OnActiveWindowChangedEventHandler -= OnActiveWindowChanged;
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
            MouseButton.MouseButton1 => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseButton1).ToList(),
            MouseButton.MouseButton2 => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseButton2).ToList(),
            MouseButton.MouseButton3 => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseButton3).ToList(),
            MouseButton.MouseButton4 => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseButton4).ToList(),
            MouseButton.MouseButton5 => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseButton5).ToList(),
            MouseButton.MouseWheelUp => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseWheelUp).ToList(),
            MouseButton.MouseWheelDown => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseWheelDown).ToList(),
            MouseButton.MouseWheelLeft => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseWheelLeft).ToList(),
            MouseButton.MouseWheelRight => _profilesService.Profiles
                .Where(p => (p.Process == "*" || _currentWindow.Contains(p.Process)) && p.Checked)
                .Select(x => x.MouseWheelRight).ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }

    private void BuildHotkeys()
    {
        _hotkeys = new Dictionary<MouseButton, List<IButtonMapping>>
        {
            { MouseButton.MouseButton1, new List<IButtonMapping>() },
            { MouseButton.MouseButton2, new List<IButtonMapping>() },
            { MouseButton.MouseButton3, new List<IButtonMapping>() },
            { MouseButton.MouseButton4, new List<IButtonMapping>() },
            { MouseButton.MouseButton5, new List<IButtonMapping>() },
            { MouseButton.MouseWheelUp, new List<IButtonMapping>() },
            { MouseButton.MouseWheelDown, new List<IButtonMapping>() },
            { MouseButton.MouseWheelLeft, new List<IButtonMapping>() },
            { MouseButton.MouseWheelRight, new List<IButtonMapping>() }
        };

        foreach (var p in _profilesService.Profiles.Where(p => p.Process == "*" || _currentWindow.Contains(p.Process)))
        {
            _hotkeys[MouseButton.MouseButton1].Add(p.MouseButton1);
            _hotkeys[MouseButton.MouseButton2].Add(p.MouseButton2);
            _hotkeys[MouseButton.MouseButton3].Add(p.MouseButton3);
            _hotkeys[MouseButton.MouseButton4].Add(p.MouseButton4);
            _hotkeys[MouseButton.MouseButton5].Add(p.MouseButton5);
            _hotkeys[MouseButton.MouseWheelUp].Add(p.MouseWheelUp);
            _hotkeys[MouseButton.MouseWheelDown].Add(p.MouseWheelDown);
            _hotkeys[MouseButton.MouseWheelLeft].Add(p.MouseWheelLeft);
            _hotkeys[MouseButton.MouseWheelRight].Add(p.MouseWheelRight);
        }
    }
}