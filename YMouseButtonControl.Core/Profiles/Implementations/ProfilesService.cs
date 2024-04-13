using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
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
    private Profile? _currentProfile;
    private bool _unsavedChanges;
    private readonly SourceCache<Profile, int> _profiles;
    private readonly ReadOnlyObservableCollection<Profile> _profilesObsCol;

    public ProfilesService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _profiles = new SourceCache<Profile, int>(x => x.Id);
        _profiles
            .Connect()
            .AutoRefresh()
            .SortBy(x => x.DisplayPriority)
            .Bind(out _profilesObsCol)
            .Subscribe();
        _unitOfWorkFactory = unitOfWorkFactory;
        CheckDefaultProfile();
        LoadProfilesFromDb();

        _profiles
            .Connect()
            .Subscribe(set =>
            {
                var change = set.Last();
                CurrentProfile = change.Reason switch
                {
                    ChangeReason.Add => change.Current,
                    ChangeReason.Remove
                        => _profiles
                            .Items.Where(x => x.DisplayPriority < change.Current.DisplayPriority)
                            .MaxBy(x => x.DisplayPriority)
                            ?? throw new Exception("Unable to get next lower priority on remove"),
                    ChangeReason.Update => change.Current,
                    _ => throw new Exception("Unhandled change reason")
                };
            });

        CurrentProfile =
            _profiles.Items.MinBy(x => x.DisplayPriority)
            ?? throw new Exception("Unable to retrieve current profile");

        var unsavedChanges = Connect()
            .AutoRefresh()
            .Select(_ => IsUnsavedChanges())
            .Subscribe(UnsavedChangesHelper);
    }

    /// <summary>
    /// Read only collection of profiles
    /// </summary>
    public ReadOnlyObservableCollection<Profile> Profiles => _profilesObsCol;

    public bool UnsavedChanges
    {
        get => _unsavedChanges;
        set => this.RaiseAndSetIfChanged(ref _unsavedChanges, value);
    }

    public IObservable<IChangeSet<Profile, int>> Connect() => _profiles.Connect();

    public Profile? CurrentProfile
    {
        get => _currentProfile;
        set => this.RaiseAndSetIfChanged(ref _currentProfile, value);
    }

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
            DisplayPriority = 0,
            Name = "Default",
            IsDefault = true,
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
        return _profiles.Items.Count() != dbProfiles.Count
            || _profiles.Items.Where((p, i) => !p.Equals(dbProfiles[i])).Any();
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

    public void AddProfile(Profile profile)
    {
        profile.Id = GetNextProfileId();
        profile.DisplayPriority = GetNextProfileDisplayPriority();
        _profiles.AddOrUpdate(profile);
    }

    public void ReplaceProfile(Profile oldProfile, Profile newProfile)
    {
        if (oldProfile.IsDefault || newProfile.IsDefault)
        {
            throw new InvalidReplaceException("Cannot replace the default profile");
        }

        newProfile.Id = oldProfile.Id;
        _profiles.AddOrUpdate(newProfile);
    }

    public void ReplaceProfile(Profile p)
    {
        if (p.IsDefault)
        {
            throw new InvalidReplaceException("Cannot replace the default profile");
        }

        _profiles.AddOrUpdate(p);
    }

    public void MoveProfileUp(Profile p)
    {
        if (p.IsDefault)
        {
            throw new InvalidMoveException("Cannot move the default profile");
        }

        var nextSmallerPriority = _profiles
            .Items.Where(x => x.DisplayPriority < p.DisplayPriority)
            .MaxBy(x => x.DisplayPriority);
        if (nextSmallerPriority is null)
        {
            throw new InvalidMoveException(
                "Unable to retrieve max display index from profiles cache"
            );
        }

        if (nextSmallerPriority.IsDefault)
        {
            throw new InvalidMoveException("Cannot move profile above default profile");
        }

        // swap priorities
        (nextSmallerPriority.DisplayPriority, p.DisplayPriority) = (
            p.DisplayPriority,
            nextSmallerPriority.DisplayPriority
        );
    }

    public void MoveProfileDown(Profile p)
    {
        if (p.IsDefault)
        {
            throw new InvalidMoveException("Cannot move the default profile");
        }

        var nextLargerPriority = _profiles
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

    public void RemoveProfile(Profile profile)
    {
        if (profile.Name == "Default")
        {
            throw new Exception("Attempted to remove default profile");
        }

        var nextSmallerPriority = _profiles
            .Items.Where(x => x.DisplayPriority < profile.DisplayPriority)
            .MaxBy(x => x.DisplayPriority);
        if (nextSmallerPriority is null)
        {
            throw new Exception("Unable to find next profile to set as current profile");
        }

        _profiles.Remove(profile);
        CurrentProfile = nextSmallerPriority;
    }

    public void ApplyProfiles()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        repository.ApplyAction(_profiles.Items);
    }

    private void LoadProfilesFromDb()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var model = repository.GetAll();
        _profiles.AddOrUpdate(model);
    }

    private int GetNextProfileId() => _profiles.Items.Max(x => x.Id) + 1;

    private int GetNextProfileDisplayPriority() => _profiles.Items.Max(x => x.DisplayPriority) + 1;

    private void UnsavedChangesHelper(bool next) => UnsavedChanges = next;
}
