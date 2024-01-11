using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.ViewModels.Interfaces;

public interface IProfilesInformationViewModel
{
    public IProfilesService ProfilesService { get; }
}