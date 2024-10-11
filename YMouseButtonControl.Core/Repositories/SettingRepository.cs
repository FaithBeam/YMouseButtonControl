using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Context;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.Repositories;

public class SettingRepository(YMouseButtonControlDbContext context)
    : IGenericRepository<Setting, BaseSettingVm>
{
    private readonly DbSet<Setting> _dbSet = context.Set<Setting>();

    public BaseSettingVm? Add(BaseSettingVm vm)
    {
        var entity = _dbSet.Add(SettingMapper.Map(vm)).Entity;
        return SettingMapper.Map(entity);
    }

    public async Task<BaseSettingVm?> AddAsync(BaseSettingVm vm)
    {
        var entity = (await _dbSet.AddAsync(SettingMapper.Map(vm))).Entity;
        return SettingMapper.Map(entity);
    }

    public IEnumerable<BaseSettingVm> Get(
        Expression<Func<Setting, bool>>? filter = null,
        Func<IQueryable<Setting>, IOrderedQueryable<Setting>>? orderBy = null,
        string includeProperties = ""
    )
    {
        IQueryable<Setting> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties
            .Split([','], StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return orderBy != null
            ? orderBy(query).Select(x => SettingMapper.Map(x))
            : query.Select(x => SettingMapper.Map(x));
    }

    public IEnumerable<BaseSettingVm> GetAll() => _dbSet.Select(x => SettingMapper.Map(x));

    public IEnumerable<BaseSettingVm> GetAll(Expression<Func<Setting, bool>> filter) =>
        _dbSet.Where(filter).Select(x => SettingMapper.Map(x));

    public async Task<BaseSettingVm?> GetAsync(Expression<Func<Setting, bool>> filter) =>
        SettingMapper.Map(await _dbSet.FirstOrDefaultAsync(filter));

    public BaseSettingVm? GetById(int id) => SettingMapper.Map(_dbSet.Find(id));

    public async Task<BaseSettingVm?> GetByIdAsync(int id) =>
        SettingMapper.Map(await _dbSet.FindAsync(id));

    public BaseSettingVm? Remove(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        return SettingMapper.Map(entity);
    }

    public async Task<BaseSettingVm?> RemoveAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        return SettingMapper.Map(entity);
    }

    public BaseSettingVm? Update(int id, BaseSettingVm vm)
    {
        var entity = _dbSet.Find(id);
        if (entity is null)
        {
            return SettingMapper.Map(_dbSet.Add(SettingMapper.Map(vm)).Entity);
        }

        SettingMapper.Map(vm, entity);
        return SettingMapper.Map(entity);
    }

    public async Task<BaseSettingVm?> UpdateAsync(int id, BaseSettingVm vm)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null)
        {
            return SettingMapper.Map(_dbSet.Add(SettingMapper.Map(vm)).Entity);
        }

        SettingMapper.Map(vm, entity);
        return SettingMapper.Map(entity);
    }
}
