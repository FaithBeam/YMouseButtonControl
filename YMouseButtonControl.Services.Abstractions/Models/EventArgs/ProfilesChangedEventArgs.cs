using YMouseButtonControl.DataAccess.Models.Implementations;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class ProfilesChangedEventArgs : System.EventArgs
{
    public IEnumerable<Profile> Profiles { get; }

    public ProfilesChangedEventArgs(IEnumerable<Profile> profiles)
    {
        Profiles = profiles;
    }
}