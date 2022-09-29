using System.Linq;
using System.Threading.Tasks;
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

    private Profile _curProfile;
    private IProfilesService _profilesService;

    #endregion

    #region Constructor

    public MainWindowViewModel(IProfilesService profilesService, ILayerViewModel layerViewModel)
    {
        _profilesService = profilesService;
        LayerViewModel = layerViewModel;
        Profiles = new AvaloniaList<Profile>(_profilesService.GetProfiles());
        CurrentProfile = profilesService.CurrentProfile;
        profilesService.SelectedProfileChanged += OnProfileChanged;
    }

    #endregion

    #region Properties
    
    public ILayerViewModel LayerViewModel { get; }
    
    public ICommand ApplyCommand { get; }

    public AvaloniaList<Profile> Profiles { get; }

    public Profile CurrentProfile
    {
        get => _curProfile;
        set => _profilesService.CurrentProfile = value;
    }

    #endregion

    #region Methods

    private void OnProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        this.RaisePropertyChanged(nameof(CurrentProfile));
    }

    #endregion
}