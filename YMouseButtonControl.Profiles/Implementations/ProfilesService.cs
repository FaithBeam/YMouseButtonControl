using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.Profiles.Implementations;


public class ProfilesService : IProfilesService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    
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

    public void AddProfile(Profile profile)
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        repository.Add(profile);
    }
}