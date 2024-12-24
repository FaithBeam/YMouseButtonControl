using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;

public static class RemoveProfile
{
    public sealed record Command(int Id);

    public sealed class Handler(IProfilesService profilesService)
    {
        public void Execute(Command c) =>
            profilesService.ProfilesSc.Edit(inner => inner.RemoveKey(c.Id));
    }
}
