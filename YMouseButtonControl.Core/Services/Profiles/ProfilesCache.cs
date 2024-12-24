using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using ReactiveUI;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.Services.Profiles;

public interface IProfilesCache
{
    SourceCache<ProfileVm, int> ProfilesSc { get; }
    ProfileVm? CurrentProfile { get; set; }
    ReadOnlyObservableCollection<ProfileVm> Profiles { get; }
    bool Dirty { get; set; }

    IObservable<IChangeSet<ProfileVm, int>> Connect();
}

public class ProfilesCache : ReactiveObject, IProfilesCache, IDisposable
{
    private readonly IRepository<Profile, ProfileVm> _profileRepository;
    private ProfileVm? _currentProfile;
    private readonly SourceCache<ProfileVm, int> _profilesSc;
    private readonly ReadOnlyObservableCollection<ProfileVm> _profilesObsCol;
    private bool _dirty;

    public ProfilesCache(IRepository<Profile, ProfileVm> profileRepository)
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

    public IObservable<IChangeSet<ProfileVm, int>> Connect() => _profilesSc.Connect();

    public ProfileVm? CurrentProfile
    {
        get => _currentProfile;
        set => this.RaiseAndSetIfChanged(ref _currentProfile, value);
    }

    public SourceCache<ProfileVm, int> ProfilesSc => _profilesSc;

    public void Dispose()
    {
        _profilesSc.Dispose();
    }
}
