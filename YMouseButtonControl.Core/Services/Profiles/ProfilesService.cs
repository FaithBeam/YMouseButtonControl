﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DynamicData;
using Newtonsoft.Json;
using ReactiveUI;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.Services.Profiles.Exceptions;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.Services.Profiles;

public interface IProfilesService
{
    SourceCache<ProfileVm, int> ProfilesSc { get; }
    ProfileVm? CurrentProfile { get; set; }
    ReadOnlyObservableCollection<ProfileVm> Profiles { get; }
    bool Dirty { get; set; }

    IObservable<IChangeSet<ProfileVm, int>> Connect();
    ProfileVm CopyProfile(ProfileVm p);
    void WriteProfileToFile(ProfileVm p, string path);
    void ImportProfileFromPath(string path);
    void AddProfile(ProfileVm profileVm);
    void ReplaceProfile(ProfileVm oldProfileVm, ProfileVm newProfileVm);
    void MoveProfileUp(ProfileVm p);
    void MoveProfileDown(ProfileVm p);
    void AddOrUpdate(ProfileVm profileVm);
}

public class ProfilesService : ReactiveObject, IProfilesService, IDisposable
{
    private readonly IRepository<Profile, ProfileVm> _profileRepository;
    private ProfileVm? _currentProfile;
    private readonly SourceCache<ProfileVm, int> _profilesSc;
    private readonly ReadOnlyObservableCollection<ProfileVm> _profilesObsCol;
    private bool _dirty;

    public ProfilesService(IRepository<Profile, ProfileVm> profileRepository)
    {
        _profileRepository = profileRepository;
        _profilesSc = new SourceCache<ProfileVm, int>(x => x.Id);
        _profilesSc
            .Connect()
            .AutoRefresh()
            .SortBy(x => x.DisplayPriority)
            .Bind(out _profilesObsCol)
            .Subscribe(IsDirty);
        _profilesSc.AddOrUpdate(profileRepository.GetAll().ToList());
        CurrentProfile ??= Profiles.FirstOrDefault();
    }

    private void IsDirty(IChangeSet<ProfileVm, int> set)
    {
        Dirty = !Profiles.SequenceEqual(_profileRepository.GetAll());
    }

    public bool Dirty
    {
        get => _dirty;
        set => this.RaiseAndSetIfChanged(ref _dirty, value);
    }

    /// <summary>
    /// Read only collection of profiles
    /// </summary>
    public ReadOnlyObservableCollection<ProfileVm> Profiles => _profilesObsCol;

    public void AddOrUpdate(ProfileVm profile) => _profilesSc.AddOrUpdate(profile);

    public IObservable<IChangeSet<ProfileVm, int>> Connect() => _profilesSc.Connect();

    public ProfileVm? CurrentProfile
    {
        get => _currentProfile;
        set => this.RaiseAndSetIfChanged(ref _currentProfile, value);
    }

    public SourceCache<ProfileVm, int> ProfilesSc => _profilesSc;

    public ProfileVm CopyProfile(ProfileVm p)
    {
        var jsonString = JsonConvert.SerializeObject(
            p,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
        );
        return JsonConvert.DeserializeObject<ProfileVm>(
                jsonString,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
            ) ?? throw new JsonSerializationException("Error deserializing profile");
    }

    public void WriteProfileToFile(ProfileVm p, string path)
    {
        var jsonString = JsonConvert.SerializeObject(
            p,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
            }
        );
        File.WriteAllText(path, jsonString);
    }

    public void ImportProfileFromPath(string path)
    {
        var f = File.ReadAllText(path);
        var deserializedProfile =
            JsonConvert.DeserializeObject<ProfileVm>(
                f,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
            ) ?? throw new JsonSerializationException("Error deserializing profile");
        AddProfile(deserializedProfile);
    }

    public void AddProfile(ProfileVm profile)
    {
        _profilesSc.AddOrUpdate(profile);
    }

    public void ReplaceProfile(ProfileVm oldProfile, ProfileVm newProfile)
    {
        if (oldProfile.IsDefault || newProfile.IsDefault)
        {
            throw new InvalidReplaceException("Cannot replace the default profile");
        }

        newProfile.Id = oldProfile.Id;
        _profilesSc.AddOrUpdate(newProfile);
    }

    public void MoveProfileUp(ProfileVm p)
    {
        if (p.IsDefault)
        {
            throw new InvalidMoveException("Cannot move the default profile");
        }

        _profilesSc.Edit(updater =>
        {
            var nextSmaller = _profilesSc
                .Items.Where(x => x.DisplayPriority < p.DisplayPriority)
                .MaxBy(x => x.DisplayPriority);
            if (nextSmaller is null)
            {
                throw new InvalidMoveException(
                    "Unable to retrieve max display index from profiles cache"
                );
            }

            if (nextSmaller.IsDefault)
            {
                throw new InvalidMoveException("Cannot move profile above default profile");
            }

            // swap priorities
            (nextSmaller.DisplayPriority, p.DisplayPriority) = (
                p.DisplayPriority,
                nextSmaller.DisplayPriority
            );
        });
    }

    public void MoveProfileDown(ProfileVm p)
    {
        if (p.IsDefault)
        {
            throw new InvalidMoveException("Cannot move the default profile");
        }

        var nextLargerPriority = _profilesSc
            .Items.Where(x => x.DisplayPriority > p.DisplayPriority)
            .MinBy(x => x.DisplayPriority);
        if (nextLargerPriority is null)
        {
            throw new InvalidMoveException(
                "Unable to retrieve max display index from profiles cache"
            );
        }

        // swap priorities
        (nextLargerPriority.DisplayPriority, p.DisplayPriority) = (
            p.DisplayPriority,
            nextLargerPriority.DisplayPriority
        );
    }

    public void Dispose()
    {
        _profilesSc.Dispose();
    }
}
