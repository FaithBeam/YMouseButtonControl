using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class ProfilesInformationViewModel(IProfilesService profilesService)
    : ViewModelBase,
        IProfilesInformationViewModel
{
    public IProfilesService ProfilesService { get; } = profilesService;
}
