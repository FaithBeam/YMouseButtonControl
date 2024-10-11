using System.Collections.Generic;
using System.Threading.Tasks;

namespace YMouseButtonControl.Core.Repositories;

public interface IRepository<TEntity, TVm>
{
    int Add(TVm vm);
    Task<int> AddAsync(TVm vm);
    TVm? GetByName(string name);
    TVm? GetById(int id);
    Task<TVm?> GetByIdAsync(int id);
    IEnumerable<TVm> GetAll();
    Task<IEnumerable<TVm>> GetAllAsync();
    int Update(TVm vm);
    Task<int> UpdateAsync(TVm vm);
    int Delete(TVm vm);
    Task<int> DeleteAsync(TVm vm);
}
