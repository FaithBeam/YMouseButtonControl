using System;
using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Implementations;


public class ProfilesService : IProfilesService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private List<Profile> _profiles;
    
    public ProfilesService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        LoadProfilesFromDb();
    }

    public event EventHandler<ProfilesChangedEventArgs> OnProfilesChangedEventHandler; 

    public IEnumerable<Profile> GetProfiles()
    {
        return _profiles;
    }

    public void AddProfile(Profile profile)
    {
        _profiles.Add(profile);
        var e = new ProfilesChangedEventArgs(_profiles);
        OnProfilesChanged(e);
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
        _profiles = new List<Profile>(model);
    }

    private void OnProfilesChanged(ProfilesChangedEventArgs e)
    {
        var handler = OnProfilesChangedEventHandler;
        handler?.Invoke(this, e);
    }
}