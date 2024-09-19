using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProfilesInformationViewModel;

public interface IProfilesInformationViewModel
{
    public IProfilesService ProfilesService { get; }
}

public class ProfilesInformationViewModel(IProfilesService profilesService)
    : ViewModelBase,
        IProfilesInformationViewModel
{
    public IProfilesService ProfilesService { get; } = profilesService;
}
