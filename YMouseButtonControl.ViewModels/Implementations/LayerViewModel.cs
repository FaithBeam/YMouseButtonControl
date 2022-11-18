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
    private readonly IProfilesService _profilesService;
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

    public Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel>
        ShowSimulatedKeystrokesPickerInteraction { get; }

    public bool ComputerEditing { get; set; } = false;
    
    public LayerViewModel(IProfilesService profilesService, IMouseListener mouseListener)
    {
        _profilesService = profilesService;
        this
            .WhenAnyValue(x => x._profilesService.CurrentProfile)
            .WhereNotNull()
            .DistinctUntilChanged()
            .Subscribe(OnSelectedCurrentProfileChanged);
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

        this
            .WhenAnyValue(x => x.Mb1)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Where(_ => !ComputerEditing)
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseButton1));
        this
            .WhenAnyValue(x => x.Mb2)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Where(_ => !ComputerEditing)
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseButton2));
        this
            .WhenAnyValue(x => x.Mb3)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseButton3));
        this
            .WhenAnyValue(x => x.Mb4)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseButton4));
        this
            .WhenAnyValue(x => x.Mb5)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseButton5));
        this
            .WhenAnyValue(x => x.Mwu)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseWheelUp));
        this
            .WhenAnyValue(x => x.Mwd)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseWheelDown));
        this
            .WhenAnyValue(x => x.Mwl)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseWheelLeft));
        this
            .WhenAnyValue(x => x.Mwr)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Subscribe(async x => await HumanSwitchedComboBoxAsync(x, MouseButton.MouseWheelRight));

        // Bool to represent whether the gear settings button is enabled/disabled
        var mb1ComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mb1,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mb2ComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mb2,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mb3ComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mb3,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mb4ComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mb4,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mb5ComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mb5,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mwuComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mwu,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mwdComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mwd,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mwlComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mwl,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));
        var mwrComboSettingCanExecute = this
            .WhenAnyValue(x => x.Mwr,
                (mapping) =>
                    mapping is not null
                    && mapping is not DisabledMapping
                    && mapping is not NothingMapping
                    && !string.IsNullOrWhiteSpace(mapping.Keys));

        // When the gear button is clicked, try to open the key dialog
        MouseButton1ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mb1, MouseButton.MouseButton1); },
            mb1ComboSettingCanExecute);
        MouseButton2ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mb2, MouseButton.MouseButton2); },
            mb2ComboSettingCanExecute);
        MouseButton3ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mb3, MouseButton.MouseButton3); },
            mb3ComboSettingCanExecute);
        MouseButton4ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mb4, MouseButton.MouseButton4); },
            mb4ComboSettingCanExecute);
        MouseButton5ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mb5, MouseButton.MouseButton5); },
            mb5ComboSettingCanExecute);
        MouseWheelUpComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mwu, MouseButton.MouseWheelUp); },
            mwuComboSettingCanExecute);
        MouseWheelDownComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mwd, MouseButton.MouseWheelDown); },
            mwdComboSettingCanExecute);
        MouseWheelLeftComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mwl, MouseButton.MouseWheelLeft); },
            mwlComboSettingCanExecute);
        MouseWheelRightComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () => { await GetMappingAsync(Mwr, MouseButton.MouseWheelRight); },
            mwrComboSettingCanExecute);
    }

    private void UpdateMouseOnIndexChange(int index, MouseButton button)
    {
        if (index < 0)
        {
            return;
        }

        IButtonMapping mapping;
        switch (button)
        {
            case MouseButton.MouseButton1:
                mapping = MouseButton1Combo[index];
                Mb1 = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseButton1);
                break;
            case MouseButton.MouseButton2:
                mapping = MouseButton2Combo[index];
                Mb2 = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseButton2);
                break;
            case MouseButton.MouseButton3:
                mapping = MouseButton3Combo[index];
                Mb3 = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseButton3);
                break;
            case MouseButton.MouseButton4:
                mapping = MouseButton4Combo[index];
                Mb4 = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseButton4);
                break;
            case MouseButton.MouseButton5:
                mapping = MouseButton5Combo[index];
                Mb5 = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseButton5);
                break;
            case MouseButton.MouseWheelUp:
                mapping = MouseWheelUpCombo[index];
                Mwu = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseWheelUp);
                break;
            case MouseButton.MouseWheelDown:
                mapping = MouseWheelDownCombo[index];
                Mwd = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseWheelDown);
                break;
            case MouseButton.MouseWheelLeft:
                mapping = MouseWheelLeftCombo[index];
                Mwl = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseWheelLeft);
                break;
            case MouseButton.MouseWheelRight:
                mapping = MouseWheelRightCombo[index];
                Mwr = mapping;
                _profilesService.UpdateCurrentMouse(mapping, MouseButton.MouseWheelRight);
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

    public AvaloniaList<IButtonMapping> MouseWheelDownCombo { get; set; } =
        new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseWheelLeftCombo { get; set; } =
        new(ButtonMappingFactory.GetButtonMappings());

    public AvaloniaList<IButtonMapping> MouseWheelRightCombo { get; set; } =
        new(ButtonMappingFactory.GetButtonMappings());

    private void OnSelectedCurrentProfileChanged(Profile newProfile)
    {
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseButton1Combo[mapping.Index] = mapping;
        }
        MouseButton1Combo[newProfile.MouseButton1.Index] = newProfile.MouseButton1;
        
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseButton2Combo[mapping.Index] = mapping;
        }
        MouseButton2Combo[newProfile.MouseButton2.Index] = newProfile.MouseButton2;
        
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseButton3Combo[mapping.Index] = mapping;
        }
        MouseButton3Combo[newProfile.MouseButton3.Index] = newProfile.MouseButton3;
        
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseButton4Combo[mapping.Index] = mapping;
        }
        MouseButton4Combo[newProfile.MouseButton4.Index] = newProfile.MouseButton4;
        
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseButton5Combo[mapping.Index] = mapping;
        }
        MouseButton5Combo[newProfile.MouseButton5.Index] = newProfile.MouseButton5;
        
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseWheelUpCombo[mapping.Index] = mapping;
        }
        MouseWheelUpCombo[newProfile.MouseWheelUp.Index] = newProfile.MouseWheelUp;
        
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseWheelDownCombo[mapping.Index] = mapping;
        }
        MouseWheelDownCombo[newProfile.MouseWheelDown.Index] = newProfile.MouseWheelDown;
        
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseWheelLeftCombo[mapping.Index] = mapping;
        }
        MouseWheelLeftCombo[newProfile.MouseWheelLeft.Index] = newProfile.MouseWheelLeft;
        
        foreach (var mapping in ButtonMappingFactory.GetButtonMappings())
        {
            MouseWheelRightCombo[mapping.Index] = mapping;
        }
        MouseWheelRightCombo[newProfile.MouseWheelRight.Index] = newProfile.MouseWheelRight;

        Mb1Index = newProfile.MouseButton1LastIndex;
        Mb2Index = newProfile.MouseButton2LastIndex;
        Mb3Index = newProfile.MouseButton3LastIndex;
        Mb4Index = newProfile.MouseButton4LastIndex;
        Mb5Index = newProfile.MouseButton5LastIndex;
        MwuIndex = newProfile.MouseWheelUpLastIndex;
        MwdIndex = newProfile.MouseWheelDownLastIndex;
        MwlIndex = newProfile.MouseWheelLeftLastIndex;
        MwrIndex = newProfile.MouseWheelRightLastIndex;
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
            case MouseButton.MouseButton1:
                MouseButton1BackgroundColor = Brushes.White;
                break;
            case MouseButton.MouseButton2:
                MouseButton2BackgroundColor = Brushes.White;
                break;
            case MouseButton.MouseButton3:
                MouseButton3BackgroundColor = Brushes.White;
                break;
            case MouseButton.MouseButton4:
                MouseButton4BackgroundColor = Brushes.White;
                break;
            case MouseButton.MouseButton5:
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
            case MouseButton.MouseButton1:
                MouseButton1BackgroundColor = Brushes.Yellow;
                break;
            case MouseButton.MouseButton2:
                MouseButton2BackgroundColor = Brushes.Yellow;
                break;
            case MouseButton.MouseButton3:
                MouseButton3BackgroundColor = Brushes.Yellow;
                break;
            case MouseButton.MouseButton4:
                MouseButton4BackgroundColor = Brushes.Yellow;
                break;
            case MouseButton.MouseButton5:
                MouseButton5BackgroundColor = Brushes.Yellow;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task HumanSwitchedComboBoxAsync(IButtonMapping mapping, MouseButton button)
    {
        if (!string.IsNullOrWhiteSpace(mapping.Keys))
        {
            return;
        }

        if (ComputerEditing)
        {
            return;
        }

        ComputerEditing = true;
        await GetMappingAsync(mapping, button);
        ComputerEditing = false;
    }
    
    private async Task GetMappingAsync(IButtonMapping mapping, MouseButton button)
    {
        var newMapping = mapping switch
        {
            SimulatedKeystrokes => await ShowSimulatedKeystrokesDialog(),
            _ => mapping
        };
        if (newMapping is null)
        {
            return;
        }
        _profilesService.UpdateCurrentMouse(newMapping, button);
        switch (button)
        {
            case MouseButton.MouseButton1:
                MouseButton1Combo.RemoveAt(newMapping.Index);
                MouseButton1Combo.Insert(newMapping.Index, newMapping);
                // Mb1Index = newMapping.Index;
                break;
            case MouseButton.MouseButton2:
                MouseButton2Combo.RemoveAt(newMapping.Index);
                MouseButton2Combo.Insert(newMapping.Index, newMapping);
                Mb2Index = newMapping.Index;
                break;
            case MouseButton.MouseButton3:
                MouseButton3Combo.RemoveAt(newMapping.Index);
                MouseButton3Combo.Insert(newMapping.Index, newMapping);
                Mb3Index = newMapping.Index;
                break;
            case MouseButton.MouseButton4:
                MouseButton4Combo.RemoveAt(newMapping.Index);
                MouseButton4Combo.Insert(newMapping.Index, newMapping);
                Mb4Index = newMapping.Index;
                break;
            case MouseButton.MouseButton5:
                MouseButton5Combo.RemoveAt(newMapping.Index);
                MouseButton5Combo.Insert(newMapping.Index, newMapping);
                Mb5Index = newMapping.Index;
                break;
            case MouseButton.MouseWheelUp:
                MouseWheelUpCombo.RemoveAt(newMapping.Index);
                MouseWheelUpCombo.Insert(newMapping.Index, newMapping);
                MwuIndex = newMapping.Index;
                break;
            case MouseButton.MouseWheelDown:
                MouseWheelDownCombo.RemoveAt(newMapping.Index);
                MouseWheelDownCombo.Insert(newMapping.Index, newMapping);
                MwdIndex = newMapping.Index;
                break;
            case MouseButton.MouseWheelLeft:
                MouseWheelLeftCombo.RemoveAt(newMapping.Index);
                MouseWheelLeftCombo.Insert(newMapping.Index, newMapping);
                MwlIndex = newMapping.Index;
                break;
            case MouseButton.MouseWheelRight:
                MouseWheelRightCombo.RemoveAt(newMapping.Index);
                MouseWheelRightCombo.Insert(newMapping.Index, newMapping);
                MwrIndex = newMapping.Index;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
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