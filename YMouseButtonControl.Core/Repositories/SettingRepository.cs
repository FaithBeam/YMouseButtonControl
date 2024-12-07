using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.Repositories;

public class SettingRepository(YMouseButtonControlDbContext ctx)
    : IRepository<Setting, BaseSettingVm>
{
    private readonly YMouseButtonControlDbContext _ctx = ctx;

    public int Add(BaseSettingVm vm)
    {
        var ent = SettingMapper.Map(vm);
        if (ent is not null)
        {
            _ctx.Add(ent);
            _ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }

    public BaseSettingVm? GetById(int id)
    {
        return SettingMapper.Map(_ctx.Settings.Find(id));
    }

    public IEnumerable<BaseSettingVm> GetAll()
    {
        return _ctx.Settings.Select(x => SettingMapper.Map(x));
    }

    public BaseSettingVm? GetByName(string name)
    {
        return SettingMapper.Map(_ctx.Settings.First(x => x.Name == name));
    }

    public int Update(BaseSettingVm vm)
    {
        var ent = _ctx.Settings.Find(vm.Id);
        if (ent is not null)
        {
            ent.Name = vm.Name;
            if (ent is SettingBool sb)
            {
                sb.BoolValue = ((SettingBoolVm)vm).BoolValue;
            }
            else if (ent is SettingInt si)
            {
                si.IntValue = ((SettingIntVm)vm).IntValue;
            }
            else if (ent is SettingString ss)
            {
                ss.StringValue = ((SettingStringVm)vm).StringValue;
            }
            _ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }

    public int Delete(BaseSettingVm vm)
    {
        var ent = _ctx.Settings.Find(vm.Id);
        if (ent is not null)
        {
            _ctx.Settings.Remove(ent);
            _ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }
}
