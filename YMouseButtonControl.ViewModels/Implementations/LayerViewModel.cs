using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Collections;
using Avalonia.Media;
using DynamicData;
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
    private IProfilesService _profilesService;
    private readonly IMouseListener _mouseListener;

    private int _mb1Index;
    private int _mb2Index;
    private int _mb3Index;
    private int _mb4Index;
    private int _mb5Index;
    private int _mwuIndex;
    private int _mwdIndex;
    private int _mwlIndex;
    private int _mwrIndex;

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
    
    public Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel>
        ShowSimulatedKeystrokesPickerInteraction { get; }

    public IProfilesService ProfilesService
    {
        get => _profilesService;
        private set => this.RaiseAndSetIfChanged(ref _profilesService, value);
    }

    public LayerViewModel(IProfilesService profilesService, IMouseListener mouseListener)
    {
        ProfilesService = profilesService;
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
            .DistinctUntilChanged()
            .

        // Bool to represent whether the gear settings button is enabled/disabled
        // var mb1ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mb1,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mb2ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mb2,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mb3ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mb3,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mb4ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mb4,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mb5ComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mb5,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mwuComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mwu,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mwdComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mwd,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mwlComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mwl,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        // var mwrComboSettingCanExecute = this
        //     .WhenAnyValue(x => x.Mwr,
        //         (mapping) =>
        //             mapping is not null
        //             && mapping is not DisabledMapping
        //             && mapping is not NothingMapping
        //             && !string.IsNullOrWhiteSpace(mapping.Keys));
        //
        // // When the gear button is clicked, try to open the key dialog
        // MouseButton1ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseButton1, MouseButton.MouseButton1, force: true); },
        //     mb1ComboSettingCanExecute);
        // MouseButton2ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseButton2, MouseButton.MouseButton2, force: true); },
        //     mb2ComboSettingCanExecute);
        // MouseButton3ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseButton3, MouseButton.MouseButton3, force: true); },
        //     mb3ComboSettingCanExecute);
        // MouseButton4ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseButton4, MouseButton.MouseButton4, force: true); },
        //     mb4ComboSettingCanExecute);
        // MouseButton5ComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseButton5, MouseButton.MouseButton5, force: true); },
        //     mb5ComboSettingCanExecute);
        // MouseWheelUpComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseWheelUp, MouseButton.MouseWheelUp, force: true); },
        //     mwuComboSettingCanExecute);
        // MouseWheelDownComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseWheelDown, MouseButton.MouseWheelDown, force: true); },
        //     mwdComboSettingCanExecute);
        // MouseWheelLeftComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseWheelLeft, MouseButton.MouseWheelLeft, force: true); },
        //     mwlComboSettingCanExecute);
        // MouseWheelRightComboSettingCommand = ReactiveCommand.CreateFromTask(
        //     async () => { await GetMappingAsync(_profilesService.CurrentProfile.MouseWheelRight, MouseButton.MouseWheelRight, force: true); },
        //     mwrComboSettingCanExecute);
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

    private async Task UpdateMouseOnMappingChange(IButtonMapping mapping, MouseButton button, bool force = false)
    {
        switch (button)
        {
            case MouseButton.MouseButton1:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseButton1, force);
                _profilesService.CurrentProfile.MouseButton1 = mapping;
                break;
            case MouseButton.MouseButton2:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseButton2, force);
                _profilesService.CurrentProfile.MouseButton2 = mapping;
                break;
            case MouseButton.MouseButton3:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseButton3, force);
                _profilesService.CurrentProfile.MouseButton3 = mapping;
                break;
            case MouseButton.MouseButton4:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseButton4, force);
                _profilesService.CurrentProfile.MouseButton4 = mapping;
                break;
            case MouseButton.MouseButton5:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseButton5, force);
                _profilesService.CurrentProfile.MouseButton5 = mapping;
                break;
            case MouseButton.MouseWheelUp:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseWheelUp, force);
                _profilesService.CurrentProfile.MouseWheelUp = mapping;
                break;
            case MouseButton.MouseWheelDown:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseWheelDown, force);
                _profilesService.CurrentProfile.MouseWheelDown = mapping;
                break;
            case MouseButton.MouseWheelLeft:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseWheelLeft, force);
                _profilesService.CurrentProfile.MouseWheelLeft = mapping;
                break;
            case MouseButton.MouseWheelRight:
                mapping = await GetMappingAsync(mapping, MouseButton.MouseWheelRight, force);
                _profilesService.CurrentProfile.MouseWheelRight = mapping;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }
    }
    
    private async Task<IButtonMapping> GetMappingAsync(IButtonMapping mapping, MouseButton button, bool force = false)
    {
        if (!force && mapping is not null && !string.IsNullOrWhiteSpace(mapping.Keys))
        {
            return mapping;
        }
        var newMapping = mapping switch
        {
            SimulatedKeystrokes => await ShowSimulatedKeystrokesDialog(),
            _ => mapping
        };
        if (newMapping is null)
        {
            return null;
        }
        // _profilesService.UpdateCurrentMouse(newMapping, button);
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
                // Mb2Index = newMapping.Index;
                break;
            case MouseButton.MouseButton3:
                MouseButton3Combo.RemoveAt(newMapping.Index);
                MouseButton3Combo.Insert(newMapping.Index, newMapping);
                // Mb3Index = newMapping.Index;
                break;
            case MouseButton.MouseButton4:
                MouseButton4Combo.RemoveAt(newMapping.Index);
                MouseButton4Combo.Insert(newMapping.Index, newMapping);
                // Mb4Index = newMapping.Index;
                break;
            case MouseButton.MouseButton5:
                MouseButton5Combo.RemoveAt(newMapping.Index);
                MouseButton5Combo.Insert(newMapping.Index, newMapping);
                // Mb5Index = newMapping.Index;
                break;
            case MouseButton.MouseWheelUp:
                MouseWheelUpCombo.RemoveAt(newMapping.Index);
                MouseWheelUpCombo.Insert(newMapping.Index, newMapping);
                // MwuIndex = newMapping.Index;
                break;
            case MouseButton.MouseWheelDown:
                MouseWheelDownCombo.RemoveAt(newMapping.Index);
                MouseWheelDownCombo.Insert(newMapping.Index, newMapping);
                // MwdIndex = newMapping.Index;
                break;
            case MouseButton.MouseWheelLeft:
                MouseWheelLeftCombo.RemoveAt(newMapping.Index);
                MouseWheelLeftCombo.Insert(newMapping.Index, newMapping);
                // MwlIndex = newMapping.Index;
                break;
            case MouseButton.MouseWheelRight:
                MouseWheelRightCombo.RemoveAt(newMapping.Index);
                MouseWheelRightCombo.Insert(newMapping.Index, newMapping);
                // MwrIndex = newMapping.Index;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }

        return newMapping;
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