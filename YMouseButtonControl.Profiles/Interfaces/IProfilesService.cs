using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models.Implementations;

namespace YMouseButtonControl.Profiles.Interfaces;

public interface IProfilesService
{
    public IEnumerable<Profile> GetProfiles();
    public void AddProfile(Profile profile);
}