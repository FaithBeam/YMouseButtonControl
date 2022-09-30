using System;
using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.ViewModels.Services.Interfaces;

public interface IProfilesService
{
    public IEnumerable<Profile> GetProfiles();
}