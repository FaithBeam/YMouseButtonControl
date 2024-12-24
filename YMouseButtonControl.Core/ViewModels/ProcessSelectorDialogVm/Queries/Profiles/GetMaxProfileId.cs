using System.Linq;
using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Profiles;

public static class GetMaxProfileId
{
    public sealed class Handler(IProfilesService profilesService)
    {
        public int Execute() =>
            profilesService.Profiles.SelectMany(x => x.ButtonMappings).Max(x => x.Id);
    }
}
