using Splat;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
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
            resolver.GetRequiredService<IProcessMonitorService>()
        ));
        services.RegisterLazySingleton<IProfilesInformationViewModel>(() => new ProfilesInformationViewModel(
            resolver.GetRequiredService<ICurrentProfileOperationsMediator>()
        ));
        services.RegisterLazySingleton<ILayerViewModel>(() => new LayerViewModel(
            resolver.GetRequiredService<ICurrentProfileOperationsMediator>(),
            resolver.GetRequiredService<IMouseListener>()
        ));
        services.RegisterLazySingleton<IProfilesListViewModel>(() => new ProfilesListViewModel(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<ICurrentProfileOperationsMediator>(),
            resolver.GetRequiredService<ProcessSelectorDialogViewModel>()
        ));
        services.RegisterLazySingleton<IMainWindowViewModel>(() => new MainWindowViewModel(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<ICurrentProfileOperationsMediator>(),
            resolver.GetRequiredService<ILayerViewModel>(),
            resolver.GetRequiredService<IProfilesListViewModel>(),
            resolver.GetRequiredService<IProfilesInformationViewModel>()
        ));
    }
}