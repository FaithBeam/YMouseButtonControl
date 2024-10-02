using System;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.ViewModels.LayerViewModel;

public interface ILayerViewModel;

public class LayerViewModel : ViewModelBase, ILayerViewModel
{
    public LayerViewModel(
        IMouseComboViewModelFactory mbComboViewModelFactory,
        IProfilesService profilesService
    )
    {
        Mb1ComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mb1);
        Mb2ComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mb2);
        Mb3ComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mb3);
        Mb4ComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mb4);
        Mb5ComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mb5);
        MwuComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mwu);
        MwdComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mwd);
        MwlComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mwl);
        MwrComboVm = mbComboViewModelFactory.CreateWithMouseButton(MouseButton.Mwr);

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
