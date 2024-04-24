using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.Implementations;
using YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.Core.ViewModels.Interfaces;
using YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.ViewModels.Services;

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
