using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProfilesInformationViewModel;

public interface IProfilesInformationViewModel
{
    public IProfilesCache ProfilesService { get; }
}

public class ProfilesInformationViewModel(IProfilesCache profilesService)
    : ViewModelBase,
        IProfilesInformationViewModel
{
    public IProfilesCache ProfilesService { get; } = profilesService;
}
