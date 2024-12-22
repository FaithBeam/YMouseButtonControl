using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.AppViewModel;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.ViewModels.ProfilesInformationViewModel;
using YMouseButtonControl.Core.ViewModels.ProfilesList;

namespace YMouseButtonControl.DependencyInjection;

public static class ViewModelsBootstrapper
{
    public static void RegisterViewModels(IServiceCollection services)
    {
        RegisterCommonViewModels(services);
    }

    private static void RegisterCommonViewModels(IServiceCollection services)
    {
        services
            //.AddScoped<IMouseComboViewModel, MouseComboViewModel>()
            .AddScoped<IProcessSelectorDialogViewModel, ProcessSelectorDialogViewModel>()
            .AddScoped<IGlobalSettingsDialogViewModel, GlobalSettingsDialogViewModel>()
            .AddScoped<IProfilesInformationViewModel, ProfilesInformationViewModel>()
            .AddScoped<
                IShowSimulatedKeystrokesDialogService,
                ShowSimulatedKeystrokesDialogService
            >()
            .AddScoped<ILayerViewModel, LayerViewModel>()
            .AddScoped<IProfilesListViewModel, ProfilesListViewModel>()
            .AddScoped<IMainWindowViewModel, MainWindowViewModel>()
            .AddScoped<IAppViewModel, AppViewModel>();
    }
}
