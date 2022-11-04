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
    private readonly ObservableAsPropertyHelper<string> _profileName;

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
        _profilesService.OnProfilesChangedEventHandler += OnProfilesChanged;
        var canApply = this
            .WhenAnyValue(x => x.CanApply)
            .DistinctUntilChanged();
        ApplyCommand = ReactiveCommand.Create(() =>
        {
            _profilesService.ApplyProfiles();
            CanApply = false;
        }, canApply);
        var profileChanged =
            Observable.FromEventPattern<SelectedProfileChangedEventArgs>(_currentProfileOperationsMediator, "CurrentProfileChanged");
        profileChanged.Subscribe(e =>
        {
            CanApply = _profilesService.IsUnsavedChanges();
        });
        _profileName = profileChanged
            .Select(x => x.EventArgs.NewProfile.Name)
            .ToProperty(this, x => x.ProfileName);
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

    public string ProfileName => _profileName.Value;

    #endregion

    #region Methods
    
    private void OnCurrentProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        CanApply = _profilesService.IsUnsavedChanges();
    }
    
    private void OnProfilesChanged(object sender, ProfilesChangedEventArgs profilesChangedEventArgs)
    {
        CanApply = _profilesService.IsUnsavedChanges();
    }

    #endregion
}