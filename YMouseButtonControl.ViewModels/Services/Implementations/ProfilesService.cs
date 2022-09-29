using System;
using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Services.Implementations;

public class ProfilesService : IProfilesService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private Profile _currentProfile;
    
    public Profile CurrentProfile
    {
        get => _currentProfile;
        set
        {
            if (_currentProfile == value)
            {
                return;
            }

            _currentProfile = value;

            var args = new SelectedProfileChangedEventArgs(_currentProfile);
            OnProfileChanged(args);
        }
    }
    
    public event EventHandler<SelectedProfileChangedEventArgs> SelectedProfileChanged;

    public void OnProfileChanged(SelectedProfileChangedEventArgs e)
    {
        var handler = SelectedProfileChanged;
        handler?.Invoke(this, e);
    }

    public ProfilesService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public IEnumerable<Profile> GetProfiles()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var model = repository.GetAll();
        return model;
    }
}