using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.Services;

public interface IProfilesService
{
    public IEnumerable<Profile> GetProfiles();
}