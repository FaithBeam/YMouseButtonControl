using System.Collections.Generic;
using System.Data;
using System.Linq;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.Repositories;

public class ButtonMappingRepository(YMouseButtonControlDbContext ctx)
    : IRepository<ButtonMapping, BaseButtonMappingVm>
{
    private readonly YMouseButtonControlDbContext _ctx = ctx;

    public int Add(BaseButtonMappingVm vm)
    {
        var ent = ButtonMappingMapper.Map(vm);
        ent.ButtonMappingType = ent switch
        {
            DisabledMapping => ButtonMappingType.Disabled,
            NothingMapping => ButtonMappingType.Nothing,
            SimulatedKeystroke => ButtonMappingType.SimulatedKeystroke,
            RightClick => ButtonMappingType.RightClick,
            _ => throw new System.NotImplementedException(),
        };
        var id = _ctx.ButtonMappings.Add(ent).Entity.Id;
        _ctx.SaveChanges();
        return id;
    }

    public BaseButtonMappingVm? GetById(int id)
    {
        return ButtonMappingMapper.Map(_ctx.ButtonMappings.Find(id));
    }

    public IEnumerable<BaseButtonMappingVm> GetAll()
    {
        return _ctx.ButtonMappings.Select(ButtonMappingMapper.Map);
    }

    public int Update(BaseButtonMappingVm vm)
    {
        var ent = _ctx.ButtonMappings.Find(vm.Id);
        if (ent is not null)
        {
            ent.MouseButton = vm.MouseButton;
            ent.Keys = vm.Keys;
            ent.Selected = vm.Selected;
            ent.ProfileId = vm.ProfileId;
            ent.BlockOriginalMouseInput = vm.BlockOriginalMouseInput;
            ent.AutoRepeatDelay = vm.AutoRepeatDelay;
            ent.AutoRepeatRandomizeDelayEnabled = vm.AutoRepeatRandomizeDelayEnabled;
            ent.ButtonMappingType = ent.ButtonMappingType = ent switch
            {
                DisabledMapping => ButtonMappingType.Disabled,
                NothingMapping => ButtonMappingType.Nothing,
                SimulatedKeystroke => ButtonMappingType.SimulatedKeystroke,
                RightClick => ButtonMappingType.RightClick,
                _ => throw new System.NotImplementedException(),
            };
            _ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }

    public int Delete(BaseButtonMappingVm vm)
    {
        var ent = _ctx.ButtonMappings.Find(vm.Id);
        if (ent is not null)
        {
            _ctx.ButtonMappings.Remove(ent);
            _ctx.SaveChanges();
        }

        return ent?.Id ?? -1;
    }

    public BaseButtonMappingVm? GetByName(string name)
    {
        throw new System.NotImplementedException();
    }
}
