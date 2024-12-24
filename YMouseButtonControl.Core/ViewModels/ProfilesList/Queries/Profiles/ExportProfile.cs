using System.Linq;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Queries.Profiles;

public static class ExportProfile
{
    public sealed record Query(int Id);

    public sealed class Handler(IProfilesService profilesService)
    {
        public ProfilesListProfileModel Execute(Query q) =>
            new(profilesService.Profiles.First(x => x.Id == q.Id).Clone(), profilesService);
    }
}
