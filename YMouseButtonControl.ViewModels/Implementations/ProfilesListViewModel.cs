using Avalonia.Collections;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesListViewModel : ViewModelBase, IProfilesListViewModel
{
    private IProfilesService _profilesService;
    private IProfileOperationsMediator _profileOperationsMediator;

    public ProfilesListViewModel(IProfilesService profilesService, IProfileOperationsMediator profileOperationsMediator)
    {
        ProfilesService = profilesService;
        _profileOperationsMediator = profileOperationsMediator;
    }

    public IProfilesService ProfilesService
    {
        get => _profilesService;
        set => _profilesService = value;
    }

    public AvaloniaList<Profile> Profiles => new(_profilesService.GetProfiles());

    public Profile SelectedProfile
    {
        get => _profileOperationsMediator.CurrentProfile;
        set => _profileOperationsMediator.CurrentProfile = value;
    }
}