using YMouseButtonControl.Core.Profiles.Interfaces;
using YMouseButtonControl.Core.ViewModels.Interfaces;

namespace YMouseButtonControl.Core.ViewModels.Implementations;

public class ProfilesInformationViewModel(IProfilesService profilesService)
    : ViewModelBase,
        IProfilesInformationViewModel
{
    public IProfilesService ProfilesService { get; } = profilesService;
}
