﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Newtonsoft.Json;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.UnitOfWork;
using YMouseButtonControl.Core.Profiles.Exceptions;
using YMouseButtonControl.Core.Profiles.Interfaces;

namespace YMouseButtonControl.Core.Profiles.Implementations;

public class ProfilesService : ReactiveObject, IProfilesService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private int _currentProfileIndex;
    private readonly ObservableAsPropertyHelper<Profile> _currentProfile;
    private bool _unsavedChanges;

    public ProfilesService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        Profiles = new ObservableCollection<Profile>();
        _unitOfWorkFactory = unitOfWorkFactory;
        CheckDefaultProfile();
        LoadProfilesFromDb();
        _currentProfile = this.WhenAnyValue(x => x.CurrentProfileIndex)
            .Select(x => Profiles[x])
            .DistinctUntilChanged()
            .ToProperty(this, x => x.CurrentProfile);

        var unsavedChanges = Profiles
            .ToObservableChangeSet()
            .AutoRefresh()
            .Select(_ => IsUnsavedChanges())
            .Subscribe(UnsavedChangesHelper);
    }

    public bool UnsavedChanges
    {
        get => _unsavedChanges;
        set => this.RaiseAndSetIfChanged(ref _unsavedChanges, value);
    }

    public ObservableCollection<Profile> Profiles { get; private set; }

    public int CurrentProfileIndex
    {
        get => _currentProfileIndex;
        set => this.RaiseAndSetIfChanged(ref _currentProfileIndex, value);
    }

    public Profile CurrentProfile => _currentProfile.Value;

    private void CheckDefaultProfile()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var model = repository.GetAll();
        if (model.Any(x => x.Name == "Default"))
            return;
        var defaultProfile = new Profile
        {
            Checked = true,
            Name = "Default",
            Description = "Default description",
            Process = "*",
            MatchType = "N/A",
            ParentClass = "N/A",
            WindowCaption = "N/A",
            WindowClass = "N/A",
            MouseButton1 = new NothingMapping(),
            MouseButton2 = new NothingMapping(),
            MouseButton3 = new NothingMapping(),
            MouseButton4 = new NothingMapping(),
            MouseButton5 = new NothingMapping(),
            MouseWheelUp = new NothingMapping(),
            MouseWheelDown = new NothingMapping(),
            MouseWheelLeft = new NothingMapping(),
            MouseWheelRight = new NothingMapping()
        };
        repository.Add(defaultProfile);
    }

    public Profile CopyProfile(Profile p)
    {
        var jsonString = JsonConvert.SerializeObject(
            p,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
        );
        return JsonConvert.DeserializeObject<Profile>(
                jsonString,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
            ) ?? throw new JsonSerializationException("Error deserializing profile");
    }

    public bool IsUnsavedChanges()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var dbProfiles = repository.GetAll().ToList();
        return Profiles.Count != dbProfiles.Count
            || Profiles.Where((p, i) => !p.Equals(dbProfiles[i])).Any();
    }

    public void WriteProfileToFile(Profile p, string path)
    {
        var jsonString = JsonConvert.SerializeObject(
            p,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
        );
        File.WriteAllText(path, jsonString);
    }

    public void ImportProfileFromPath(string path)
    {
        var f = File.ReadAllText(path);
        var deserializedProfile =
            JsonConvert.DeserializeObject<Profile>(
                f,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
            ) ?? throw new JsonSerializationException("Error deserializing profile");
        AddProfile(deserializedProfile);
    }

    public IEnumerable<Profile> GetProfiles()
    {
        return Profiles;
    }

    public void AddProfile(Profile profile)
    {
        profile.Id = GetNextProfileId();
        Profiles.Add(profile);
        CurrentProfileIndex = Profiles.IndexOf(profile);
    }

    public void ReplaceProfile(Profile oldProfile, Profile newProfile)
    {
        if (oldProfile.Id == 1)
        {
            throw new InvalidReplaceException("Cannot replace the default profile");
        }
        var pIndex = Profiles.IndexOf(oldProfile);
        Profiles.Replace(oldProfile, newProfile);
        // Trigger CurrentProfile to update
        CurrentProfileIndex = 0;
        CurrentProfileIndex = pIndex;
    }

    public void MoveProfileUp(Profile p)
    {
        if (p.Id == 1)
        {
            throw new InvalidMoveException("Cannot move the default profile");
        }
        var index = Profiles.IndexOf(p);
        switch (index)
        {
            case < 1:
                throw new InvalidMoveException($"Index is less than 1: {index}");
            case 1:
                throw new InvalidMoveException("Cannot move profile above default profile");
        }

        Profiles.Remove(p);
        Profiles.Insert(index - 1, p);
        CurrentProfileIndex = index - 1;
    }

    public void MoveProfileDown(Profile p)
    {
        if (p.Id == 1)
        {
            throw new InvalidMoveException("Cannot move the default profile");
        }
        var index = Profiles.IndexOf(p);
        if (index < 0)
        {
            throw new InvalidMoveException($"Index is less than 0: {index}");
        }
        if (Profiles.Count < index + 2)
        {
            return;
        }

        Profiles.Remove(p);
        Profiles.Insert(index + 1, p);
        CurrentProfileIndex = index + 1;
    }

    public void RemoveProfile(Profile profile)
    {
        if (profile.Name == "Default")
        {
            throw new Exception("Attempted to remove default profile");
        }
        CurrentProfileIndex--;
        Profiles.Remove(profile);
    }

    public void ApplyProfiles()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        repository.ApplyAction(Profiles);
    }

    private void LoadProfilesFromDb()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var model = repository.GetAll();
        Profiles = new ObservableCollection<Profile>(model);
    }

    private int GetNextProfileId()
    {
        return Profiles.Max(x => x.Id) + 1;
    }

    private void UnsavedChangesHelper(bool next)
    {
        UnsavedChanges = next;
    }
}