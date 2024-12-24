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
    IObservable<IChangeSet<ProfileVm, int>> Connect();
}

public class ProfilesCache : ReactiveObject, IProfilesCache, IDisposable
{
    private ProfileVm? _currentProfile;
    private readonly SourceCache<ProfileVm, int> _profilesSc;
    private readonly ReadOnlyObservableCollection<ProfileVm> _profilesObsCol;

    public ProfilesCache(IRepository<Profile, ProfileVm> profileRepository)
    {
        _profilesSc = new SourceCache<ProfileVm, int>(x => x.Id);
        _profilesSc.Connect().AutoRefresh().Bind(out _profilesObsCol).Subscribe();
        _profilesSc.AddOrUpdate(profileRepository.GetAll().ToList());
        CurrentProfile ??= Profiles.FirstOrDefault();
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
