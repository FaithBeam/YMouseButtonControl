using Splat;
using YMouseButtonControl.Core.Config;

namespace YMouseButtonControl.Core.DI;

public static class ConfigurationBootstrapper
{
    public static void RegisterConfiguration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterProfilesConfiguration(services, resolver);
    }

    public static void RegisterProfilesConfiguration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        var profiles = ProfilesConfiguration.LoadProfiles();
        services.RegisterConstant(profiles);
    }
}