using Splat;
using YMouseButtonControl.Configuration;

namespace YMouseButtonControl.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver,
        DataAccessConfiguration dataAccessConfig)
    {
        EnvironmentServicesBootstrapper.RegisterEnvironmentServices(services, resolver);
        ServicesBootstrapper.RegisterServices(services, resolver);
        ConfigurationBootstrapper.RegisterConfiguration(services, resolver, dataAccessConfig);
        DataAccessBootstrapper.RegisterDataAccess(services, resolver);
        KeyboardAndMouseBootstrapper.RegisterKeyboardAndMouse(services, resolver);
        ViewModelsBootstrapper.RegisterViewModels(services, resolver);
    }
}