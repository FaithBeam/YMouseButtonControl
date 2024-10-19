using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Timers;
using Avalonia.Media;
using DynamicData;
using ReactiveUI;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.ViewModels.LayerViewModel;

public interface IMouseComboViewModel
{
    BaseButtonMappingVm? SelectedBtnMap { get; }
    IShowSimulatedKeystrokesDialogService ShowSimulatedKeystrokesDialogService { get; }
}

public class MouseComboViewModel : ReactiveObject, IMouseComboViewModel, IDisposable
{
    private readonly IProfilesService _profilesService;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _btnMappings;
    private BaseButtonMappingVm? _selectedBtnMap;
    private readonly IDisposable? _mbDownDisposable;
    private readonly IDisposable? _mbUpDisposable;
    private readonly IDisposable? _mWheelDisposable;
    private IBrush _backgroundColor;
    private readonly Timer _wheelTimer = new() { Interval = 200, AutoReset = false };
    private string? _labelTxt;

    public MouseComboViewModel(
        IProfilesService profilesService,
        IMouseListener mouseListener,
        IThemeService themeService,
        MouseButton mouseButton,
        IShowSimulatedKeystrokesDialogService showSimulatedKeystrokesDialogService
    )
    {
        _backgroundColor = themeService.Background;
        switch (mouseButton)
        {
            case MouseButton.Mb1:
            case MouseButton.Mb2:
            case MouseButton.Mb3:
            case MouseButton.Mb4:
            case MouseButton.Mb5:
                _mbDownDisposable = mouseListener.OnMousePressedChanged.Subscribe(next =>
                {
                    if (next.Button == (YMouseButton)(mouseButton + 1))
                    {
                        BackgroundColor = themeService.Highlight;
                    }
                });
                _mbUpDisposable = mouseListener.OnMouseReleasedChanged.Subscribe(next =>
                {
                    if (next.Button == (YMouseButton)(mouseButton + 1))
                    {
                        BackgroundColor = themeService.Background;
                    }
                });
                break;
            case MouseButton.Mwu:
            case MouseButton.Mwd:
            case MouseButton.Mwl:
            case MouseButton.Mwr:
                _wheelTimer.Elapsed += delegate
                {
                    BackgroundColor = themeService.Background;
                };
                _mWheelDisposable = mouseListener.OnMouseWheelChanged.Subscribe(next =>
                {
                    switch (next.Direction)
                    {
                        case WheelScrollDirection.VerticalUp when mouseButton == MouseButton.Mwu:
                        case WheelScrollDirection.VerticalDown when mouseButton == MouseButton.Mwd:
                        case WheelScrollDirection.HorizontalRight
                            when mouseButton == MouseButton.Mwr:
                        case WheelScrollDirection.HorizontalLeft
                            when mouseButton == MouseButton.Mwl:
                            MouseWheelDoHighlight();
                            break;
                    }
                });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mouseButton), mouseButton, null);
        }

        _profilesService = profilesService;
        SourceCache<BaseButtonMappingVm, int> sourceButtonMappings = new(x => x.Index);
        var myOp = sourceButtonMappings.Connect().AutoRefresh().Bind(out _btnMappings).Subscribe();
        sourceButtonMappings.AddOrUpdate(GetButtonMappings(mouseButton));
        this.WhenAnyValue(x => x._profilesService.CurrentProfile)
            .WhereNotNull()
            .DistinctUntilChanged()
            .Subscribe(newProfile =>
            {
                sourceButtonMappings.Edit(updater =>
                {
                    updater.Clear();
                    updater.AddOrUpdate(GetButtonMappings(mouseButton));

                    var src =
                        newProfile.ButtonMappings.First(x => x.MouseButton == mouseButton)
                        ?? throw new Exception("Error retrieving button mapping");
                    updater.AddOrUpdate(src);
                });

                var found = _btnMappings.FirstOrDefault(x => x.Selected);
                SelectedBtnMap = found ?? _btnMappings.MinBy(x => x.Index);
            });
        ShowSimulatedKeystrokesDialogService = showSimulatedKeystrokesDialogService;
        var canClickUserClickedSettingsBtn = this.WhenAnyValue(
            x => x.SelectedBtnMap,
            selector: btnMap => btnMap is SimulatedKeystrokeVm
        );
        UserClickedEditSettingButton = ReactiveCommand.CreateFromTask(
            async () =>
            {
                if (SelectedBtnMap is null)
                {
                    return;
                }

                var newMapping =
                    await ShowSimulatedKeystrokesDialogService.ShowSimulatedKeystrokesDialog(
                        "Something",
                        mouseButton,
                        SelectedBtnMap
                    );
                if (newMapping is not null)
                {
                    newMapping.Selected = true;
                    sourceButtonMappings.AddOrUpdate(newMapping);
                    SelectedBtnMap = newMapping;
                }
            },
            canClickUserClickedSettingsBtn
        );
        return;

        void MouseWheelDoHighlight()
        {
            BackgroundColor = themeService.Highlight;
            if (!_wheelTimer.Enabled)
            {
                _wheelTimer.Start();
            }
        }
    }

    public IBrush BackgroundColor
    {
        get => _backgroundColor;
        set => this.RaiseAndSetIfChanged(ref _backgroundColor, value);
    }

    public IShowSimulatedKeystrokesDialogService ShowSimulatedKeystrokesDialogService { get; }

    public BaseButtonMappingVm? SelectedBtnMap
    {
        get => _selectedBtnMap;
        set => this.RaiseAndSetIfChanged(ref _selectedBtnMap, value);
    }

    public string? LabelTxt
    {
        get => _labelTxt;
        set => this.RaiseAndSetIfChanged(ref _labelTxt, value);
    }

    public ReadOnlyObservableCollection<BaseButtonMappingVm> BtnMappings => _btnMappings;

    public ReactiveCommand<Unit, Unit> UserClickedEditSettingButton { get; set; }

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

    public void Dispose()
    {
        _mbDownDisposable?.Dispose();
        _mbUpDisposable?.Dispose();
        _mWheelDisposable?.Dispose();
        UserClickedEditSettingButton.Dispose();
    }
}
