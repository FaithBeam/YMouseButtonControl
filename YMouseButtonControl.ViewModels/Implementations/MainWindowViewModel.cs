using System.Windows.Input;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    #region Fields

    private IProfilesService _profilesService;
    private IProfileOperationsMediator _profileOperationsMediator;
    private IProfilesListViewModel _profilesListViewModel;

    #endregion

    #region Constructor

    public MainWindowViewModel(IProfilesService profilesService, IProfileOperationsMediator profileOperationsMediator,
        ILayerViewModel layerViewModel, IProfilesListViewModel profilesListViewModel)
    {
        _profilesListViewModel = profilesListViewModel;
        _profilesService = profilesService;
        ProfileOperationsMediator = profileOperationsMediator;
        LayerViewModel = layerViewModel;
    }

    #endregion

    #region Properties

    public IProfilesListViewModel ProfilesListViewModel => _profilesListViewModel;

    public IProfileOperationsMediator ProfileOperationsMediator { get; }
    
    public ILayerViewModel LayerViewModel { get; }

    public ICommand ApplyCommand { get; }

    #endregion

    #region Methods

    #endregion
}