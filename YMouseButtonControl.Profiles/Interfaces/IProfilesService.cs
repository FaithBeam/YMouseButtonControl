using System;
using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Interfaces;

public interface IProfilesService
{
    event EventHandler<ProfilesChangedEventArgs> OnProfilesChangedEventHandler;
    bool IsUnsavedChanges();
    IEnumerable<Profile> GetProfiles();
    void AddProfile(Profile profile);
    void ApplyProfiles();
}