using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.Core.Config;
using YMouseButtonControl.DataAccess.Configuration;

namespace YMouseButtonControl.DI;

public static class ConfigurationBootstrapper
{
    public static void RegisterConfiguration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver, DataAccessConfiguration dataAccessConfig)
    {
        var builder = BuildConfiguration();
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
}