using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesInformationViewModel : ViewModelBase, IProfilesInformationViewModel
{
    public ProfilesInformationViewModel(IProfilesService profilesService)
    {
        ProfilesService = profilesService;
    }

    public IProfilesService ProfilesService { get; }
}