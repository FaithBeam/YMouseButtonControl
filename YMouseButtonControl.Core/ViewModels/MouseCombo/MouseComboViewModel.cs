using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Timers;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using ReactiveUI;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.MouseCombo.Queries.Theme;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.MouseCombo;

public interface IMouseComboViewModel
{
    BaseButtonMappingVm? SelectedBtnMap { get; }
    ReadOnlyObservableCollection<BaseButtonMappingVm> BtnMappings { get; }
}

public class MouseComboViewModel : ReactiveObject, IMouseComboViewModel, IDisposable
{
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _btnMappings;
    private BaseButtonMappingVm? _selectedBtnMap;
    private readonly IDisposable? _mbDownDisposable;
    private readonly IDisposable? _mbUpDisposable;
    private readonly IDisposable? _mWheelDisposable;
    private IBrush _backgroundColor;
    private readonly Timer _wheelTimer = new() { Interval = 200, AutoReset = false };
    private string? _labelTxt;

    public MouseComboViewModel(
        SourceCache<BaseButtonMappingVm, int> BtnSc,
        IMouseListener mouseListener,
        ISimulatedKeystrokesDialogVmFactory simulatedKeystrokesDialogVmFactory,
        GetThemeBackground.Handler getThemeBackgroundHandler,
        GetThemeHighlight.Handler getThemeHighlightHandler,
        MouseButton mouseButton,
        ReadOnlyObservableCollection<BaseButtonMappingVm> btnMappings,
        Interaction<
            SimulatedKeystrokesDialogViewModel,
            SimulatedKeystrokeVm?
        > showSimulatedKeystrokesPickerInteraction
    )
    {
        _btnMappings = btnMappings;
        _backgroundColor = getThemeBackgroundHandler.Execute();

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
                        Dispatcher.UIThread.Post(
                            () => BackgroundColor = getThemeHighlightHandler.Execute()
                        );
                    }
                });
                _mbUpDisposable = mouseListener.OnMouseReleasedChanged.Subscribe(next =>
                {
                    if (next.Button == (YMouseButton)(mouseButton + 1))
                    {
                        Dispatcher.UIThread.Post(
                            () => BackgroundColor = getThemeBackgroundHandler.Execute()
                        );
                    }
                });
                break;
            case MouseButton.Mwu:
            case MouseButton.Mwd:
            case MouseButton.Mwl:
            case MouseButton.Mwr:
                _wheelTimer.Elapsed += delegate
                {
                    Dispatcher.UIThread.Post(
                        () => BackgroundColor = getThemeBackgroundHandler.Execute()
                    );
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
                            BackgroundColor = getThemeHighlightHandler.Execute();
                            if (!_wheelTimer.Enabled)
                            {
                                _wheelTimer.Start();
                            }
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
                BtnSc.Edit(inner =>
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

                var newMapping = await showSimulatedKeystrokesPickerInteraction.Handle(
                    simulatedKeystrokesDialogVmFactory.Create(
                        "Something",
                        mouseButton,
                        SelectedBtnMap
                    )
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
                        BtnSc.AddOrUpdate([previouslySelectedBtnMap, newMapping]);
                    }
                    else
                    {
                        BtnSc.AddOrUpdate([newMapping]);
                    }
                    SelectedBtnMap = newMapping;
                }
            },
            canClickUserClickedSettingsBtn
        );
    }

    public IBrush BackgroundColor
    {
        get => _backgroundColor;
        set => this.RaiseAndSetIfChanged(ref _backgroundColor, value);
    }

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
