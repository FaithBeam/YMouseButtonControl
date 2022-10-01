using Splat;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;
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
        services.Register(() => new ProcessSelectorDialogViewModel(
            resolver.GetRequiredService<IProcessesService>()
        ));
        services.RegisterLazySingleton<IProfilesInformationViewModel>(() => new ProfilesInformationViewModel(
            resolver.GetRequiredService<IProfileOperationsMediator>()
        ));
        services.RegisterLazySingleton<ILayerViewModel>(() => new LayerViewModel(
            resolver.GetRequiredService<IProfileOperationsMediator>()));
        services.RegisterLazySingleton<IProfilesListViewModel>(() => new ProfilesListViewModel(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<IProfileOperationsMediator>(),
            resolver.GetRequiredService<ProcessSelectorDialogViewModel>()
        ));
        services.RegisterLazySingleton<IMainWindowViewModel>(() => new MainWindowViewModel(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<IProfileOperationsMediator>(),
            resolver.GetRequiredService<ILayerViewModel>(),
            resolver.GetRequiredService<IProfilesListViewModel>(),
            resolver.GetRequiredService<IProfilesInformationViewModel>()
        ));
    }
}