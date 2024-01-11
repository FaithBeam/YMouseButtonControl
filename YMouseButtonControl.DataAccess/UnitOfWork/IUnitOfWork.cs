using System;
using YMouseButtonControl.DataAccess.Repositories;

namespace YMouseButtonControl.DataAccess.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class;

    void SaveChanges();
}