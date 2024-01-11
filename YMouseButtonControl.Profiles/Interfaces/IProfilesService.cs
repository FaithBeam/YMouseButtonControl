using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;

namespace YMouseButtonControl.Profiles.Interfaces;

public interface IProfilesService
{
    IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing { get; }
    IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed { get; }
    IObservable<Exception> ThrownExceptions { get; }
    bool UnsavedChanges { get; set; }
    ObservableCollection<Profile> Profiles { get; }
    int CurrentProfileIndex { get; set; }
    Profile CurrentProfile { get; }
    IDisposable SuppressChangeNotifications();
    bool AreChangeNotificationsEnabled();
    IDisposable DelayChangeNotifications();
    event PropertyChangingEventHandler PropertyChanging;
    event PropertyChangedEventHandler PropertyChanged;
    Profile CopyProfile(Profile p);
    bool IsUnsavedChanges();
    void WriteProfileToFile(Profile p, string path);
    void ImportProfileFromPath(string path);
    IEnumerable<Profile> GetProfiles();
    void AddProfile(Profile profile);
    void ReplaceProfile(Profile oldProfile, Profile newProfile);
    void MoveProfileUp(Profile p);
    void MoveProfileDown(Profile p);
    void RemoveProfile(Profile profile);
    void ApplyProfiles();
}