using System;
using System.Collections.Generic;
using LiteDB;
using YMouseButtonControl.DataAccess.Repositories;

namespace YMouseButtonControl.DataAccess.LiteDb;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ILiteCollection<T> _collection;

    public Repository(ILiteCollection<T> collection)
    {
        _collection = collection;
    }

    public T GetById(string id) => _collection.FindById(id);

    public IEnumerable<T> GetAll() => _collection.FindAll();

    public void Add(T entity) => _collection.Insert(entity);

    public void Update(string id, T entity) => _collection.Update(id, entity);

    public void Upsert(string id, T entity) => _collection.Upsert(id, entity);

    public void Remove(string id) => _collection.Delete(id);
}