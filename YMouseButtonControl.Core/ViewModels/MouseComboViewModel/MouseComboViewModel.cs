using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Timers;
using Avalonia.Media;
using ReactiveUI;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.Theme;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.MouseComboViewModel;

public interface IMouseComboViewModel
{
    BaseButtonMappingVm? SelectedBtnMap { get; }
    IShowSimulatedKeystrokesDialogService ShowSimulatedKeystrokesDialogService { get; }
    ReadOnlyObservableCollection<BaseButtonMappingVm> BtnMappings { get; }
}

public class MouseComboViewModel : ReactiveObject, IMouseComboViewModel, IDisposable
{
    private readonly ProfileVm _profileVm;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _btnMappings;
    private BaseButtonMappingVm? _selectedBtnMap;
    private readonly IDisposable? _mbDownDisposable;
    private readonly IDisposable? _mbUpDisposable;
    private readonly IDisposable? _mWheelDisposable;
    private IBrush _backgroundColor;
    private readonly Timer _wheelTimer = new() { Interval = 200, AutoReset = false };
    private string? _labelTxt;

    public MouseComboViewModel(
        ProfileVm profileVm,
        IMouseListener mouseListener,
        IThemeService themeService,
        MouseButton mouseButton,
        IShowSimulatedKeystrokesDialogService showSimulatedKeystrokesDialogService,
        ReadOnlyObservableCollection<BaseButtonMappingVm> btnMappings
    )
    {
        _profileVm = profileVm;
        _btnMappings = btnMappings;
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

        SelectedBtnMap = BtnMappings.First(x => x.Selected);
        this.WhenAnyValue(x => x.SelectedBtnMap)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Subscribe(current =>
            {
                profileVm.BtnSc.Edit(inner =>
                {
                    var previous = inner.Items.FirstOrDefault(x =>
                        x.Selected && x.MouseButton == current.MouseButton && !x.Equals(current)
                    );
                    if (previous != null)
                    {
                        previous.Selected = false;
                    }
                    var target = inner.Items.First(x => x.Id == current.Id);
                    target.Selected = true;
                });
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
                    var previouslySelectedBtnMap = BtnMappings.FirstOrDefault(x =>
                        x.Selected && x.Id != newMapping.Id
                    );
                    if (
                        previouslySelectedBtnMap is not null
                        && !previouslySelectedBtnMap.Equals(newMapping)
                    )
                    {
                        previouslySelectedBtnMap.Selected = false;
                        _profileVm.AddOrUpdateBtnMapping([previouslySelectedBtnMap, newMapping]);
                    }
                    else
                    {
                        _profileVm.AddOrUpdateBtnMapping([newMapping]);
                    }
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

    public void Dispose()
    {
        _mbDownDisposable?.Dispose();
        _mbUpDisposable?.Dispose();
        _mWheelDisposable?.Dispose();
        UserClickedEditSettingButton.Dispose();
    }
}
