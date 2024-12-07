using System.Collections.Generic;
using System.Threading.Tasks;

namespace YMouseButtonControl.Core.Repositories;

public interface IRepository<TEntity, TVm>
{
    int Add(TVm vm);
    TVm? GetByName(string name);
    TVm? GetById(int id);
    IEnumerable<TVm> GetAll();
    int Update(TVm vm);
    int Delete(TVm vm);
}
