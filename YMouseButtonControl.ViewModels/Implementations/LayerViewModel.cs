using System;
using System.Reactive;
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
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseButton1Mapping;
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseButton2Mapping;
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseButton3Mapping;
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseButton4Mapping;
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseButton5Mapping;
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseWheelUpMapping;
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseWheelDownMapping;
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseWheelLeftMapping;
    private readonly ObservableAsPropertyHelper<IButtonMapping> _selectedMouseWheelRightMapping;
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

        // When the combobox index changes, get the mapping
        _selectedMouseButton1Mapping = this
            .WhenAnyValue(x => x.CurrentMouseButton1ComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseButton1Combo[x])
            .ToProperty(this, x => x.SelectedMouseButton1Mapping);
        _selectedMouseButton2Mapping = this
            .WhenAnyValue(x => x.CurrentMouseButton2ComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseButton2Combo[x])
            .ToProperty(this, x => x.SelectedMouseButton2Mapping);
        _selectedMouseButton3Mapping = this
            .WhenAnyValue(x => x.CurrentMouseButton3ComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseButton3Combo[x])
            .ToProperty(this, x => x.SelectedMouseButton3Mapping);
        _selectedMouseButton4Mapping = this
            .WhenAnyValue(x => x.CurrentMouseButton4ComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseButton4Combo[x])
            .ToProperty(this, x => x.SelectedMouseButton4Mapping);
        _selectedMouseButton5Mapping = this
            .WhenAnyValue(x => x.CurrentMouseButton5ComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseButton5Combo[x])
            .ToProperty(this, x => x.SelectedMouseButton5Mapping);
        _selectedMouseWheelUpMapping = this
            .WhenAnyValue(x => x.CurrentMouseWheelUpComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseWheelUpCombo[x])
            .ToProperty(this, x => x.SelectedMouseWheelUpMapping);
        _selectedMouseWheelDownMapping = this
            .WhenAnyValue(x => x.CurrentMouseWheelDownComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseWheelDownCombo[x])
            .ToProperty(this, x => x.SelectedMouseWheelDownMapping);
        _selectedMouseWheelLeftMapping = this
            .WhenAnyValue(x => x.CurrentMouseWheelLeftComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseWheelLeftCombo[x])
            .ToProperty(this, x => x.SelectedMouseWheelLeftMapping);
        _selectedMouseWheelRightMapping = this
            .WhenAnyValue(x => x.CurrentMouseWheelRightComboIndex)
            .Where(x => x >= 0)
            .Select(x => MouseWheelRightCombo[x])
            .ToProperty(this, x => x.SelectedMouseWheelRightMapping);

        // when the mapping changes, try to raise its key type window
        this
            .WhenAnyValue(x => x.SelectedMouseButton1Mapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mb1"));
        this
            .WhenAnyValue(x => x.SelectedMouseButton2Mapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mb2"));
        this
            .WhenAnyValue(x => x.SelectedMouseButton3Mapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mb3"));
        this
            .WhenAnyValue(x => x.SelectedMouseButton4Mapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mb4"));
        this
            .WhenAnyValue(x => x.SelectedMouseButton5Mapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mb5"));
        this
            .WhenAnyValue(x => x.SelectedMouseWheelUpMapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mwu"));
        this
            .WhenAnyValue(x => x.SelectedMouseWheelDownMapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mwd"));
        this
            .WhenAnyValue(x => x.SelectedMouseWheelLeftMapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mwl"));
        this
            .WhenAnyValue(x => x.SelectedMouseWheelRightMapping)
            .Where(x => string.IsNullOrWhiteSpace(x.Keys))
            .Subscribe(async x => await GetMappingAsync(x, "mwr"));

        // Bool to represent whether the gear settings button is enabled/disabled
        var mb1ComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseButton1Mapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mb2ComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseButton2Mapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mb3ComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseButton3Mapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mb4ComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseButton4Mapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mb5ComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseButton5Mapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mwuComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseWheelUpMapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mwdComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseWheelDownMapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mwlComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseWheelLeftMapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mwrComboSettingCanExecute = this
            .WhenAnyValue(x => x.SelectedMouseWheelRightMapping,
                (mapping) => 
                    mapping is not DisabledMapping 
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        
        // When the gear button is clicked, try to open the key dialog
        MouseButton1ComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseButton1Mapping, "mb1");
        }, mb1ComboSettingCanExecute);
        MouseButton2ComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseButton2Mapping, "mb2");
        }, mb2ComboSettingCanExecute);
        MouseButton3ComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseButton3Mapping, "mb3");
        }, mb3ComboSettingCanExecute);
        MouseButton4ComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseButton4Mapping, "mb4");
        }, mb4ComboSettingCanExecute);
        MouseButton5ComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseButton5Mapping, "mb5");
        }, mb5ComboSettingCanExecute);
        MouseWheelUpComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseWheelUpMapping, "mwu");
        }, mwuComboSettingCanExecute);
        MouseWheelDownComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseWheelDownMapping, "mwd");
        }, mwdComboSettingCanExecute);
        MouseWheelLeftComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseWheelLeftMapping, "mwl");
        }, mwlComboSettingCanExecute);
        MouseWheelRightComboSettingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await GetMappingAsync(SelectedMouseWheelRightMapping, "mwr");
        }, mwrComboSettingCanExecute);
    }

    public IButtonMapping SelectedMouseButton1Mapping => _selectedMouseButton1Mapping.Value;
    public IButtonMapping SelectedMouseButton2Mapping => _selectedMouseButton2Mapping.Value;
    public IButtonMapping SelectedMouseButton3Mapping => _selectedMouseButton3Mapping.Value;
    public IButtonMapping SelectedMouseButton4Mapping => _selectedMouseButton4Mapping.Value;
    public IButtonMapping SelectedMouseButton5Mapping => _selectedMouseButton5Mapping.Value;
    public IButtonMapping SelectedMouseWheelUpMapping => _selectedMouseWheelUpMapping.Value;
    public IButtonMapping SelectedMouseWheelDownMapping => _selectedMouseWheelDownMapping.Value;
    public IButtonMapping SelectedMouseWheelLeftMapping => _selectedMouseWheelLeftMapping.Value;
    public IButtonMapping SelectedMouseWheelRightMapping => _selectedMouseWheelRightMapping.Value;
    
    public ReactiveCommand<Unit, Unit> MouseButton1ComboSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> MouseButton2ComboSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> MouseButton3ComboSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> MouseButton4ComboSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> MouseButton5ComboSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> MouseWheelUpComboSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> MouseWheelDownComboSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> MouseWheelLeftComboSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> MouseWheelRightComboSettingCommand { get; }
    
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
        switch (mapping)
        {
            case null:
                return;
            case SimulatedKeystrokes:
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
                    }
                }
                break;
            }
            default:
                switch (mouseAction)
                {
                    case "mb1":
                        _currentProfileOperationsMediator.UpdateMouseButton1(mapping);
                        break;
                    case "mb2":
                        _currentProfileOperationsMediator.UpdateMouseButton2(mapping);
                        break;
                    case "mb3":
                        _currentProfileOperationsMediator.UpdateMouseButton3(mapping);
                        break;
                    case "mb4":
                        _currentProfileOperationsMediator.UpdateMouseButton4(mapping);
                        break;
                    case "mb5":
                        _currentProfileOperationsMediator.UpdateMouseButton5(mapping);
                        break;
                    case "mwu":
                        _currentProfileOperationsMediator.UpdateMouseWheelUp(mapping);
                        break;
                    case "mwd":
                        _currentProfileOperationsMediator.UpdateMouseWheelDown(mapping);
                        break;
                    case "mwl":
                        _currentProfileOperationsMediator.UpdateMouseWheelLeft(mapping);
                        break;
                    case "mwr":
                        _currentProfileOperationsMediator.UpdateMouseWheelRight(mapping);
                        break;
                }

                break;
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