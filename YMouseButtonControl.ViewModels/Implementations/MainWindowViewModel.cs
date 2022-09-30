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

    #endregion

    #region Constructor

    public MainWindowViewModel(IProfilesService profilesService, IProfileOperationsMediator profileOperationsMediator,
        ILayerViewModel layerViewModel)
    {
        _profilesService = profilesService;
        ProfileOperationsMediator = profileOperationsMediator;
        LayerViewModel = layerViewModel;
        Profiles = new AvaloniaList<Profile>(_profilesService.GetProfiles());
    }

    #endregion

    #region Properties

    public IProfileOperationsMediator ProfileOperationsMediator { get; }
    
    public ILayerViewModel LayerViewModel { get; }

    public ICommand ApplyCommand { get; }

    public AvaloniaList<Profile> Profiles { get; }

    #endregion

    #region Methods

    #endregion
}