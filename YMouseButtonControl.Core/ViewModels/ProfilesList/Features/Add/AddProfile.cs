using System.Linq;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.Profiles.Interfaces;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Features.Add;

public interface IAddProfile
{
    void Add(Profile profile);
}

public class AddProfile(IProfilesService profilesService) : IAddProfile
{
    public void Add(Profile profile)
    {
        profile.Id = GetNextProfileId();
        profile.DisplayPriority = GetNextProfileDisplayPriority();
        profilesService.AddOrUpdate(profile);
    }

    private int GetNextProfileDisplayPriority() =>
        profilesService.Profiles.Max(x => x.DisplayPriority) + 1;

    private int GetNextProfileId() => profilesService.Profiles.Max(x => x.Id) + 1;
}
