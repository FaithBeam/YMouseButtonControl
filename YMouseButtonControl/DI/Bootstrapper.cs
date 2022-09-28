using Avalonia.Collections;
using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.DI;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver,
        DataAccessConfiguration dataAccessConfig)
    {
        EnvironmentServicesBootstrapper.RegisterEnvironmentServices(services, resolver);
        ServicesBootstrapper.RegisterServices(services, resolver);
        ViewModelsBootstrapper.RegisterViewModels(services, resolver);
        ConfigurationBootstrapper.RegisterConfiguration(services, resolver, dataAccessConfig);
        DataAccessBootstrapper.RegisterDataAccess(services, resolver);
    }
}