using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.UnitOfWork;

namespace YMouseButtonControl.Core.Services;

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
}