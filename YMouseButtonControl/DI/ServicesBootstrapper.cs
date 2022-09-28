using Splat;
using YMouseButtonControl.Core.Services;
using YMouseButtonControl.DataAccess.UnitOfWork;

namespace YMouseButtonControl.DI;

public static class ServicesBootstrapper
{
    public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterCommonServices(services, resolver);
    }

    private static void RegisterCommonServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.Register<IProfilesService>(() => new ProfilesService(
            resolver.GetRequiredService<IUnitOfWorkFactory>()
        ));
    }
}