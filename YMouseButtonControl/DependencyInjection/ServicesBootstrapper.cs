using SharpHook;
using Splat;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.KeyboardAndMouse.SharpHook;
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
        services.RegisterLazySingleton<IMouseListener>(() => new MouseListener(
            new TaskPoolGlobalHook()
        ));
        services.RegisterLazySingleton<IProcessesService>(() => new ProcessesService());
        services.RegisterLazySingleton<IProfilesService>(() => new ProfilesService(
            resolver.GetRequiredService<IUnitOfWorkFactory>()
        ));
        services.RegisterLazySingleton<IProfileOperationsMediator>(() => new ProfileOperationsMediator());
        services.Register<ICheckDefaultProfileService>(() => new CheckDefaultProfileService(
            resolver.GetRequiredService<IUnitOfWorkFactory>(),
            resolver.GetRequiredService<IProfileOperationsMediator>()
        ));
    }
}