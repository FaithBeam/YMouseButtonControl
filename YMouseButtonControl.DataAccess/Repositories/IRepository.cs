using System.Collections.Generic;

namespace YMouseButtonControl.DataAccess.Repositories;

public interface IRepository<T>
{
    T GetById(string id);

    IEnumerable<T> GetAll();

    void Add(T entity);

    void Update(string id, T entity);

    void Upsert(string id, T entity);

    void Remove(string id);
}