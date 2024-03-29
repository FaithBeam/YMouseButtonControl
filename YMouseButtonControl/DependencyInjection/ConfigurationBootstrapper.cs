using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DataAccess.Configuration;
using YMouseButtonControl.Services.Environment.Enums;
using YMouseButtonControl.Services.Environment.Interfaces;

namespace YMouseButtonControl.DependencyInjection;

public static class ConfigurationBootstrapper
{
    public static void RegisterConfiguration(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver,
        DataAccessConfiguration dataAccessConfig
    )
    {
        var configuration = BuildConfiguration();

        RegisterDatabaseConfiguration(services, resolver, configuration, dataAccessConfig);
    }

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    private static void RegisterDatabaseConfiguration(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver,
        IConfiguration configuration,
        DataAccessConfiguration dataAccessConfig
    )
    {
        var config = new DatabaseConfiguration
        {
            ConnectionString = GetDatabaseConnectionString(configuration, resolver),
            UseInMemoryDatabase = dataAccessConfig.UseInMemoryDatabase
        };
        services.RegisterConstant(config);
    }

    private static string GetDatabaseConnectionString(
        IConfiguration configuration,
        IReadonlyDependencyResolver resolver
    )
    {
        var platformService = resolver.GetRequiredService<IPlatformService>();
        var databaseName = configuration["DataAccess:DatabaseName"] ?? throw new Exception("DatabaseName empty");
        var connectionString = configuration["DataAccess:ConnectionString"] ?? throw new Exception("Connection empty");

        var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
        var dbDirectory = Path.GetDirectoryName(assemblyLocation) ?? throw new InvalidOperationException();
        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }
        
        return string.Format(connectionString, Path.Combine(dbDirectory, databaseName));
    }
}
