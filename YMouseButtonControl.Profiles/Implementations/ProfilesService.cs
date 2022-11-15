using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

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
        LoadProfilesFromDb();
        _currentProfile = this
            .WhenAnyValue(x => x.CurrentProfileIndex)
            .Select(x => _profiles[x])
            .ToProperty(this, x => x.CurrentProfile);
    }

    public AvaloniaList<Profile> Profiles => _profiles;

    public int CurrentProfileIndex
    {
        get => _currentProfileIndex;
        set => this.RaiseAndSetIfChanged(ref _currentProfileIndex, value);
    }

    public Profile CurrentProfile => _currentProfile.Value;
    
    public void UpdateCurrentMouse(IButtonMapping value, MouseButton button)
    {
        switch (button)
        {
            case MouseButton.MouseButton1:
                CurrentProfile.MouseButton1 = value;
                CurrentProfile.MouseButton1LastIndex = value.Index;
                break;
            case MouseButton.MouseButton2:
                CurrentProfile.MouseButton2 = value;
                CurrentProfile.MouseButton2LastIndex = value.Index;
                break;
            case MouseButton.MouseButton3:
                CurrentProfile.MouseButton3 = value;
                CurrentProfile.MouseButton3LastIndex = value.Index;
                break;
            case MouseButton.MouseButton4:
                CurrentProfile.MouseButton4 = value;
                CurrentProfile.MouseButton4LastIndex = value.Index;
                break;
            case MouseButton.MouseButton5:
                CurrentProfile.MouseButton5 = value;
                CurrentProfile.MouseButton5LastIndex = value.Index;
                break;
            case MouseButton.MouseWheelUp:
                CurrentProfile.MouseWheelUp = value;
                CurrentProfile.MouseWheelUpLastIndex = value.Index;
                break;
            case MouseButton.MouseWheelDown:
                CurrentProfile.MouseWheelDown = value;
                CurrentProfile.MouseWheelDownLastIndex = value.Index;
                break;
            case MouseButton.MouseWheelLeft:
                CurrentProfile.MouseWheelLeft = value;
                CurrentProfile.MouseWheelLeftLastIndex = value.Index;
                break;
            case MouseButton.MouseWheelRight:
                CurrentProfile.MouseWheelRight = value;
                CurrentProfile.MouseWheelRightLastIndex = value.Index;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
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