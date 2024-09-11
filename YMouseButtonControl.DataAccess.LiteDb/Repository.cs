using System.Collections.Generic;
using LiteDB;
using YMouseButtonControl.Core.DataAccess.Repositories;

namespace YMouseButtonControl.DataAccess.LiteDb;

public class Repository<T>(ILiteCollection<T> collection) : IRepository<T>
    where T : class
{
    public T GetById(string id) => collection.FindById(id);

    public T GetById(int id) => collection.FindById(id);

    public IEnumerable<T> GetAll() => collection.FindAll();

    public void Add(T entity) => collection.Insert(entity);

    public void Update(string id, T entity) => collection.Update(id, entity);

    public void Upsert(string id, T entity) => collection.Upsert(id, entity);

    public void Remove(string id) => collection.Delete(id);

    public void ApplyAction(IEnumerable<T> entities)
    {
        collection.DeleteAll();
        collection.InsertBulk(entities);
    }
}
