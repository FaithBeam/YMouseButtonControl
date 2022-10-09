using System.Linq;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Services.Implementations;

public class CheckDefaultProfileService : ICheckDefaultProfileService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly ICurrentProfileOperationsMediator _currentProfileOperationsMediator;

    public CheckDefaultProfileService(IUnitOfWorkFactory unitOfWorkFactory, ICurrentProfileOperationsMediator currentProfileOperationsMediator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _currentProfileOperationsMediator = currentProfileOperationsMediator;
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
                Process = "Some process",
                MatchType = "N/A",
                ParentClass = "N/A",
                WindowCaption = "N/A",
                WindowClass = "N/A",
                MouseButton1 = new NothingMapping(),
                MouseButton2 = new NothingMapping(),
                MouseButton3 = new SimulatedKeystrokes
                {
                    Keys = "{shift}W",
                    CanRaiseDialog = true,
                    SimulatedKeystrokesType = new StickyHoldActionType()
                },
                MouseButton3LastIndex = 2,
                MouseButton4 = new NothingMapping(),
                MouseButton5 = new NothingMapping(),
                WheelUp = new NothingMapping(),
                WheelDown = new NothingMapping(),
                WheelLeft = new NothingMapping(),
                WheelRight = new NothingMapping()
            });
        }

        _currentProfileOperationsMediator.CurrentProfile = repository.GetAll().First();
    }
}