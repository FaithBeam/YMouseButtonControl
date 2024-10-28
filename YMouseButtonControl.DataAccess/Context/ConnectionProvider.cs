using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace YMouseButtonControl.DataAccess.Context;

public interface IConnectionProvider
{
    IConfigurationRoot? Configuration { get; }

    IDbConnection CreateConnection();
}

public class ConnectionProvider(IConfigurationRoot? configuration) : IConnectionProvider
{
    public IConfigurationRoot? Configuration { get; } = configuration;
    private SqliteConnection? _connection;

    public IDbConnection CreateConnection()
    {
        if (Configuration == null)
        {
            _connection ??= new SqliteConnection("Data Source=:memory:;Cache=Shared");
            return _connection;
        }
        _connection ??= new SqliteConnection(
            Configuration?.GetConnectionString("YMouseButtonControlContext")
        );
        return _connection;
    }
}
