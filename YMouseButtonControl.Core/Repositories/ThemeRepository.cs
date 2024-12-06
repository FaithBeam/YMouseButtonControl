using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Queries;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.Repositories;

public class ThemeRepository(YMouseButtonControlDbContext ctx, ThemeQueries queries)
    : IRepository<Theme, ThemeVm>
{
    private const string TblName = "Themes";

    public int Add(ThemeVm vm)
    {
        using var conn = ctx.CreateConnection();
        return conn.Query<int>(queries.Add(), ThemeMapper.Map(vm)).Single();
    }

    public Task<int> AddAsync(ThemeVm vm)
    {
        throw new NotImplementedException();
    }

    public int Delete(ThemeVm vm)
    {
        using var conn = ctx.CreateConnection();
        return conn.Execute(queries.DeleteById(TblName), new { ThemeMapper.Map(vm).Id });
    }

    public Task<int> DeleteAsync(ThemeVm vm)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ThemeVm> GetAll()
    {
        using var conn = ctx.CreateConnection();
        return conn.Query<Theme>(queries.GetAll(TblName)).Select(ThemeMapper.Map);
    }

    public Task<IEnumerable<ThemeVm>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public ThemeVm? GetById(int id)
    {
        using var conn = ctx.CreateConnection();
        return ThemeMapper.Map(
            conn.QueryFirstOrDefault<Theme>(queries.GetById(TblName), new { Id = id })
        );
    }

    public Task<ThemeVm?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public ThemeVm? GetByName(string name)
    {
        using var conn = ctx.CreateConnection();
        return ThemeMapper.Map(
            conn.QuerySingleOrDefault<Theme>(queries.GetByName(TblName), new { Name = name })
        );
    }

    public int Update(ThemeVm vm)
    {
        using var conn = ctx.CreateConnection();
        return conn.Execute(queries.Update(), ThemeMapper.Map(vm));
    }

    public Task<int> UpdateAsync(ThemeVm vm)
    {
        throw new NotImplementedException();
    }
}
