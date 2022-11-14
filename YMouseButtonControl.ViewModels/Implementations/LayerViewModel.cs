using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Collections;
using Avalonia.Media;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Enums;
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
    
    private readonly Timer _wheelUpTimer = new() {Interval = 200, AutoReset = false};
    private readonly Timer _wheelDownTimer = new() {Interval = 200, AutoReset = false};
    private readonly Timer _wheelLeftTimer = new() {Interval = 200, AutoReset = false};
    private readonly Timer _wheelRightTimer = new() {Interval = 200, AutoReset = false};
    
    private int _mb1Index;
    private int _mb2Index;
    private int _mb3Index;
    private int _mb4Index;
    private int _mb5Index;
    private int _mwuIndex;
    private int _mwdIndex;
    private int _mwlIndex;
    private int _mwrIndex;

    private IButtonMapping _mb1;
    private IButtonMapping _mb2;
    private IButtonMapping _mb3;
    private IButtonMapping _mb4;
    private IButtonMapping _mb5;
    private IButtonMapping _mwu;
    private IButtonMapping _mwd;
    private IButtonMapping _mwl;
    private IButtonMapping _mwr;

    public Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel> ShowSimulatedKeystrokesPickerInteraction { get; }

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
        ShowSimulatedKeystrokesPickerInteraction = new Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel>();

        this
            .WhenAnyValue(x => x.Mb1Index)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseButton1));
        this
            .WhenAnyValue(x => x.Mb2Index)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseButton2));
        this
            .WhenAnyValue(x => x.Mb3Index)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseButton3));
        this
            .WhenAnyValue(x => x.Mb4Index)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseButton4));
        this
            .WhenAnyValue(x => x.Mb5Index)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseButton5));
        this
            .WhenAnyValue(x => x.MwuIndex)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseWheelUp));
        this
            .WhenAnyValue(x => x.MwdIndex)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseWheelDown));
        this
            .WhenAnyValue(x => x.MwlIndex)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseWheelLeft));
        this
            .WhenAnyValue(x => x.MwrIndex)
            .Subscribe(x => UpdateMouseOnIndexChange(x, MouseButton.MouseWheelRight));
        
        // Bool to represent whether the gear settings button is enabled/disabled
        // var mb1ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseButton1Mapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mb2ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseButton2Mapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mb3ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseButton3Mapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mb4ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseButton4Mapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mb5ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseButton5Mapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mwuComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseWheelUpMapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mwdComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseWheelDownMapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mwlComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseWheelLeftMapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mwrComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.SelectedMouseWheelRightMapping,
        //         (mapping) =>
        //             mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));

        // When the gear button is clicked, try to open the key dialog
        // MouseButton1ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseButton1, MouseButton.MouseButton1, true); },
        //     mb1ComboSettingCanExecute);
        // MouseButton2ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseButton2, MouseButton.MouseButton2, true); },
        //     mb2ComboSettingCanExecute);
        // MouseButton3ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseButton3, MouseButton.MouseButton3, true); },
        //     mb3ComboSettingCanExecute);
        // MouseButton4ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseButton4, MouseButton.MouseButton4, true); },
        //     mb4ComboSettingCanExecute);
        // MouseButton5ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseButton5, MouseButton.MouseButton5, true); },
        //     mb5ComboSettingCanExecute);
        // MouseWheelUpComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseWheelUp, MouseButton.MouseWheelUp, true); },
        //     mwuComboSettingCanExecute);
        // MouseWheelDownComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseWheelDown, MouseButton.MouseWheelDown, true); },
        //     mwdComboSettingCanExecute);
        // MouseWheelLeftComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseWheelLeft, MouseButton.MouseWheelLeft, true); },
        //     mwlComboSettingCanExecute);
        // MouseWheelRightComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_currentProfileOperationsMediator.CurrentProfile.MouseWheelRight, MouseButton.MouseWheelRight, true); },
        //     mwrComboSettingCanExecute);
    }

    private void UpdateMouseOnIndexChange(int index, MouseButton button)
    {
        IButtonMapping mapping = default;
        switch (button)
        {
            case MouseButton.MouseButton1:
                mapping = MouseButton1Combo[index];
                break;
            case MouseButton.MouseButton2:
                mapping = MouseButton2Combo[index];
                break;
            case MouseButton.MouseButton3:
                mapping = MouseButton3Combo[index];
                break;
            case MouseButton.MouseButton4:
                mapping = MouseButton4Combo[index];
                break;
            case MouseButton.MouseButton5:
                mapping = MouseButton5Combo[index];
                break;
            case MouseButton.MouseWheelUp:
                mapping = MouseWheelUpCombo[index];
                break;
            case MouseButton.MouseWheelDown:
                mapping = MouseWheelDownCombo[index];
                break;
            case MouseButton.MouseWheelLeft:
                mapping = MouseWheelLeftCombo[index];
                break;
            case MouseButton.MouseWheelRight:
                mapping = MouseWheelRightCombo[index];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }
    }

    public IButtonMapping Mb1
    {
        get => _mb1;
        set => this.RaiseAndSetIfChanged(ref _mb1, value);
    }
    
    public IButtonMapping Mb2
    {
        get => _mb2;
        set => this.RaiseAndSetIfChanged(ref _mb2, value);
    }
    
    public IButtonMapping Mb3
    {
        get => _mb3;
        set => this.RaiseAndSetIfChanged(ref _mb3, value);
    }
    
    public IButtonMapping Mb4
    {
        get => _mb4;
        set => this.RaiseAndSetIfChanged(ref _mb4, value);
    }
    
    public IButtonMapping Mb5
    {
        get => _mb5;
        set => this.RaiseAndSetIfChanged(ref _mb5, value);
    }
    
    public IButtonMapping Mwu
    {
        get => _mwu;
        set => this.RaiseAndSetIfChanged(ref _mwu, value);
    }
    
    public IButtonMapping Mwd
    {
        get => _mwd;
        set => this.RaiseAndSetIfChanged(ref _mwd, value);
    }
    
    public IButtonMapping Mwl
    {
        get => _mwl;
        set => this.RaiseAndSetIfChanged(ref _mwl, value);
    }
    
    public IButtonMapping Mwr
    {
        get => _mwr;
        set => this.RaiseAndSetIfChanged(ref _mwr, value);
    }
    
    public int Mb1Index
    {
        get => _mb1Index;
        set => this.RaiseAndSetIfChanged(ref _mb1Index, value);
    }
    
    public int Mb2Index
    {
        get => _mb2Index;
        set => this.RaiseAndSetIfChanged(ref _mb2Index, value);
    }
    
    public int Mb3Index
    {
        get => _mb3Index;
        set => this.RaiseAndSetIfChanged(ref _mb3Index, value);
    }
    
    public int Mb4Index
    {
        get => _mb4Index;
        set => this.RaiseAndSetIfChanged(ref _mb4Index, value);
    }
    
    public int Mb5Index
    {
        get => _mb5Index;
        set => this.RaiseAndSetIfChanged(ref _mb5Index, value);
    }
    
    public int MwuIndex
    {
        get => _mwuIndex;
        set => this.RaiseAndSetIfChanged(ref _mwuIndex, value);
    }
    
    public int MwdIndex
    {
        get => _mwdIndex;
        set => this.RaiseAndSetIfChanged(ref _mwdIndex, value);
    }
    
    public int MwlIndex
    {
        get => _mwlIndex;
        set => this.RaiseAndSetIfChanged(ref _mwlIndex, value);
    }
    
    public int MwrIndex
    {
        get => _mwrIndex;
        set => this.RaiseAndSetIfChanged(ref _mwrIndex, value);
    }
    
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

    private async Task GetMappingAsync(IButtonMapping mapping, MouseButton button, bool programmaticChange = false)
    {
        switch (mapping)
        {
            case SimulatedKeystrokes:
            {
                // Skip raising the dialog when the mapping was set programmatically
                if (!programmaticChange)
                {
                    var result = await ShowSimulatedKeystrokesDialog();
                    if (result is null)
                    {
                        return;
                    }
                    UpdateCurrentProfileMediator(result, button);
                }
                else
                {
                    IButtonMapping result = default;
                    if (string.IsNullOrWhiteSpace(mapping.Keys))
                    {
                        result = await ShowSimulatedKeystrokesDialog();
                    }
                    else
                    {
                        result = mapping;
                    }
                    if (result is null)
                    {
                        return;
                    }
                    UpdateCurrentProfileMediator(result, button);
                }
                break;
            }
            default:
                UpdateCurrentProfileMediator(mapping, button);
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