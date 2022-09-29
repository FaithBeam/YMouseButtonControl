using System.Linq;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Services.Implementations;

public class CheckDefaultProfileService : ICheckDefaultProfileService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CheckDefaultProfileService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
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
                Description = string.Empty,
                Process = string.Empty,
                MatchType = string.Empty,
                ParentClass = string.Empty,
                WindowCaption = string.Empty,
                WindowClass = string.Empty
            });
        }
    }
}