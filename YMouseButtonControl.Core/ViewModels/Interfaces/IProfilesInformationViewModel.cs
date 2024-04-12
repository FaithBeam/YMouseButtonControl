using YMouseButtonControl.Core.Profiles.Interfaces;

namespace YMouseButtonControl.Core.ViewModels.Interfaces;

public interface IProfilesInformationViewModel
{
    public IProfilesService ProfilesService { get; }
}
