using Splat;
using YMouseButtonControl.Services.Environment.Implementations;
using YMouseButtonControl.Services.Environment.Interfaces;

namespace YMouseButtonControl.DependencyInjection;

public static class EnvironmentServicesBootstrapper
{
    public static void RegisterEnvironmentServices(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        RegisterCommonServices(services);
    }

    private static void RegisterCommonServices(IMutableDependencyResolver services)
    {
        services.Register<IPlatformService>(() => new PlatformService());
    }
}