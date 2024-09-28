using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Timers;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using ReactiveUI;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.ViewModels.LayerViewModel;

public interface ILayerViewModel;

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

    private static readonly IBrush HighlightLight = Brushes.Yellow;
    private static readonly IBrush HighlightDark = Brush.Parse("#3700b3");
    private static readonly IBrush BackgroundDark = Brushes.Black;
    private static readonly IBrush BackgroundLight = Brushes.White;
    private readonly IBrush _curBackground;
    private readonly IBrush _curHighlight;

    private IBrush _mouseButton1BackgroundColor;
    private IBrush _mouseButton2BackgroundColor;
    private IBrush _mouseButton3BackgroundColor;
    private IBrush _mouseButton4BackgroundColor;
    private IBrush _mouseButton5BackgroundColor;
    private IBrush _wheelUpBackgroundColor;
    private IBrush _wheelDownBackgroundColor;
    private IBrush _wheelRightBackgroundColor;
    private IBrush _wheelLeftBackgroundColor;

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

    public IProfilesService ProfilesService
    {
        get => _profilesService;
        private set => this.RaiseAndSetIfChanged(ref _profilesService, value);
    }

    public IShowSimulatedKeystrokesDialogService ShowSimulatedKeystrokesDialogService { get; }

    private async Task OnComboIndexChangedAsync(
        IReadOnlyList<BaseButtonMappingVm> list,
        Action<BaseButtonMappingVm> setComboList,
        int index,
        Action<BaseButtonMappingVm> setMouseMapping,
        MouseButton mouseButton,
        string buttonName,
        bool settingsGearClicked = false
    )
    {
        if (index < 0)
        {
            return;
        }
        var mapping = list[index];
        var newMapping = mapping;
        if (
            mapping is SimulatedKeystrokeVm
            && (string.IsNullOrWhiteSpace(mapping.Keys) || settingsGearClicked)
        )
        {
            newMapping = await ShowSimulatedKeystrokesDialogService.ShowSimulatedKeystrokesDialog(
                buttonName,
                mouseButton,
                mapping
            );
            if (newMapping is null)
            {
                return;
            }
            setComboList(newMapping);
        }

        setMouseMapping(newMapping);
    }

    private static bool CanClickGearButton(BaseButtonMappingVm? mapping) =>
        mapping is not null && mapping.CanRaiseDialog && !string.IsNullOrWhiteSpace(mapping.Keys);

    public LayerViewModel(
        IProfilesService profilesService,
        IMouseListener mouseListener,
        IShowSimulatedKeystrokesDialogService showSimulatedKeystrokesDialogService
    )
    {
        _curBackground = GetCurrentThemeBackground();
        _curHighlight = GetCurrentThemeHighlight();
        _mouseButton1BackgroundColor = _curBackground;
        _mouseButton2BackgroundColor = _curBackground;
        _mouseButton3BackgroundColor = _curBackground;
        _mouseButton4BackgroundColor = _curBackground;
        _mouseButton5BackgroundColor = _curBackground;
        _wheelDownBackgroundColor = _curBackground;
        _wheelUpBackgroundColor = _curBackground;
        _wheelLeftBackgroundColor = _curBackground;
        _wheelRightBackgroundColor = _curBackground;

        _profilesService = profilesService;
        this.WhenAnyValue(x => x._profilesService.CurrentProfile)
            .WhereNotNull()
            .DistinctUntilChanged()
            .Subscribe(OnSelectedCurrentProfileChanged);
        _mouseListener = mouseListener;
        _mouseListener.OnMousePressedEventHandler += OnMouseClicked;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
        _mouseListener.OnMouseWheelEventHandler += OnWheelScroll;
        _wheelUpTimer.Elapsed += delegate
        {
            WheelUpBackgroundColor = _curBackground;
        };
        _wheelDownTimer.Elapsed += delegate
        {
            WheelDownBackgroundColor = _curBackground;
        };
        _wheelLeftTimer.Elapsed += delegate
        {
            WheelLeftBackgroundColor = _curBackground;
        };
        _wheelRightTimer.Elapsed += delegate
        {
            WheelRightBackgroundColor = _curBackground;
        };
        ShowSimulatedKeystrokesDialogService = showSimulatedKeystrokesDialogService;

        this.WhenAnyValue(x => x.Mb1Index)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    list: MouseButton1Combo,
                    setComboList: value =>
                    {
                        MouseButton1Combo[x] = value;
                        Mb1Index = x;
                    },
                    index: x,
                    setMouseMapping: value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseButton1.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseButton1 = value;
                        }
                    },
                    mouseButton: MouseButton.Mb1,
                    buttonName: "Left Mouse Button"
                )
            );
        this.WhenAnyValue(x => x.Mb2Index)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    MouseButton2Combo,
                    value =>
                    {
                        MouseButton2Combo[x] = value;
                        Mb2Index = x;
                    },
                    x,
                    value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseButton2.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseButton2 = value;
                        }
                    },
                    mouseButton: MouseButton.Mb2,
                    buttonName: "Right Mouse Button"
                )
            );
        this.WhenAnyValue(x => x.Mb3Index)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    MouseButton3Combo,
                    value =>
                    {
                        MouseButton3Combo[x] = value;
                        Mb3Index = x;
                    },
                    x,
                    value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseButton3.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseButton3 = value;
                        }
                    },
                    mouseButton: MouseButton.Mb3,
                    buttonName: "Middle Mouse Button"
                )
            );
        this.WhenAnyValue(x => x.Mb4Index)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    list: MouseButton4Combo,
                    setComboList: value =>
                    {
                        MouseButton4Combo[x] = value;
                        Mb4Index = x;
                    },
                    index: x,
                    setMouseMapping: value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseButton4.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseButton4 = value;
                        }
                    },
                    mouseButton: MouseButton.Mb4,
                    buttonName: "Mouse Button 4"
                )
            );
        this.WhenAnyValue(x => x.Mb5Index)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    MouseButton5Combo,
                    value =>
                    {
                        MouseButton5Combo[x] = value;
                        Mb5Index = x;
                    },
                    x,
                    value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseButton5.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseButton5 = value;
                        }
                    },
                    mouseButton: MouseButton.Mb5,
                    buttonName: "Mouse Button 5"
                )
            );
        this.WhenAnyValue(x => x.MwuIndex)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    MouseWheelUpCombo,
                    value =>
                    {
                        MouseWheelUpCombo[x] = value;
                        MwuIndex = x;
                    },
                    x,
                    value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseWheelUp.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseWheelUp = value;
                        }
                    },
                    mouseButton: MouseButton.Mwu,
                    buttonName: "Mouse Wheel Up"
                )
            );
        this.WhenAnyValue(x => x.MwdIndex)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    MouseWheelDownCombo,
                    value =>
                    {
                        MouseWheelDownCombo[x] = value;
                        MwdIndex = x;
                    },
                    x,
                    value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseWheelDown.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseWheelDown = value;
                        }
                    },
                    mouseButton: MouseButton.Mwd,
                    buttonName: "Mouse Wheel Down"
                )
            );
        this.WhenAnyValue(x => x.MwlIndex)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    MouseWheelLeftCombo,
                    value =>
                    {
                        MouseWheelLeftCombo[x] = value;
                        MwlIndex = x;
                    },
                    x,
                    value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseWheelLeft.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseWheelLeft = value;
                        }
                    },
                    mouseButton: MouseButton.Mwl,
                    buttonName: "Mouse Wheel Left"
                )
            );
        this.WhenAnyValue(x => x.MwrIndex)
            .DistinctUntilChanged()
            .Subscribe(async x =>
                await OnComboIndexChangedAsync(
                    MouseWheelRightCombo,
                    value =>
                    {
                        MouseWheelRightCombo[x] = value;
                        MwrIndex = x;
                    },
                    x,
                    value =>
                    {
                        if (
                            _profilesService.CurrentProfile is not null
                            && !_profilesService.CurrentProfile.MouseWheelRight.Equals(value)
                        )
                        {
                            _profilesService.CurrentProfile.MouseWheelRight = value;
                        }
                    },
                    mouseButton: MouseButton.Mwr,
                    buttonName: "Mouse Wheel Right"
                )
            );

        // Bool to represent whether the gear settings button is enabled/disabled
        var mb1ComboSettingCanExecute = this.WhenAnyValue(x => x.Mb1Index)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseButton1Combo[x]));
        var mb2ComboSettingCanExecute = this.WhenAnyValue(x => x.Mb2Index)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseButton2Combo[x]));
        var mb3ComboSettingCanExecute = this.WhenAnyValue(x => x.Mb3Index)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseButton3Combo[x]));
        var mb4ComboSettingCanExecute = this.WhenAnyValue(x => x.Mb4Index)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseButton4Combo[x]));
        var mb5ComboSettingCanExecute = this.WhenAnyValue(x => x.Mb5Index)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseButton5Combo[x]));
        var mwuComboSettingCanExecute = this.WhenAnyValue(x => x.MwuIndex)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseWheelUpCombo[x]));
        var mwdComboSettingCanExecute = this.WhenAnyValue(x => x.MwdIndex)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseWheelDownCombo[x]));
        var mwlComboSettingCanExecute = this.WhenAnyValue(x => x.MwlIndex)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseWheelLeftCombo[x]));
        var mwrComboSettingCanExecute = this.WhenAnyValue(x => x.MwrIndex)
            .Where(x => x >= 0)
            .Select(x => CanClickGearButton(MouseWheelRightCombo[x]));

        // When the gear button is clicked, try to open the key dialog
        MouseButton1ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseButton1Combo,
                    value =>
                    {
                        MouseButton1Combo[Mb1Index] = value;
                        Mb1Index = value.Index;
                    },
                    Mb1Index,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseButton1 = value;
                    },
                    mouseButton: MouseButton.Mb1,
                    buttonName: "Left Mouse Button",
                    true
                );
            },
            mb1ComboSettingCanExecute
        );
        MouseButton2ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseButton2Combo,
                    value =>
                    {
                        MouseButton2Combo[Mb2Index] = value;
                        Mb2Index = value.Index;
                    },
                    Mb2Index,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseButton2 = value;
                    },
                    mouseButton: MouseButton.Mb2,
                    buttonName: "Right Mouse Button",
                    true
                );
            },
            mb2ComboSettingCanExecute
        );
        MouseButton3ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseButton3Combo,
                    value =>
                    {
                        MouseButton3Combo[Mb3Index] = value;
                        Mb3Index = value.Index;
                    },
                    Mb3Index,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseButton3 = value;
                    },
                    mouseButton: MouseButton.Mb3,
                    buttonName: "Middle Mouse Button",
                    true
                );
            },
            mb3ComboSettingCanExecute
        );
        MouseButton4ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseButton4Combo,
                    value =>
                    {
                        MouseButton4Combo[Mb4Index] = value;
                        Mb4Index = value.Index;
                    },
                    Mb4Index,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseButton4 = value;
                    },
                    mouseButton: MouseButton.Mb4,
                    buttonName: "Mouse Button 4",
                    true
                );
            },
            mb4ComboSettingCanExecute
        );
        MouseButton5ComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseButton5Combo,
                    value =>
                    {
                        MouseButton5Combo[Mb5Index] = value;
                        Mb5Index = value.Index;
                    },
                    Mb5Index,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseButton5 = value;
                    },
                    mouseButton: MouseButton.Mb5,
                    buttonName: "Mouse Button 5",
                    true
                );
            },
            mb5ComboSettingCanExecute
        );
        MouseWheelUpComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseWheelUpCombo,
                    value =>
                    {
                        MouseWheelUpCombo[MwuIndex] = value;
                        MwuIndex = value.Index;
                    },
                    MwuIndex,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseWheelUp = value;
                    },
                    mouseButton: MouseButton.Mwu,
                    buttonName: "Mouse Wheel Up",
                    true
                );
            },
            mwuComboSettingCanExecute
        );
        MouseWheelDownComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseWheelDownCombo,
                    value =>
                    {
                        MouseWheelDownCombo[MwdIndex] = value;
                        MwdIndex = value.Index;
                    },
                    MwdIndex,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseWheelDown = value;
                    },
                    mouseButton: MouseButton.Mwd,
                    buttonName: "Mouse Wheel Down",
                    true
                );
            },
            mwdComboSettingCanExecute
        );
        MouseWheelLeftComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseWheelLeftCombo,
                    value =>
                    {
                        MouseWheelLeftCombo[MwlIndex] = value;
                        MwlIndex = value.Index;
                    },
                    MwlIndex,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseWheelLeft = value;
                    },
                    mouseButton: MouseButton.Mwl,
                    buttonName: "Mouse Wheel Left",
                    true
                );
            },
            mwlComboSettingCanExecute
        );
        MouseWheelRightComboSettingCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                await OnComboIndexChangedAsync(
                    MouseWheelRightCombo,
                    value =>
                    {
                        MouseWheelRightCombo[MwrIndex] = value;
                        MwrIndex = value.Index;
                    },
                    MwrIndex,
                    value =>
                    {
                        Debug.Assert(
                            _profilesService.CurrentProfile != null,
                            "_profilesService.CurrentProfile != null"
                        );
                        _profilesService.CurrentProfile.MouseWheelRight = value;
                    },
                    mouseButton: MouseButton.Mwr,
                    buttonName: "Mouse Wheel Right",
                    true
                );
            },
            mwrComboSettingCanExecute
        );
    }

    private static IBrush GetCurrentThemeBackground()
    {
        if (Application.Current?.ActualThemeVariant == ThemeVariant.Light)
        {
            return BackgroundLight;
        }

        if (Application.Current?.ActualThemeVariant == ThemeVariant.Dark)
        {
            return BackgroundDark;
        }

        throw new Exception("Unknown theme");
    }

    private static IBrush GetCurrentThemeHighlight()
    {
        if (Application.Current?.ActualThemeVariant == ThemeVariant.Light)
        {
            return HighlightLight;
        }

        if (Application.Current?.ActualThemeVariant == ThemeVariant.Dark)
        {
            return HighlightDark;
        }

        throw new Exception("Unknown theme");
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

    public ObservableCollection<BaseButtonMappingVm> MouseButton1Combo { get; set; } =
        new(GetButtonMappings(MouseButton.Mb1));

    public ObservableCollection<BaseButtonMappingVm> MouseButton2Combo { get; set; } =
        new(GetButtonMappings(MouseButton.Mb2));

    public ObservableCollection<BaseButtonMappingVm> MouseButton3Combo { get; set; } =
        new(GetButtonMappings(MouseButton.Mb3));

    public ObservableCollection<BaseButtonMappingVm> MouseButton4Combo { get; set; } =
        new(GetButtonMappings(MouseButton.Mb4));

    public ObservableCollection<BaseButtonMappingVm> MouseButton5Combo { get; set; } =
        new(GetButtonMappings(MouseButton.Mb5));

    public ObservableCollection<BaseButtonMappingVm> MouseWheelUpCombo { get; set; } =
        new(GetButtonMappings(MouseButton.Mwu));

    public ObservableCollection<BaseButtonMappingVm> MouseWheelDownCombo { get; set; } =
        new(GetButtonMappings(MouseButton.Mwd));

    public ObservableCollection<BaseButtonMappingVm> MouseWheelLeftCombo { get; set; } =
        new(GetButtonMappings(MouseButton.Mwl));

    public ObservableCollection<BaseButtonMappingVm> MouseWheelRightCombo { get; set; } =
        new(GetButtonMappings(MouseButton.Mwr));

    private void OnSelectedCurrentProfileChanged(ProfileVm newProfile)
    {
        foreach (var mapping in GetButtonMappings(MouseButton.Mb1))
        {
            MouseButton1Combo[mapping.Index] = mapping;
        }
        MouseButton1Combo[newProfile.MouseButton1.Index] = newProfile.MouseButton1;
        Mb1Index = newProfile.MouseButton1.Index;

        foreach (var mapping in GetButtonMappings(MouseButton.Mb2))
        {
            MouseButton2Combo[mapping.Index] = mapping;
        }
        MouseButton2Combo[newProfile.MouseButton2.Index] = newProfile.MouseButton2;
        Mb2Index = newProfile.MouseButton2.Index;

        foreach (var mapping in GetButtonMappings(MouseButton.Mb3))
        {
            MouseButton3Combo[mapping.Index] = mapping;
        }
        MouseButton3Combo[newProfile.MouseButton3.Index] = newProfile.MouseButton3;
        Mb3Index = newProfile.MouseButton3.Index;

        foreach (var mapping in GetButtonMappings(MouseButton.Mb4))
        {
            MouseButton4Combo[mapping.Index] = mapping;
        }
        MouseButton4Combo[newProfile.MouseButton4.Index] = newProfile.MouseButton4;
        Mb4Index = newProfile.MouseButton4.Index;

        foreach (var mapping in GetButtonMappings(MouseButton.Mb5))
        {
            MouseButton5Combo[mapping.Index] = mapping;
        }
        MouseButton5Combo[newProfile.MouseButton5.Index] = newProfile.MouseButton5;
        Mb5Index = newProfile.MouseButton5.Index;

        foreach (var mapping in GetButtonMappings(MouseButton.Mwu))
        {
            MouseWheelUpCombo[mapping.Index] = mapping;
        }
        MouseWheelUpCombo[newProfile.MouseWheelUp.Index] = newProfile.MouseWheelUp;
        MwuIndex = newProfile.MouseWheelUp.Index;

        foreach (var mapping in GetButtonMappings(MouseButton.Mwd))
        {
            MouseWheelDownCombo[mapping.Index] = mapping;
        }
        MouseWheelDownCombo[newProfile.MouseWheelDown.Index] = newProfile.MouseWheelDown;
        MwdIndex = newProfile.MouseWheelDown.Index;

        foreach (var mapping in GetButtonMappings(MouseButton.Mwl))
        {
            MouseWheelLeftCombo[mapping.Index] = mapping;
        }
        MouseWheelLeftCombo[newProfile.MouseWheelLeft.Index] = newProfile.MouseWheelLeft;
        MwlIndex = newProfile.MouseWheelLeft.Index;

        foreach (var mapping in GetButtonMappings(MouseButton.Mwr))
        {
            MouseWheelRightCombo[mapping.Index] = mapping;
        }
        MouseWheelRightCombo[newProfile.MouseWheelRight.Index] = newProfile.MouseWheelRight;
        MwrIndex = newProfile.MouseWheelRight.Index;
    }

    private void OnWheelScroll(object? sender, NewMouseWheelEventArgs e)
    {
        switch (e.Direction)
        {
            case WheelScrollDirection.VerticalUp:
                WheelUpBackgroundColor = _curHighlight;
                if (!_wheelUpTimer.Enabled)
                {
                    _wheelUpTimer.Start();
                }

                break;
            case WheelScrollDirection.VerticalDown:
                WheelDownBackgroundColor = _curHighlight;
                if (!_wheelDownTimer.Enabled)
                {
                    _wheelDownTimer.Start();
                }

                break;
            case WheelScrollDirection.HorizontalLeft:
                WheelLeftBackgroundColor = _curHighlight;
                if (!_wheelLeftTimer.Enabled)
                {
                    _wheelLeftTimer.Start();
                }

                break;
            case WheelScrollDirection.HorizontalRight:
                WheelRightBackgroundColor = _curHighlight;
                if (!_wheelRightTimer.Enabled)
                {
                    _wheelRightTimer.Start();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseReleased(object? sender, NewMouseHookEventArgs e)
    {
        switch (e.Button)
        {
            case YMouseButton.MouseButton1:
                MouseButton1BackgroundColor = _curBackground;
                break;
            case YMouseButton.MouseButton2:
                MouseButton2BackgroundColor = _curBackground;
                break;
            case YMouseButton.MouseButton3:
                MouseButton3BackgroundColor = _curBackground;
                break;
            case YMouseButton.MouseButton4:
                MouseButton4BackgroundColor = _curBackground;
                break;
            case YMouseButton.MouseButton5:
                MouseButton5BackgroundColor = _curBackground;
                break;
            case YMouseButton.MouseWheelUp:
                break;
            case YMouseButton.MouseWheelDown:
                break;
            case YMouseButton.MouseWheelLeft:
                break;
            case YMouseButton.MouseWheelRight:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseClicked(object? sender, NewMouseHookEventArgs e)
    {
        switch (e.Button)
        {
            case YMouseButton.MouseButton1:
                MouseButton1BackgroundColor = _curHighlight;
                break;
            case YMouseButton.MouseButton2:
                MouseButton2BackgroundColor = _curHighlight;
                break;
            case YMouseButton.MouseButton3:
                MouseButton3BackgroundColor = _curHighlight;
                break;
            case YMouseButton.MouseButton4:
                MouseButton4BackgroundColor = _curHighlight;
                break;
            case YMouseButton.MouseButton5:
                MouseButton5BackgroundColor = _curHighlight;
                break;
            case YMouseButton.MouseWheelUp:
                break;
            case YMouseButton.MouseWheelDown:
                break;
            case YMouseButton.MouseWheelLeft:
                break;
            case YMouseButton.MouseWheelRight:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static IEnumerable<BaseButtonMappingVm> GetButtonMappings(MouseButton mouseButton) =>
        ButtonMappingDictionary.Select(x => x.Value(mouseButton));

    private static readonly Dictionary<
        ButtonMappings,
        Func<MouseButton, BaseButtonMappingVm>
    > ButtonMappingDictionary =
        new()
        {
            {
                ButtonMappings.Nothing,
                mb => new NothingMappingVm { MouseButton = mb }
            },
            {
                ButtonMappings.Disabled,
                mb => new DisabledMappingVm { MouseButton = mb }
            },
            {
                ButtonMappings.SimulatedKeystrokes,
                mb => new SimulatedKeystrokeVm { MouseButton = mb }
            },
            {
                ButtonMappings.RightClick,
                mb => new RightClickVm { MouseButton = mb }
            },
        };

    private enum ButtonMappings
    {
        Nothing,
        Disabled,
        SimulatedKeystrokes,
        RightClick,
    }
}
