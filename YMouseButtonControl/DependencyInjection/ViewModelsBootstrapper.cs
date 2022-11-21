using Splat;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;

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
            resolver.GetRequiredService<IProfilesService>()
        ));
        services.RegisterLazySingleton<ILayerViewModel>(() => new LayerViewModel(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<IMouseListener>()
        ));
        services.RegisterLazySingleton<IProfilesListViewModel>(() => new ProfilesListViewModel(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<ProcessSelectorDialogViewModel>()
        ));
        services.RegisterLazySingleton<IMainWindowViewModel>(() => new MainWindowViewModel(
            resolver.GetRequiredService<IProfilesService>(),
            resolver.GetRequiredService<ILayerViewModel>(),
            resolver.GetRequiredService<IProfilesListViewModel>(),
            resolver.GetRequiredService<IProfilesInformationViewModel>()
        ));
    }
}