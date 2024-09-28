using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.DataAccess.Context;

namespace YMouseButtonControl.Core.Repositories;

public interface IGenericRepository<TEntity, TVm>
    where TEntity : class
    where TVm : class
{
    TVm? Add(TVm vm);
    Task<TVm?> AddAsync(TVm vm);
    IEnumerable<TVm> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = ""
    );
    Task<IEnumerable<TVm>> GetAllAsync();
    Task<IEnumerable<TVm>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
    Task<TVm?> GetAsync(Expression<Func<TEntity, bool>> filter);
    TVm? GetById(int id);
    Task<TVm?> GetByIdAsync(int id);
    TVm? Remove(int id);
    Task<TVm?> RemoveAsync(int id);
    TVm? Update(int id, TVm vm);
    Task<TVm?> UpdateAsync(int id, TVm vm);
}

public class GenericRepository<TEntity, TVm>(YMouseButtonControlDbContext context, IMapper mapper)
    : IGenericRepository<TEntity, TVm>
    where TEntity : class
    where TVm : class
{
    private readonly YMouseButtonControlDbContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private readonly IMapper _mapper = mapper;

    public virtual IEnumerable<TVm> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = ""
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        // query = query.AsNoTracking();

        return _mapper.Map<IEnumerable<TVm>>(
            orderBy != null ? orderBy(query).ToList() : query.ToList()
        );
    }

    public async Task<IEnumerable<TVm>> GetAllAsync() =>
        _mapper.Map<IEnumerable<TVm>>(await _dbSet.ToListAsync());

    public async Task<IEnumerable<TVm>> GetAllAsync(Expression<Func<TEntity, bool>> filter) =>
        _mapper.Map<IEnumerable<TVm>>(await _dbSet.Where(filter).ToListAsync());

    public async Task<TVm?> GetAsync(Expression<Func<TEntity, bool>> filter) =>
        _mapper.Map<TVm?>(await _dbSet.FirstOrDefaultAsync(filter));

    public TVm? GetById(int id) => _mapper.Map<TVm?>(_dbSet.Find(id));

    public async Task<TVm?> GetByIdAsync(int id) => _mapper.Map<TVm?>(await _dbSet.FindAsync(id));

    public TVm? Add(TVm vm)
    {
        var entity = _dbSet.Add(_mapper.Map<TEntity>(vm)).Entity;
        return _mapper.Map<TVm>(entity);
    }

    public async Task<TVm?> AddAsync(TVm vm)
    {
        var entity = (await _dbSet.AddAsync(_mapper.Map<TEntity>(vm))).Entity;
        return _mapper.Map<TVm>(entity);
    }

    /// <summary>
    /// Add or update view model
    /// </summary>
    /// <param name="id"></param>
    /// <param name="vm"></param>
    /// <returns></returns>
    public TVm? Update(int id, TVm vm)
    {
        var entity = _dbSet.Find(id);
        return entity is null
            ? _mapper.Map<TVm>((_dbSet.Add(_mapper.Map<TEntity>(vm))).Entity)
            : _mapper.Map<TVm>(_mapper.Map<TVm, TEntity>(vm, entity));
    }

    public async Task<TVm?> UpdateAsync(int id, TVm vm)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity is null
            ? _mapper.Map<TVm>((await _dbSet.AddAsync(_mapper.Map<TEntity>(vm))).Entity)
            : _mapper.Map<TVm>(_mapper.Map<TVm, TEntity>(vm, entity));
    }

    public TVm? Remove(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        return _mapper.Map<TVm?>(entity);
    }

    public async Task<TVm?> RemoveAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        return _mapper.Map<TVm?>(entity);
    }
}
