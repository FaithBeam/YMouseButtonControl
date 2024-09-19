using System.Linq;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Features.Add;

public interface IAddProfile
{
    void Add(ProfileVm profile);
}

public class AddProfile(IProfilesService profilesService) : IAddProfile
{
    public void Add(ProfileVm profile)
    {
        profile.Id = GetNextProfileId();
        profile.DisplayPriority = GetNextProfileDisplayPriority();
        profilesService.AddOrUpdate(profile);
    }

    private int GetNextProfileDisplayPriority() =>
        profilesService.Profiles.Max(x => x.DisplayPriority) + 1;

    private int GetNextProfileId() => profilesService.Profiles.Max(x => x.Id) + 1;
}
