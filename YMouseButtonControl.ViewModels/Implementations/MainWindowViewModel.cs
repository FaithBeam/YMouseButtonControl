using System.Windows.Input;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    #region Fields

    private IProfilesService _profilesService;
    private ICurrentProfileOperationsMediator _currentProfileOperationsMediator;
    private IProfilesListViewModel _profilesListViewModel;

    #endregion

    #region Constructor

    public MainWindowViewModel(IProfilesService profilesService, ICurrentProfileOperationsMediator currentProfileOperationsMediator,
        ILayerViewModel layerViewModel, IProfilesListViewModel profilesListViewModel, IProfilesInformationViewModel profilesInformationViewModel)
    {
        _profilesListViewModel = profilesListViewModel;
        _profilesService = profilesService;
        CurrentProfileOperationsMediator = currentProfileOperationsMediator;
        LayerViewModel = layerViewModel;
        ProfilesInformationViewModel = profilesInformationViewModel;
    }

    #endregion

    #region Properties

    public IProfilesInformationViewModel ProfilesInformationViewModel { get; }
    
    public IProfilesListViewModel ProfilesListViewModel => _profilesListViewModel;

    public ICurrentProfileOperationsMediator CurrentProfileOperationsMediator { get; }
    
    public ILayerViewModel LayerViewModel { get; }

    public ICommand ApplyCommand { get; }

    #endregion

    #region Methods

    #endregion
}