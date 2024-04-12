using LiteDB;
using YMouseButtonControl.Core.DataAccess.Repositories;
using YMouseButtonControl.Core.DataAccess.UnitOfWork;

namespace YMouseButtonControl.DataAccess.LiteDb;

public class LiteDbUnitOfWork(LiteDatabase database) : IUnitOfWork
{
    public IRepository<T> GetRepository<T>()
        where T : class
    {
        var collection = database.GetCollection<T>();

        return new Repository<T>(collection);
    }

    public void SaveChanges() => database.Commit();

    public void Dispose() => database.Dispose();
}
