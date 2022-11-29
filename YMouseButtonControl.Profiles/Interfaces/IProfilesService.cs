using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;

namespace YMouseButtonControl.Profiles.Interfaces;

public interface IProfilesService
{
    IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing { get; }
    IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed { get; }
    IObservable<Exception> ThrownExceptions { get; }
    AvaloniaList<Profile> Profiles { get; }
    int CurrentProfileIndex { get; set; }
    Profile CurrentProfile { get; }
    IDisposable SuppressChangeNotifications();
    bool AreChangeNotificationsEnabled();
    IDisposable DelayChangeNotifications();
    event PropertyChangingEventHandler PropertyChanging;
    event PropertyChangedEventHandler PropertyChanged;
    // void UpdateCurrentMouse(IButtonMapping value, MouseButton button);
    bool IsUnsavedChanges();
    IEnumerable<Profile> GetProfiles();
    void AddProfile(Profile profile);
    void RemoveProfile(Profile profile);
    void ApplyProfiles();
}