using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Configuration;

namespace YMouseButtonControl.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(
        IServiceCollection services,
        DataAccessConfiguration dataAccessConfig
    )
    {
        ServicesBootstrapper.RegisterServices(services);
        ConfigurationBootstrapper.RegisterConfiguration(services, dataAccessConfig);
        DataAccessBootstrapper.RegisterDataAccess(services);
        KeyboardAndMouseBootstrapper.RegisterKeyboardAndMouse(services);
        ViewModelsBootstrapper.RegisterViewModels(services);
    }
}
