using System.Linq;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Services.Implementations;

public class CheckDefaultProfileService : ICheckDefaultProfileService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IProfileOperationsMediator _profileOperationsMediator;

    public CheckDefaultProfileService(IUnitOfWorkFactory unitOfWorkFactory, IProfileOperationsMediator profileOperationsMediator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _profileOperationsMediator = profileOperationsMediator;
    }

    public void CheckDefaultProfile()
    {
        using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        var model = repository.GetAll();
        if (model.All(x => x.Name != "Default"))
        {
            repository.Add("0", new Profile
            {
                Checked = true,
                Name = "Default",
                Description = "Default description",
                Process = "Some process",
                MatchType = "N/A",
                ParentClass = "N/A",
                WindowCaption = "N/A",
                WindowClass = "N/A"
            });
        }

        _profileOperationsMediator.CurrentProfile = repository.GetAll().First();
    }
}