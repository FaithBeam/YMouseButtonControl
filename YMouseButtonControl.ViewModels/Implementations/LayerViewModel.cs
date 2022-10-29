using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Collections;
using Avalonia.Media;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Factories;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Models;
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
    private IButtonMapping _selectedMouseButton1Mapping;
    private IButtonMapping _selectedMouseButton2Mapping;
    private IButtonMapping _selectedMouseButton3Mapping;
    private IButtonMapping _selectedMouseButton4Mapping;
    private IButtonMapping _selectedMouseButton5Mapping;
    private IButtonMapping _selectedMouseWheelUpMapping;
    private IButtonMapping _selectedMouseWheelDownMapping;
    private IButtonMapping _selectedMouseWheelLeftMapping;
    private IButtonMapping _selectedMouseWheelRightMapping;
    private int _currentMouseButton1ComboIndex;
    private int _currentMouseButton2ComboIndex;
    private int _currentMouseButton3ComboIndex;
    private int _currentMouseButton4ComboIndex;
    private int _currentMouseButton5ComboIndex;
    private int _currentMouseWheelUpComboIndex;
    private int _currentMouseWheelDownComboIndex;
    private int _currentMouseWheelLeftComboIndex;
    private int _currentMouseWheelRightComboIndex;
     
    public Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel> ShowSimulatedKeystrokesPickerInteraction
    {
        get;
    }

    public LayerViewModel(ICurrentProfileOperationsMediator currentProfileOperationsMediator,
        IMouseListener mouseListener)
    {
        _currentProfileOperationsMediator = currentProfileOperationsMediator;
        _currentProfileOperationsMediator.CurrentProfileChanged += OnSelectedCurrentProfileChanged;
        _mouseListener = mouseListener;
        _mouseListener.OnMousePressedEventHandler += OnMouseClicked;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler += OnWheelScroll;
        _wheelUpTimer.Elapsed += delegate { WheelUpBackgroundColor = Brushes.White; };
        _wheelDownTimer.Elapsed += delegate { WheelDownBackgroundColor = Brushes.White; };
        _wheelLeftTimer.Elapsed += delegate { WheelLeftBackgroundColor = Brushes.White; };
        _wheelRightTimer.Elapsed += delegate { WheelRightBackgroundColor = Brushes.White; };
        ShowSimulatedKeystrokesPickerInteraction =
            new Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel>();
        
        this
            .WhenAnyValue(x => x.SelectedMouseButton1Mapping)
            .Subscribe(async x => await GetMappingAsync(x, "mb1"));
        this
            .WhenAnyValue(x => x.SelectedMouseButton2Mapping)
            .Subscribe(async x => await GetMappingAsync(x, "mb2"));
        this
            .WhenAnyValue(x => x.SelectedMouseButton3Mapping)
            .Subscribe(async x => await GetMappingAsync(x, "mb3"));
        this
            .WhenAnyValue(x => x.SelectedMouseButton4Mapping)
            .Subscribe(async x => await GetMappingAsync(x, "mb4"));
        this
            .WhenAnyValue(x => x.SelectedMouseButton5Mapping)
            .Subscribe(async x => await GetMappingAsync(x, "mb5"));
        this
            .WhenAnyValue(x => x.SelectedMouseWheelUpMapping)
            .Subscribe(async x => await GetMappingAsync(x, "mwu"));
        this
            .WhenAnyValue(x => x.SelectedMouseWheelDownMapping)
            .Subscribe(async x => await GetMappingAsync(x, "mwd"));
        this
            .WhenAnyValue(x => x.SelectedMouseWheelLeftMapping)
            .Subscribe(async x => await GetMappingAsync(x, "mwl"));
        this
            .WhenAnyValue(x => x.SelectedMouseWheelRightMapping)
            .Subscribe(async x => await GetMappingAsync(x, "mwr"));
        
        InitialLoadSelectedMappings();
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

    public AvaloniaList<IButtonMapping> MouseButton1Combo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseButton2Combo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseButton3Combo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseButton4Combo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseButton5Combo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseWheelUpCombo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseWheelDownCombo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseWheelLeftCombo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseWheelRightCombo { get; set; } = new(ButtonMappingFactory.GetButtonMappings());

    public IButtonMapping SelectedMouseButton1Mapping
    {
        get => _selectedMouseButton1Mapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseButton1Mapping, value);
    }

    public IButtonMapping SelectedMouseButton2Mapping
    {
        get => _selectedMouseButton2Mapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseButton2Mapping, value);
    }

    public IButtonMapping SelectedMouseButton3Mapping
    {
        get => _selectedMouseButton3Mapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseButton3Mapping, value);
    }

    public IButtonMapping SelectedMouseButton4Mapping
    {
        get => _selectedMouseButton4Mapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseButton4Mapping, value);
    }

    public IButtonMapping SelectedMouseButton5Mapping
    {
        get => _selectedMouseButton5Mapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseButton5Mapping, value);
    }

    public IButtonMapping SelectedMouseWheelUpMapping
    {
        get => _selectedMouseWheelUpMapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseWheelUpMapping, value);
    }

    public IButtonMapping SelectedMouseWheelDownMapping
    {
        get => _selectedMouseWheelDownMapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseWheelDownMapping, value);
    }

    public IButtonMapping SelectedMouseWheelLeftMapping
    {
        get => _selectedMouseWheelLeftMapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseWheelLeftMapping, value);
    }

    public IButtonMapping SelectedMouseWheelRightMapping
    {
        get => _selectedMouseWheelRightMapping;
        set => this.RaiseAndSetIfChanged(ref _selectedMouseWheelRightMapping, value);
    }

    public int CurrentMouseButton1ComboIndex
    {
        get => _currentMouseButton1ComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseButton1ComboIndex, value);
    }
    
    public int CurrentMouseButton2ComboIndex
    {
        get => _currentMouseButton2ComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseButton2ComboIndex, value);
    }
    
    public int CurrentMouseButton3ComboIndex
    {
        get => _currentMouseButton3ComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseButton3ComboIndex, value);
    }
    
    public int CurrentMouseButton4ComboIndex
    {
        get => _currentMouseButton4ComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseButton4ComboIndex, value);
    }
    
    public int CurrentMouseButton5ComboIndex
    {
        get => _currentMouseButton5ComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseButton5ComboIndex, value);
    }
    
    public int CurrentMouseWheelUpComboIndex
    {
        get => _currentMouseWheelUpComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseWheelUpComboIndex, value);
    }
    
    public int CurrentMouseWheelDownComboIndex
    {
        get => _currentMouseWheelDownComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseWheelDownComboIndex, value);
    }
    
    public int CurrentMouseWheelLeftComboIndex
    {
        get => _currentMouseWheelLeftComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseWheelLeftComboIndex, value);
    }
    
    public int CurrentMouseWheelRightComboIndex
    {
        get => _currentMouseWheelRightComboIndex;
        set => this.RaiseAndSetIfChanged(ref _currentMouseWheelRightComboIndex, value);
    }
    
    private void InitialLoadSelectedMappings()
    {
        SelectedMouseButton1Mapping = _currentProfileOperationsMediator.CurrentProfile.MouseButton1;
        SelectedMouseButton2Mapping = _currentProfileOperationsMediator.CurrentProfile.MouseButton2;
        SelectedMouseButton3Mapping = _currentProfileOperationsMediator.CurrentProfile.MouseButton3;
        SelectedMouseButton4Mapping = _currentProfileOperationsMediator.CurrentProfile.MouseButton4;
        SelectedMouseButton5Mapping = _currentProfileOperationsMediator.CurrentProfile.MouseButton5;
        SelectedMouseWheelUpMapping = _currentProfileOperationsMediator.CurrentProfile.MouseWheelUp;
        SelectedMouseWheelDownMapping = _currentProfileOperationsMediator.CurrentProfile.MouseWheelDown;
        SelectedMouseWheelLeftMapping = _currentProfileOperationsMediator.CurrentProfile.MouseWheelLeft;
        SelectedMouseWheelRightMapping = _currentProfileOperationsMediator.CurrentProfile.MouseWheelRight;
    }

    private void OnSelectedCurrentProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        MouseButton1Combo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseButton1.Index] = e.NewProfile.MouseButton1
        };
        MouseButton2Combo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseButton2.Index] = e.NewProfile.MouseButton2
        };
        MouseButton3Combo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseButton3.Index] = e.NewProfile.MouseButton3
        };
        MouseButton4Combo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseButton4.Index] = e.NewProfile.MouseButton4
        };
        MouseButton5Combo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseButton5.Index] = e.NewProfile.MouseButton5
        };
        MouseWheelUpCombo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseWheelUp.Index] = e.NewProfile.MouseWheelUp
        };
        MouseWheelDownCombo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseWheelDown.Index] = e.NewProfile.MouseWheelDown
        };
        MouseWheelLeftCombo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseWheelLeft.Index] = e.NewProfile.MouseWheelLeft
        };
        MouseWheelRightCombo = new AvaloniaList<IButtonMapping>(ButtonMappingFactory.GetButtonMappings())
        {
            [e.NewProfile.MouseWheelRight.Index] = e.NewProfile.MouseWheelRight
        };
        
        this.RaisePropertyChanged(nameof(MouseButton1Combo));
        this.RaisePropertyChanged(nameof(MouseButton2Combo));
        this.RaisePropertyChanged(nameof(MouseButton3Combo));
        this.RaisePropertyChanged(nameof(MouseButton4Combo));
        this.RaisePropertyChanged(nameof(MouseButton5Combo));
        this.RaisePropertyChanged(nameof(MouseWheelUpCombo));
        this.RaisePropertyChanged(nameof(MouseWheelDownCombo));
        this.RaisePropertyChanged(nameof(MouseWheelLeftCombo));
        this.RaisePropertyChanged(nameof(MouseWheelRightCombo));
        
        CurrentMouseButton1ComboIndex = e.NewProfile.MouseButton1LastIndex;
        CurrentMouseButton2ComboIndex = e.NewProfile.MouseButton2LastIndex;
        CurrentMouseButton3ComboIndex = e.NewProfile.MouseButton3LastIndex;
        CurrentMouseButton4ComboIndex = e.NewProfile.MouseButton4LastIndex;
        CurrentMouseButton5ComboIndex = e.NewProfile.MouseButton5LastIndex;
        CurrentMouseWheelUpComboIndex = e.NewProfile.MouseWheelUpLastIndex;
        CurrentMouseWheelDownComboIndex = e.NewProfile.MouseWheelDownLastIndex;
        CurrentMouseWheelLeftComboIndex = e.NewProfile.MouseWheelLeftLastIndex;
        CurrentMouseWheelRightComboIndex = e.NewProfile.MouseWheelRightLastIndex;
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

    private async Task GetMappingAsync(IButtonMapping mapping, string mouseAction)
    {
        if (mapping is null || !mapping.CanRaiseDialog)
        {
            return;
        }
        
        if (mapping is SimulatedKeystrokes)
        {
            var result = await ShowSimulatedKeystrokesDialog();
            if (result is not null)
            {
                switch (mouseAction)
                {
                    case "mb1":
                        _currentProfileOperationsMediator.UpdateMouseButton1(result);
                        break;
                    case "mb2":
                        _currentProfileOperationsMediator.UpdateMouseButton2(result);
                        break;
                    case "mb3":
                        _currentProfileOperationsMediator.UpdateMouseButton3(result);
                        break;
                    case "mb4":
                        _currentProfileOperationsMediator.UpdateMouseButton4(result);
                        break;
                    case "mb5":
                        _currentProfileOperationsMediator.UpdateMouseButton5(result);
                        break;
                    case "mwu":
                        _currentProfileOperationsMediator.UpdateMouseWheelUp(result);
                        break;
                    case "mwd":
                        _currentProfileOperationsMediator.UpdateMouseWheelDown(result);
                        break;
                    case "mwl":
                        _currentProfileOperationsMediator.UpdateMouseWheelLeft(result);
                        break;
                    case "mwr":
                        _currentProfileOperationsMediator.UpdateMouseWheelRight(result);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    private async Task<SimulatedKeystrokes> ShowSimulatedKeystrokesDialog()
    {
        var result = await ShowSimulatedKeystrokesPickerInteraction.Handle(new SimulatedKeystrokesDialogViewModel());
        if (result is null)
        {
            return null;
        }

        return new SimulatedKeystrokes()
        {
            Keys = result.CustomKeys,
            SimulatedKeystrokesType = result.SimulatedKeystrokesType,
        };
    }
}