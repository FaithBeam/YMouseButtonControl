using System;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.LayerViewModel;

public interface ILayerViewModel;

public class LayerViewModel : ViewModelBase, ILayerViewModel
{
    public LayerViewModel(
        IMouseComboViewModelFactory mbComboViewModelFactory,
        IProfilesService profilesService
    )
    {
        Mb1ComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mb1, "Left Button");
        Mb2ComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mb2, "Right Button");
        Mb3ComboVm = mbComboViewModelFactory.CreateWithMouseButton(
            MouseButton.Mb3,
            "Middle Button"
        );
        Mb4ComboVm = mbComboViewModelFactory.CreateWithMouseButton(
            MouseButton.Mb4,
            "Mouse Button 4"
        );
        Mb5ComboVm = mbComboViewModelFactory.CreateWithMouseButton(
            MouseButton.Mb5,
            "Mouse Button 5"
        );
        MwuComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mwu, "Wheel Up");
        MwdComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mwd, "Wheel Down");
        MwlComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mwl, "Wheel Left");
        MwrComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mwr, "Wheel Right");

        this.WhenAnyValue(x => x.Mb1ComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseButton1 = x;
                }
            });
        this.WhenAnyValue(x => x.Mb2ComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseButton2 = x;
                }
            });
        this.WhenAnyValue(x => x.Mb3ComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseButton3 = x;
                }
            });
        this.WhenAnyValue(x => x.Mb4ComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseButton4 = x;
                }
            });
        this.WhenAnyValue(x => x.Mb5ComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseButton5 = x;
                }
            });
        this.WhenAnyValue(x => x.MwuComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseWheelUp = x;
                }
            });
        this.WhenAnyValue(x => x.MwdComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseWheelDown = x;
                }
            });
        this.WhenAnyValue(x => x.MwlComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseWheelLeft = x;
                }
            });
        this.WhenAnyValue(x => x.MwrComboVm.SelectedBtnMap)
            .WhereNotNull()
            .Subscribe(x =>
            {
                if (profilesService.CurrentProfile is not null)
                {
                    profilesService.CurrentProfile.MouseWheelRight = x;
                }
            });
    }

    public IMouseComboViewModel Mb1ComboVm { get; }

    public IMouseComboViewModel Mb2ComboVm { get; }

    public IMouseComboViewModel Mb3ComboVm { get; }

    public IMouseComboViewModel Mb4ComboVm { get; }

    public IMouseComboViewModel Mb5ComboVm { get; }
    public IMouseComboViewModel MwrComboVm { get; }

    public IMouseComboViewModel MwlComboVm { get; }

    public IMouseComboViewModel MwdComboVm { get; }

    public IMouseComboViewModel MwuComboVm { get; }
}
