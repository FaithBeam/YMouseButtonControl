using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Collections;
using DynamicData;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.Profiles.Implementations;

public class ProfilesService : ReactiveObject, IProfilesService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private AvaloniaList<Profile> _profiles;
    private int _currentProfileIndex;
    private readonly ObservableAsPropertyHelper<Profile> _currentProfile;

    public ProfilesService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        CheckDefaultProfile();
        LoadProfilesFromDb();
        _currentProfile = this
            .WhenAnyValue(x => x.CurrentProfileIndex)
            .Select(x => _profiles[x])
            // .DistinctUntilChanged()
            .ToProperty(this, x => x.CurrentProfile);
    }

    public AvaloniaList<Profile> Profiles => _profiles;

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
        if (model.Any(x => x.Name == "Default")) return;
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

    public bool IsUnsavedChanges()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var dbProfiles = repository.GetAll().ToList();
        return _profiles.Any(inMemProfile => !dbProfiles.Contains(inMemProfile)) ||
               dbProfiles.Any(x => !_profiles.Contains(x));
    }
    
    public IEnumerable<Profile> GetProfiles()
    {
        return _profiles;
    }

    public void AddProfile(Profile profile)
    {
        _profiles.Add(profile);
    }

    public void ReplaceProfile(Profile oldProfile, Profile newProfile)
    {
        var pIndex = _profiles.IndexOf(oldProfile);
        _profiles.Replace(oldProfile, newProfile);
        // Trigger CurrentProfile to update
        CurrentProfileIndex = 0;
        CurrentProfileIndex = pIndex;
    }

    public void MoveProfileUp(Profile p)
    {
        var index = _profiles.IndexOf(p);
        if (index < 1) return;
        _profiles.Remove(p);
        _profiles.Insert(index - 1, p);
        CurrentProfileIndex--;
    }
    
    public void MoveProfileDown(Profile p)
    {
        var index = _profiles.IndexOf(p);
        if (index < 0) return;
        if (_profiles.Count < index + 2)
        {
            return;
        }

        _profiles.Remove(p);
        _profiles.Insert(index + 1, p);
        CurrentProfileIndex = 0;
        this.RaisePropertyChanged(nameof(CurrentProfileIndex));
        CurrentProfileIndex++;
    }

    public void RemoveProfile(Profile profile)
    {
        Profiles.Remove(profile);
        CurrentProfileIndex = 0;
    }

    public void ApplyProfiles()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        repository.ApplyAction(_profiles);
    }

    private void LoadProfilesFromDb()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var model = repository.GetAll();
        _profiles = new AvaloniaList<Profile>(model);
    }
}