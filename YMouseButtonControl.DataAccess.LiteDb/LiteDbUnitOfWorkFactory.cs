using LiteDB;
using YMouseButtonControl.Core.DataAccess.Configuration;
using YMouseButtonControl.Core.DataAccess.UnitOfWork;

namespace YMouseButtonControl.DataAccess.LiteDb;

public class LiteDbUnitOfWorkFactory(DatabaseConfiguration databaseConfiguration)
    : IUnitOfWorkFactory
{
    public IUnitOfWork Create() =>
        new LiteDbUnitOfWork(
            databaseConfiguration.UseInMemoryDatabase
                ? CreateInMemoryDatabase()
                : CreateDatabaseFromConnectionString()
        );

    private static LiteDatabase CreateInMemoryDatabase() =>
        new("Filename=:memory:;Connection=shared");

    private LiteDatabase CreateDatabaseFromConnectionString() =>
        new(databaseConfiguration.ConnectionString);
}
