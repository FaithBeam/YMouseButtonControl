using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models.Implementations;

namespace YMouseButtonControl.ViewModels.Services.Interfaces;

public interface IProfilesService
{
    public IEnumerable<Profile> GetProfiles();
    public void AddProfile(Profile profile);
}