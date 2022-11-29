using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.Profiles.Implementations;

public class ProfilesService : ReactiveObject, IProfilesService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private ObservableCollection<Profile> _profiles;
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
            .ToProperty(this, x => x.CurrentProfile);
    }

    public ObservableCollection<Profile> Profiles => _profiles;

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
        if (model.All(x => x.Name != "Default"))
        {
            repository.Add(new Profile
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
            });
        }
    }

    public bool IsUnsavedChanges()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var dbProfiles = repository.GetAll().ToList();
        return _profiles.Any(inMemProfile => !dbProfiles.Contains(inMemProfile));
    }
    
    public IEnumerable<Profile> GetProfiles()
    {
        return _profiles;
    }

    public void AddProfile(Profile profile)
    {
        _profiles.Add(profile);
    }

    public void RemoveProfile(Profile profile)
    {
        _profiles.Remove(profile);
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
        _profiles = new ObservableCollection<Profile>(model);
    }
}