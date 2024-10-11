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

public interface IGenericRepository<TEntity, TVm>
{
    TVm? Add(TVm vm);
    Task<TVm?> AddAsync(TVm vm);
    IEnumerable<TVm> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = ""
    );

    IEnumerable<ProfileVm> GetAll();
    IQueryable<ProfileVm> GetAll(Expression<Func<TEntity, bool>> filter);
    Task<TVm?> GetAsync(Expression<Func<TEntity, bool>> filter);
    TVm? GetById(int id);
    Task<TVm?> GetByIdAsync(int id);
    TVm? Remove(int id);
    Task<TVm?> RemoveAsync(int id);
    TVm? Update(int id, TVm vm);
    Task<TVm?> UpdateAsync(int id, TVm vm);
}

public class ProfileRepository(YMouseButtonControlDbContext context)
    : IGenericRepository<Profile, ProfileVm>
{
    private readonly DbSet<Profile> _dbSet = context.Set<Profile>();

    public ProfileVm? Add(ProfileVm vm)
    {
        var entity = _dbSet.Add(ProfileMapper.Map(vm)).Entity;
        return ProfileMapper.Map(entity);
    }

    public async Task<ProfileVm?> AddAsync(ProfileVm vm)
    {
        var entity = (await _dbSet.AddAsync(ProfileMapper.Map(vm))).Entity;
        return ProfileMapper.Map(entity);
    }

    public IEnumerable<ProfileVm> Get(
        Expression<Func<Profile, bool>>? filter = null,
        Func<IQueryable<Profile>, IOrderedQueryable<Profile>>? orderBy = null,
        string includeProperties = ""
    )
    {
        IQueryable<Profile> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties
            .Split([','], StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return ProfileMapper.Map(orderBy != null ? orderBy(query) : query);
    }

    public IEnumerable<ProfileVm> GetAll()
    {
        return ProfileMapper.Map(_dbSet);
    }

    public IQueryable<ProfileVm> GetAll(Expression<Func<Profile, bool>> filter)
    {
        return ProfileMapper.Map(_dbSet.Where(filter));
    }

    public async Task<ProfileVm?> GetAsync(Expression<Func<Profile, bool>> filter)
    {
        return ProfileMapper.Map(await _dbSet.FirstOrDefaultAsync(filter));
    }

    public ProfileVm? GetById(int id) => ProfileMapper.Map(_dbSet.Find(id));

    public async Task<ProfileVm?> GetByIdAsync(int id) =>
        ProfileMapper.Map(await _dbSet.FindAsync(id));

    public ProfileVm? Remove(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        return ProfileMapper.Map(entity);
    }

    public async Task<ProfileVm?> RemoveAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        return ProfileMapper.Map(entity);
    }

    public ProfileVm? Update(int id, ProfileVm vm)
    {
        var entity = _dbSet.Find(id);
        if (entity is null)
        {
            return ProfileMapper.Map(_dbSet.Add(ProfileMapper.Map(vm)).Entity);
        }

        ProfileMapper.Map(vm, entity);
        return ProfileMapper.Map(entity);
    }

    public async Task<ProfileVm?> UpdateAsync(int id, ProfileVm vm)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null)
        {
            return ProfileMapper.Map(_dbSet.Add(ProfileMapper.Map(vm)).Entity);
        }

        ProfileMapper.Map(vm, entity);
        return ProfileMapper.Map(entity);
    }
}
