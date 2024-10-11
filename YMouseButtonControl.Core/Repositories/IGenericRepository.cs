using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YMouseButtonControl.Core.ViewModels.Models;

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

    IEnumerable<TVm> GetAll();
    IEnumerable<TVm> GetAll(Expression<Func<TEntity, bool>> filter);
    Task<TVm?> GetAsync(Expression<Func<TEntity, bool>> filter);
    TVm? GetById(int id);
    Task<TVm?> GetByIdAsync(int id);
    TVm? Remove(int id);
    Task<TVm?> RemoveAsync(int id);
    TVm? Update(int id, TVm vm);
    Task<TVm?> UpdateAsync(int id, TVm vm);
}
