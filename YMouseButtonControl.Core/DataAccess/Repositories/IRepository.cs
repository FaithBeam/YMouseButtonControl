using System.Collections.Generic;

namespace YMouseButtonControl.Core.DataAccess.Repositories;

public interface IRepository<T>
{
    T GetById(string id);
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(string id, T entity);
    void Upsert(string id, T entity);
    void Remove(string id);
    void ApplyAction(IEnumerable<T> entities);
}
