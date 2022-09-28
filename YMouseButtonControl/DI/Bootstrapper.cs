using Avalonia.Collections;
using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.DI;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver,
        DataAccessConfiguration dataAccessConfiguration)
    {
        ServicesBootstrapper.RegisterServices(services, resolver);
        ViewModelsBootstrapper.RegisterViewModels(services, resolver);
        DataAccessBootstrapper.RegisterDataAccess(services, resolver);
    }
}