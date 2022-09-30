using Splat;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.ViewModels.Services;
using YMouseButtonControl.ViewModels.Services.Implementations;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterCommonServices(services, resolver);
    }

    private static void RegisterCommonServices(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IProfilesService>(() => new ProfilesService(
            resolver.GetRequiredService<IUnitOfWorkFactory>()
        ));
        services.Register<ICheckDefaultProfileService>(() => new CheckDefaultProfileService(
            resolver.GetRequiredService<IUnitOfWorkFactory>()
        ));
        services.RegisterLazySingleton<IProfileOperationsMediator>(() => new ProfileOperationsMediator());
    }
}