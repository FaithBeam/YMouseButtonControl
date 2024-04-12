using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.Core.DataAccess.Configuration;

namespace YMouseButtonControl.DependencyInjection;

public static class ConfigurationBootstrapper
{
    public static void RegisterConfiguration(
        IServiceCollection services,
        DataAccessConfiguration dataAccessConfig
    )
    {
        var configuration = BuildConfiguration();

        RegisterDatabaseConfiguration(services, configuration, dataAccessConfig);
    }

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    private static void RegisterDatabaseConfiguration(
        IServiceCollection services,
        IConfiguration configuration,
        DataAccessConfiguration dataAccessConfig
    )
    {
        services.AddTransient<DatabaseConfiguration>(_ => new DatabaseConfiguration
        {
            ConnectionString = GetDatabaseConnectionString(configuration),
            UseInMemoryDatabase = dataAccessConfig.UseInMemoryDatabase
        });
    }

    private static string GetDatabaseConnectionString(IConfiguration configuration)
    {
        var databaseName =
            configuration["DataAccess:DatabaseName"] ?? throw new Exception("DatabaseName empty");
        var connectionString =
            configuration["DataAccess:ConnectionString"] ?? throw new Exception("Connection empty");

        var dbDirectory =
            Path.GetDirectoryName(AppContext.BaseDirectory)
            ?? throw new InvalidOperationException();
        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        return string.Format(connectionString, Path.Combine(dbDirectory, databaseName));
    }
}
