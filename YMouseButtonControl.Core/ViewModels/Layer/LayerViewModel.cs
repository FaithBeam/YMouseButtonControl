using System;
using System.Reactive.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.MouseCombo;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.Layer;

public interface ILayerViewModel;

public class LayerViewModel : ViewModelBase, ILayerViewModel
{
    private IMouseComboViewModel? _mb1ComboVm;
    private IMouseComboViewModel? _mb2ComboVm;
    private IMouseComboViewModel? _mb3ComboVm;
    private IMouseComboViewModel? _mb4ComboVm;
    private IMouseComboViewModel? _mb5ComboVm;
    private IMouseComboViewModel? _mwrComboVm;
    private IMouseComboViewModel? _mwlComboVm;
    private IMouseComboViewModel? _mwdComboVm;
    private IMouseComboViewModel? _mwuComboVm;

    public LayerViewModel(
        IMouseComboViewModelFactory mbComboViewModelFactory,
        IProfilesCache profilesService
    )
    {
        ShowSimulatedKeystrokesPickerInteraction =
            new Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokeVm?>();

        profilesService
            .WhenAnyValue(x => x.CurrentProfile)
            .WhereNotNull()
            .Subscribe(profileVm =>
            {
                Mb1ComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mb1,
                    "Left Button",
                    profileVm.Mb1Mappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
                Mb2ComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mb2,
                    "Right Button",
                    profileVm.Mb2Mappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
                Mb3ComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mb3,
                    "Middle Button",
                    profileVm.Mb3Mappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
                Mb4ComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mb4,
                    "Mouse Button 4",
                    profileVm.Mb4Mappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
                Mb5ComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mb5,
                    "Mouse Button 5",
                    profileVm.Mb5Mappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
                MwuComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mwu,
                    "Wheel Up",
                    profileVm.MwuMappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
                MwdComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mwd,
                    "Wheel Down",
                    profileVm.MwdMappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
                MwlComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mwl,
                    "Wheel Left",
                    profileVm.MwlMappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
                MwrComboVm = mbComboViewModelFactory.CreateWithMouseButton(
                    profileVm.BtnSc,
                    MouseButton.Mwr,
                    "Wheel Right",
                    profileVm.MwrMappings,
                    ShowSimulatedKeystrokesPickerInteraction
                );
            });
    }

    public IMouseComboViewModel? Mb1ComboVm
    {
        get => _mb1ComboVm;
        set => this.RaiseAndSetIfChanged(ref _mb1ComboVm, value);
    }

    public IMouseComboViewModel? Mb2ComboVm
    {
        get => _mb2ComboVm;
        set => this.RaiseAndSetIfChanged(ref _mb2ComboVm, value);
    }

    public IMouseComboViewModel? Mb3ComboVm
    {
        get => _mb3ComboVm;
        set => this.RaiseAndSetIfChanged(ref _mb3ComboVm, value);
    }

    public IMouseComboViewModel? Mb4ComboVm
    {
        get => _mb4ComboVm;
        set => this.RaiseAndSetIfChanged(ref _mb4ComboVm, value);
    }

    public IMouseComboViewModel? Mb5ComboVm
    {
        get => _mb5ComboVm;
        set => this.RaiseAndSetIfChanged(ref _mb5ComboVm, value);
    }

    public IMouseComboViewModel? MwrComboVm
    {
        get => _mwrComboVm;
        set => this.RaiseAndSetIfChanged(ref _mwrComboVm, value);
    }

    public IMouseComboViewModel? MwlComboVm
    {
        get => _mwlComboVm;
        set => this.RaiseAndSetIfChanged(ref _mwlComboVm, value);
    }

    public IMouseComboViewModel? MwdComboVm
    {
        get => _mwdComboVm;
        set => this.RaiseAndSetIfChanged(ref _mwdComboVm, value);
    }

    public IMouseComboViewModel? MwuComboVm
    {
        get => _mwuComboVm;
        set => this.RaiseAndSetIfChanged(ref _mwuComboVm, value);
    }

    public Interaction<
        SimulatedKeystrokesDialogViewModel,
        SimulatedKeystrokeVm?
    > ShowSimulatedKeystrokesPickerInteraction { get; }
}
