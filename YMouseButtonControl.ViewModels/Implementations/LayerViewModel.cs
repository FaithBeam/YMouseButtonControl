using System;
using System.Timers;
using Avalonia.Collections;
using Avalonia.Media;
using ReactiveUI;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class LayerViewModel : ViewModelBase, ILayerViewModel
{
    private readonly ICurrentProfileOperationsMediator _currentProfileOperationsMediator;
    private readonly IMouseListener _mouseListener;
    private IBrush _mouseButton1BackgroundColor = Brushes.White;
    private IBrush _mouseButton2BackgroundColor = Brushes.White;
    private IBrush _mouseButton3BackgroundColor = Brushes.White;
    private IBrush _mouseButton4BackgroundColor = Brushes.White;
    private IBrush _mouseButton5BackgroundColor = Brushes.White;
    private IBrush _wheelUpBackgroundColor = Brushes.White;
    private IBrush _wheelDownBackgroundColor = Brushes.White;
    private IBrush _wheelRightBackgroundColor = Brushes.White;
    private IBrush _wheelLeftBackgroundColor = Brushes.White;
    private readonly Timer _wheelUpTimer = new() { Interval = 200, AutoReset = false };
    private readonly Timer _wheelDownTimer = new() { Interval = 200, AutoReset = false };
    private readonly Timer _wheelLeftTimer = new() { Interval = 200, AutoReset = false };
    private readonly Timer _wheelRightTimer = new() { Interval = 200, AutoReset = false };

    public LayerViewModel(ICurrentProfileOperationsMediator currentProfileOperationsMediator, IMouseListener mouseListener)
    {
        _currentProfileOperationsMediator = currentProfileOperationsMediator;
        _currentProfileOperationsMediator.SelectedProfileChanged += OnSelectedCurrentProfileChanged;
        _mouseListener = mouseListener;
        _mouseListener.OnMousePressedEventHandler += OnMouseClicked;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler += OnWheelScroll;
        _wheelUpTimer.Elapsed += delegate { WheelUpBackgroundColor = Brushes.White; };
        _wheelDownTimer.Elapsed += delegate { WheelDownBackgroundColor = Brushes.White; };
        _wheelLeftTimer.Elapsed += delegate { WheelLeftBackgroundColor = Brushes.White; };
        _wheelRightTimer.Elapsed += delegate { WheelRightBackgroundColor = Brushes.White; };
    }

    public IBrush WheelLeftBackgroundColor
    {
        get => _wheelLeftBackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _wheelLeftBackgroundColor, value);
    }

    public IBrush WheelRightBackgroundColor
    {
        get => _wheelRightBackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _wheelRightBackgroundColor, value);
    }

    public IBrush WheelDownBackgroundColor
    {
        get => _wheelDownBackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _wheelDownBackgroundColor, value);
    }

    public IBrush WheelUpBackgroundColor
    {
        get => _wheelUpBackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _wheelUpBackgroundColor, value);
    }

    public IBrush MouseButton5BackgroundColor
    {
        get => _mouseButton5BackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _mouseButton5BackgroundColor, value);
    }

    public IBrush MouseButton4BackgroundColor
    {
        get => _mouseButton4BackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _mouseButton4BackgroundColor, value);
    }

    public IBrush MouseButton3BackgroundColor
    {
        get => _mouseButton3BackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _mouseButton3BackgroundColor, value);
    }

    public IBrush MouseButton2BackgroundColor
    {
        get => _mouseButton2BackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _mouseButton2BackgroundColor, value);
    }

    public IBrush MouseButton1BackgroundColor
    {
        get => _mouseButton1BackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _mouseButton1BackgroundColor, value);
    }
    
    public AvaloniaList<string> MouseButton1Combo { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseButton1LastIndex => _currentProfileOperationsMediator.CurrentProfile.MouseButton1LastIndex;
    
    public AvaloniaList<string> MouseButton2Combo { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseButton2LastIndex => _currentProfileOperationsMediator.CurrentProfile.MouseButton2LastIndex;
    
    public AvaloniaList<string> MouseButton3Combo { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseButton3LastIndex => _currentProfileOperationsMediator.CurrentProfile.MouseButton3LastIndex;

    public AvaloniaList<string> MouseButton4Combo { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseButton4LastIndex => _currentProfileOperationsMediator.CurrentProfile.MouseButton4LastIndex;
    
    public AvaloniaList<string> MouseButton5Combo { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseButton5LastIndex => _currentProfileOperationsMediator.CurrentProfile.MouseButton5LastIndex;

    public AvaloniaList<string> MouseWheelUp { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseWheelUpLastIndex => _currentProfileOperationsMediator.CurrentProfile.WheelUpLastIndex;
    
    public AvaloniaList<string> MouseWheelDown { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseWheelDownLastIndex => _currentProfileOperationsMediator.CurrentProfile.WheelDownLastIndex;
    
    public AvaloniaList<string> MouseWheelLeft { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseWheelLeftLastIndex => _currentProfileOperationsMediator.CurrentProfile.WheelLeftLastIndex;

    public AvaloniaList<string> MouseWheelRight { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
    
    public int MouseWheelRightLastIndex => _currentProfileOperationsMediator.CurrentProfile.WheelRightLastIndex;
    
    private void OnSelectedCurrentProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        MouseButton1Combo = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.MouseButton1.Index] =
                _currentProfileOperationsMediator.CurrentProfile.MouseButton1.ToString()
        };
        MouseButton2Combo = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.MouseButton2.Index] =
                _currentProfileOperationsMediator.CurrentProfile.MouseButton2.ToString()
        };
        MouseButton3Combo = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.MouseButton3.Index] =
                _currentProfileOperationsMediator.CurrentProfile.MouseButton3.ToString()
        };
        MouseButton4Combo = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.MouseButton4.Index] =
                _currentProfileOperationsMediator.CurrentProfile.MouseButton4.ToString()
        };
        MouseButton5Combo = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.MouseButton5.Index] =
                _currentProfileOperationsMediator.CurrentProfile.MouseButton5.ToString()
        };
        MouseWheelUp = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.WheelUp.Index] =
                _currentProfileOperationsMediator.CurrentProfile.WheelUp.ToString()
        };
        MouseWheelDown = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.WheelDown.Index] =
                _currentProfileOperationsMediator.CurrentProfile.WheelDown.ToString()
        };
        MouseWheelLeft = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.WheelLeft.Index] =
                _currentProfileOperationsMediator.CurrentProfile.WheelLeft.ToString()
        };
        MouseWheelRight = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_currentProfileOperationsMediator.CurrentProfile.WheelRight.Index] =
                _currentProfileOperationsMediator.CurrentProfile.WheelRight.ToString()
        };
        this.RaisePropertyChanged(nameof(MouseButton1Combo));
        this.RaisePropertyChanged(nameof(MouseButton1LastIndex));
        this.RaisePropertyChanged(nameof(MouseButton2Combo));
        this.RaisePropertyChanged(nameof(MouseButton2LastIndex));
        this.RaisePropertyChanged(nameof(MouseButton3Combo));
        this.RaisePropertyChanged(nameof(MouseButton3LastIndex));
        this.RaisePropertyChanged(nameof(MouseButton4Combo));
        this.RaisePropertyChanged(nameof(MouseButton4LastIndex));
        this.RaisePropertyChanged(nameof(MouseButton5Combo));
        this.RaisePropertyChanged(nameof(MouseButton5LastIndex));
        this.RaisePropertyChanged(nameof(MouseWheelUp));
        this.RaisePropertyChanged(nameof(MouseWheelUpLastIndex));
        this.RaisePropertyChanged(nameof(MouseWheelDown));
        this.RaisePropertyChanged(nameof(MouseWheelDownLastIndex));
        this.RaisePropertyChanged(nameof(MouseWheelLeft));
        this.RaisePropertyChanged(nameof(MouseWheelLeftLastIndex));
        this.RaisePropertyChanged(nameof(MouseWheelRight));
        this.RaisePropertyChanged(nameof(MouseWheelRightLastIndex));
    }

    private void OnWheelScroll(object sender, NewMouseWheelEventArgs e)
    {
        switch (e.Direction)
        {
            case WheelScrollDirection.VerticalUp:
                WheelUpBackgroundColor = Brushes.Yellow;
                if (!_wheelUpTimer.Enabled)
                {
                    _wheelUpTimer.Start();
                }
                break;
            case WheelScrollDirection.VerticalDown:
                WheelDownBackgroundColor = Brushes.Yellow;
                if (!_wheelDownTimer.Enabled)
                {
                    _wheelDownTimer.Start();
                }
                break;
            case WheelScrollDirection.HorizontalLeft:
                WheelLeftBackgroundColor = Brushes.Yellow;
                if (!_wheelLeftTimer.Enabled)
                {
                    _wheelLeftTimer.Start();
                }
                break;    
            case WheelScrollDirection.HorizontalRight:
                WheelRightBackgroundColor = Brushes.Yellow;
                if (!_wheelRightTimer.Enabled)
                {
                    _wheelRightTimer.Start();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseReleased(object sender, NewMouseHookEventArgs e)
    {
        switch (e.Button)
        {
            case NewMouseButton.Button1:
                MouseButton1BackgroundColor = Brushes.White;
                break;
            case NewMouseButton.Button2:
                MouseButton2BackgroundColor = Brushes.White;
                break;
            case NewMouseButton.Button3:
                MouseButton3BackgroundColor = Brushes.White;
                break;
            case NewMouseButton.Button4:
                MouseButton4BackgroundColor = Brushes.White;
                break;
            case NewMouseButton.Button5:
                MouseButton5BackgroundColor = Brushes.White;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseClicked(object sender, NewMouseHookEventArgs e)
    {
        switch (e.Button)
        {
            case NewMouseButton.Button1:
                MouseButton1BackgroundColor = Brushes.Yellow;
                break;
            case NewMouseButton.Button2:
                MouseButton2BackgroundColor = Brushes.Yellow;
                break;
            case NewMouseButton.Button3:
                MouseButton3BackgroundColor = Brushes.Yellow;
                break;
            case NewMouseButton.Button4:
                MouseButton4BackgroundColor = Brushes.Yellow;
                break;
            case NewMouseButton.Button5:
                MouseButton5BackgroundColor = Brushes.Yellow;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}