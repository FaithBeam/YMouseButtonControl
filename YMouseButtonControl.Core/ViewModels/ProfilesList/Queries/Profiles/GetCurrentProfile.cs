using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Queries.Profiles;

public static class GetCurrentProfile
{
    public sealed class Handler(IProfilesService profilesService)
    {
        public ProfilesListProfileModel? Execute()
        {
            var vm = profilesService.CurrentProfile;
            return vm is null ? null : new(vm, profilesService);
        }
    }
}
