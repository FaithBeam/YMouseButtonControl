﻿using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DataAccess.Configuration;
using YMouseButtonControl.Services.Environment.Enums;
using YMouseButtonControl.Services.Environment.Interfaces;

namespace YMouseButtonControl.DI;

public static class ConfigurationBootstrapper
{
    public static void RegisterConfiguration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver, DataAccessConfiguration dataAccessConfig)
    {
        var configuration = BuildConfiguration();
        
        RegisterDatabaseConfiguration(services, resolver, configuration, dataAccessConfig);
    }

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    
    private static void RegisterDatabaseConfiguration(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver, IConfiguration configuration, DataAccessConfiguration dataAccessConfig)
    {
        var config = new DatabaseConfiguration
        {
            ConnectionString = GetDatabaseConnectionString(configuration, resolver),
            UseInMemoryDatabase = dataAccessConfig.UseInMemoryDatabase
        };
        services.RegisterConstant(config);
    }
    
    private static string GetDatabaseConnectionString(IConfiguration configuration,
        IReadonlyDependencyResolver resolver)
    {
        var platformService = resolver.GetRequiredService<IPlatformService>();
        var databaseName = configuration["DataAccess:DatabaseName"];
        var connectionString = configuration["DataAccess:ConnectionString"];

        string dbDirectory;
        if (platformService.GetPlatform() == Platform.Linux)
        {
            var environmentService = resolver.GetRequiredService<IEnvironmentService>();

            dbDirectory = $"{environmentService.GetEnvironmentVariable("HOME")}/.config/camelot";
        }
        else
        {
            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            dbDirectory = Path.GetDirectoryName(assemblyLocation);
        }

        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        return string.Format(connectionString, Path.Combine(dbDirectory, databaseName));
    }
}