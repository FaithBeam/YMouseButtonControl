using System;
using DynamicData;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;

namespace YMouseButtonControl.Core.Profiles.Implementations;

public interface ISettingsService
{
    IObservable<IChangeSet<Setting, int>> Connect();
    bool IsUnsavedChanges();
    Setting? GetSetting(string name);
}
