using System.Linq;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Services.Implementations;

public class CheckDefaultProfileService : ICheckDefaultProfileService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IProfilesService _profilesService;

    public CheckDefaultProfileService(IUnitOfWorkFactory unitOfWorkFactory, IProfilesService profilesService)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _profilesService = profilesService;
    }

    public void CheckDefaultProfile()
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

        _profilesService.CurrentProfileIndex = 0;
    }
}