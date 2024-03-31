using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.ViewModels;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;
using YMouseButtonControl.ViewModels.Services;

namespace YMouseButtonControl.DependencyInjection;

public static class ViewModelsBootstrapper
{
    public static void RegisterViewModels(IServiceCollection services)
    {
        RegisterCommonViewModels(services);
    }

    private static void RegisterCommonViewModels(IServiceCollection services)
    {
        services.AddTransient<IProcessSelectorDialogViewModel, ProcessSelectorDialogViewModel>();
        services.AddSingleton<IProfilesInformationViewModel, ProfilesInformationViewModel>();
        services.AddSingleton<
            IShowSimulatedKeystrokesDialogService,
            ShowSimulatedKeystrokesDialogService
        >();
        services.AddSingleton<ILayerViewModel, LayerViewModel>();
        services.AddSingleton<IProfilesListViewModel, ProfilesListViewModel>();
        services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
        services.AddSingleton<IAppViewModel, AppViewModel>();
    }
}
