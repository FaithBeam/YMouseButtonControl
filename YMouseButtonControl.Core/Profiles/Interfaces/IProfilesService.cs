using System;
using System.Collections.ObjectModel;
using DynamicData;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;

namespace YMouseButtonControl.Core.Profiles.Interfaces;

public interface IProfilesService
{
    bool UnsavedChanges { get; set; }
    Profile? CurrentProfile { get; set; }
    ReadOnlyObservableCollection<Profile> Profiles { get; }
    IObservable<IChangeSet<Profile, int>> Connect();
    Profile CopyProfile(Profile p);
    bool IsUnsavedChanges();
    void WriteProfileToFile(Profile p, string path);
    void ImportProfileFromPath(string path);
    void AddProfile(Profile profile);
    void ReplaceProfile(Profile oldProfile, Profile newProfile);
    void MoveProfileUp(Profile p);
    void MoveProfileDown(Profile p);
    void RemoveProfile(Profile profile);
    void AddOrUpdate(Profile profile);
}
