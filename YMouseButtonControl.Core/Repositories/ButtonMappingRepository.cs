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

public class ButtonMappingRepository(YMouseButtonControlDbContext context)
    : IGenericRepository<ButtonMapping, BaseButtonMappingVm>
{
    private readonly DbSet<ButtonMapping> _dbSet = context.Set<ButtonMapping>();

    public BaseButtonMappingVm? Add(BaseButtonMappingVm vm)
    {
        var entity = _dbSet.Add(ButtonMappingMapper.Map(vm)).Entity;
        return ButtonMappingMapper.Map(entity);
    }

    public async Task<BaseButtonMappingVm?> AddAsync(BaseButtonMappingVm vm)
    {
        var entity = (await _dbSet.AddAsync(ButtonMappingMapper.Map(vm))).Entity;
        return ButtonMappingMapper.Map(entity);
    }

    public IEnumerable<BaseButtonMappingVm> Get(
        Expression<Func<ButtonMapping, bool>>? filter = null,
        Func<IQueryable<ButtonMapping>, IOrderedQueryable<ButtonMapping>>? orderBy = null,
        string includeProperties = ""
    )
    {
        IQueryable<ButtonMapping> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties
            .Split([','], StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return ButtonMappingMapper.Map(orderBy != null ? orderBy(query).ToList() : query.ToList());
    }

    public IEnumerable<BaseButtonMappingVm> GetAll() => ButtonMappingMapper.Map(_dbSet);

    public IEnumerable<BaseButtonMappingVm> GetAll(Expression<Func<ButtonMapping, bool>> filter) =>
        ButtonMappingMapper.Map(_dbSet.Where(filter).ToList());

    public async Task<BaseButtonMappingVm?> GetAsync(
        Expression<Func<ButtonMapping, bool>> filter
    ) => ButtonMappingMapper.Map(await _dbSet.FirstOrDefaultAsync(filter));

    public BaseButtonMappingVm? GetById(int id) => ButtonMappingMapper.Map(_dbSet.Find(id));

    public async Task<BaseButtonMappingVm?> GetByIdAsync(int id) =>
        ButtonMappingMapper.Map(await _dbSet.FindAsync(id));

    public BaseButtonMappingVm? Remove(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        return ButtonMappingMapper.Map(entity);
    }

    public async Task<BaseButtonMappingVm?> RemoveAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        return ButtonMappingMapper.Map(entity);
    }

    public BaseButtonMappingVm? Update(int id, BaseButtonMappingVm vm)
    {
        var entity = _dbSet.Find(id);
        if (entity is null)
        {
            return ButtonMappingMapper.Map(_dbSet.Add(ButtonMappingMapper.Map(vm)).Entity);
        }

        ButtonMappingMapper.Map(vm, entity);
        return ButtonMappingMapper.Map(entity);
    }

    public async Task<BaseButtonMappingVm?> UpdateAsync(int id, BaseButtonMappingVm vm)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null)
        {
            return ButtonMappingMapper.Map(_dbSet.Add(ButtonMappingMapper.Map(vm)).Entity);
        }

        ButtonMappingMapper.Map(vm, entity);
        return ButtonMappingMapper.Map(entity);
    }
}
