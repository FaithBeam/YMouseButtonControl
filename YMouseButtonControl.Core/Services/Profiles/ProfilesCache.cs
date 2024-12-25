using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Infrastructure.Context;

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

    public ProfilesCache(YMouseButtonControlDbContext db)
    {
        _profilesSc = new SourceCache<ProfileVm, int>(x => x.Id);
        _profilesSc.Connect().AutoRefresh().Bind(out _profilesObsCol).Subscribe();
        _profilesSc.AddOrUpdate(
            db.Profiles.AsNoTracking()
                .Include(x => x.ButtonMappings)
                .ToList()
                .Select(ProfileMapper.Map)
        );
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
