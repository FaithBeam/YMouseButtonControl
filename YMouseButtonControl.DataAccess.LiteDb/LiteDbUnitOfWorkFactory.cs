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

    public IUnitOfWork Create()
    {
        var database = _databaseConfiguration.UseInMemoryDatabase
            ? CreateInMemoryDatabase()
            : CreateDatabaseFromConnectionString();

        return new LiteDbUnitOfWork(database);
    }

    private static LiteDatabase CreateInMemoryDatabase() => new(new MemoryStream());

    private LiteDatabase CreateDatabaseFromConnectionString()
    {
        return new LiteDatabase(_databaseConfiguration.ConnectionString);
    }
}
