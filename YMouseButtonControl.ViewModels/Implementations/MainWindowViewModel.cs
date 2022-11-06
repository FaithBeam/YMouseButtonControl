using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    #region Fields

    private IProfilesService _profilesService;
    private ICurrentProfileOperationsMediator _currentProfileOperationsMediator;
    private IProfilesListViewModel _profilesListViewModel;
    private bool _canApply;
    private string _profileName;

    #endregion

    #region Constructor

    public MainWindowViewModel(IProfilesService profilesService, ICurrentProfileOperationsMediator currentProfileOperationsMediator,
        ILayerViewModel layerViewModel, IProfilesListViewModel profilesListViewModel, IProfilesInformationViewModel profilesInformationViewModel)
    {
        _profilesListViewModel = profilesListViewModel;
        _profilesService = profilesService;
        _currentProfileOperationsMediator = currentProfileOperationsMediator;
        LayerViewModel = layerViewModel;
        ProfilesInformationViewModel = profilesInformationViewModel;
        var profilesChanged =
            Observable.FromEventPattern<ProfilesChangedEventArgs>(_profilesService, "OnProfilesChangedEventHandler");
        profilesChanged
            .Subscribe(e =>
            {
                CanApply = _profilesService.IsUnsavedChanges();
            });
        var canApply = this
            .WhenAnyValue(x => x.CanApply)
            .DistinctUntilChanged();
        ApplyCommand = ReactiveCommand.Create(() =>
        {
            _profilesService.ApplyProfiles();
            CanApply = false;
        }, canApply);
        _currentProfileOperationsMediator.CurrentProfileChanged += OnProfileChanged;
        ProfileName = _currentProfileOperationsMediator.CurrentProfile.Name;
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

    private void OnProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        ProfileName = e.NewProfile.Name;
    }
}