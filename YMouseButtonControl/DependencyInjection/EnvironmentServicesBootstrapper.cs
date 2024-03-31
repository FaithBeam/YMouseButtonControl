using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Services.Environment.Implementations;
using YMouseButtonControl.Services.Environment.Interfaces;

namespace YMouseButtonControl.DependencyInjection;

public static class EnvironmentServicesBootstrapper
{
    public static void RegisterEnvironmentServices(IServiceCollection services)
    {
        RegisterCommonServices(services);
    }

    private static void RegisterCommonServices(IServiceCollection services)
    {
        services.AddTransient<IPlatformService, PlatformService>();
    }
}
