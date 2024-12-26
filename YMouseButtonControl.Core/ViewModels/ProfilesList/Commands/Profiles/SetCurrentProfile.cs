using System.Linq;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;

/// <summary>
/// Assign ProfilesService CurrentProfile to a new profile
/// </summary>
public static class SetCurrentProfile
{
    public sealed record Command(ProfilesListProfileModel? Profile);

    public sealed class Handler(IProfilesCache profilesService)
    {
        public void Execute(Command c)
        {
            if (c.Profile is null)
            {
                profilesService.CurrentProfile = null;
            }
            else
            {
                var profile = profilesService.Profiles.First(x => x.Id == c.Profile.Id);
                profilesService.CurrentProfile = profile;
            }
        }
    }
}
