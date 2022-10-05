﻿using System.Linq;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Models.SimulatedKeystrokesTypes;
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
                WindowClass = "N/A",
                MouseButton1 = new NothingMapping(),
                MouseButton2 = new NothingMapping(),
                MouseButton3 = new NothingMapping(),
                MouseButton4 = new SimulatedKeystrokes
                {
                    Keys = "w",
                    CanRaiseDialog = true,
                    SimulatedKeystrokesType = new StickyHoldActionType()
                },
                MouseButton4LastIndex = 2,
                MouseButton5 = new NothingMapping(),
                WheelUp = new NothingMapping(),
                WheelDown = new NothingMapping(),
                WheelLeft = new NothingMapping(),
                WheelRight = new NothingMapping()
            });
        }

        _profileOperationsMediator.CurrentProfile = repository.GetAll().First();
    }
}