using System.Linq;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;

public static class AddProfile
{
    public sealed record Command(ProfileVm Profile);

    public sealed class Handler(IProfilesCache profilesService)
    {
        public void Execute(Command c)
        {
            profilesService.ProfilesSc.Edit(inner =>
            {
                c.Profile.Id = inner.Items.Max(x => x.Id) + 1;
                c.Profile.DisplayPriority = inner.Items.Max(x => x.DisplayPriority) + 1;
                inner.AddOrUpdate(c.Profile);
            });
        }
    }
}
