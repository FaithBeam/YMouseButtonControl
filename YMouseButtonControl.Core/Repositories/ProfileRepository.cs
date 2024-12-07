using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.Repositories;

public class ProfileRepository(YMouseButtonControlDbContext ctx) : IRepository<Profile, ProfileVm>
{
    private readonly YMouseButtonControlDbContext _ctx = ctx;

    public int Add(ProfileVm vm)
    {
        var ent = ProfileMapper.Map(vm);
        if (ent is not null)
        {
            _ctx.Profiles.Add(ent);
            _ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }

    public ProfileVm? GetById(int id)
    {
        var ent = _ctx.Profiles.Find(id);
        return ent is null ? null : ProfileMapper.Map(ent);
    }

    public IEnumerable<ProfileVm> GetAll()
    {
        return _ctx.Profiles.Include(x => x.ButtonMappings).Select(x => ProfileMapper.Map(x));
    }

    public int Update(ProfileVm vm)
    {
        var ent = _ctx.Profiles.Find(vm.Id);
        if (ent is not null)
        {
            ent.Checked = vm.Checked;
            ent.Description = vm.Description;
            ent.DisplayPriority = vm.DisplayPriority;
            ent.IsDefault = vm.IsDefault;
            ent.MatchType = vm.MatchType;
            ent.Name = vm.Name;
            ent.ParentClass = vm.ParentClass;
            ent.Process = vm.Process;
            ent.WindowCaption = vm.WindowCaption;
            ent.WindowClass = vm.WindowClass;
            _ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }

    public int Delete(ProfileVm vm)
    {
        var ent = _ctx.Profiles.Find(vm.Id);
        if (ent is not null)
        {
            _ctx.Profiles.Remove(ent);
            _ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }

    public ProfileVm? GetByName(string name)
    {
        return ProfileMapper.Map(_ctx.Profiles.Find(name));
    }
}
