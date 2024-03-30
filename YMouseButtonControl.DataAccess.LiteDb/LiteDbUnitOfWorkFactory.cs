using System.IO;
using LiteDB;
using YMouseButtonControl.DataAccess.Configuration;
using YMouseButtonControl.DataAccess.UnitOfWork;

namespace YMouseButtonControl.DataAccess.LiteDb;

public class LiteDbUnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly DatabaseConfiguration _databaseConfiguration;

    public LiteDbUnitOfWorkFactory(DatabaseConfiguration databaseConfiguration)
    {
        _databaseConfiguration = databaseConfiguration;
    }

    public IUnitOfWork Create() =>
        new LiteDbUnitOfWork(
            _databaseConfiguration.UseInMemoryDatabase
                ? CreateInMemoryDatabase()
                : CreateDatabaseFromConnectionString()
        );

    private static LiteDatabase CreateInMemoryDatabase() =>
        new("Filename=:memory:;Connection=shared");

    private LiteDatabase CreateDatabaseFromConnectionString() =>
        new(_databaseConfiguration.ConnectionString);
}
