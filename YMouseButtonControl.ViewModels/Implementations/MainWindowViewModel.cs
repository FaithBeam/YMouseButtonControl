using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    #region Fields

    private IProfilesService _ps;
    private IProfilesListViewModel _profilesListViewModel;
    private bool _canApply;
    private string _profileName;

    #endregion

    #region Constructor

    public MainWindowViewModel(IProfilesService ps,
        ILayerViewModel layerViewModel, IProfilesListViewModel profilesListViewModel,
        IProfilesInformationViewModel profilesInformationViewModel)
    {
        _profilesListViewModel = profilesListViewModel;
        _ps = ps;
        LayerViewModel = layerViewModel;
        ProfilesInformationViewModel = profilesInformationViewModel;
        var canApply = this
            .WhenAnyValue(x => x._ps.CurrentProfile, x => x._ps.CurrentProfile.MouseButton1,
                x => x._ps.CurrentProfile.MouseButton2,
                x => x._ps.CurrentProfile.MouseButton3, x => x._ps.CurrentProfile.MouseButton4,
                x => x._ps.CurrentProfile.MouseButton5, x => x._ps.Profiles)
            .Select(_ => _ps.IsUnsavedChanges())
            .DistinctUntilChanged();
        ApplyCommand = ReactiveCommand.Create(() => _ps.ApplyProfiles(), canApply);
        this
            .WhenAnyValue(x => x._ps.CurrentProfile)
            .Subscribe(OnProfileChanged);
        ProfileName = _ps.CurrentProfile.Name;
    }

    #endregion

    #region Properties

    public IProfilesInformationViewModel ProfilesInformationViewModel { get; }

    public IProfilesListViewModel ProfilesListViewModel => _profilesListViewModel;

    public ILayerViewModel LayerViewModel { get; }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }

    public bool CanApply
    {
        get => _canApply;
        set => this.RaiseAndSetIfChanged(ref _canApply, value);
    }

    public string ProfileName
    {
        get => _profileName;
        set => this.RaiseAndSetIfChanged(ref _profileName, value);
    }

    #endregion

    private void OnProfileChanged(Profile profile)
    {
        ProfileName = profile.Name;
    }
}