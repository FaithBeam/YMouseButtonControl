using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.Repositories;

public class ThemeRepository(YMouseButtonControlDbContext ctx) : IRepository<Theme, ThemeVm>
{
    public int Add(ThemeVm vm)
    {
        var ent = ThemeMapper.Map(vm);
        if (ent is not null)
        {
            ctx.Themes.Add(ent);
            ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }

    public int Delete(ThemeVm vm)
    {
        var ent = ctx.Themes.Find(vm.Id);
        if (ent is not null)
        {
            ctx.Themes.Remove(ent);
            ctx.SaveChanges();
        }

        return ent?.Id ?? -1;
    }

    public IEnumerable<ThemeVm> GetAll()
    {
        return ctx.Themes.Select(ThemeMapper.Map);
    }

    public ThemeVm? GetById(int id)
    {
        return ThemeMapper.Map(ctx.Themes.Find(id));
    }

    public ThemeVm? GetByName(string name)
    {
        return ThemeMapper.Map(ctx.Themes.Find(name));
    }

    public int Update(ThemeVm vm)
    {
        var ent = ctx.Themes.Find(vm.Id);
        if (ent is not null)
        {
            ent.Background = vm.Background;
            ent.Highlight = vm.Highlight;
            ent.Name = vm.Name;
            ctx.SaveChanges();
        }
        return ent?.Id ?? -1;
    }
}
