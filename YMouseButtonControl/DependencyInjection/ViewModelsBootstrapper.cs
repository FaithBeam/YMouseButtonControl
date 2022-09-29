using Splat;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.DependencyInjection;

public static class ViewModelsBootstrapper
{
    public static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterCommonViewModels(services, resolver);
    }

    private static void RegisterCommonViewModels(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<ILayerViewModel>(() => new LayerViewModel());
        services.RegisterLazySingleton<IMainWindowViewModel>(() => new MainWindowViewModel(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<ILayerViewModel>()
        ));
    }
}