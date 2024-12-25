using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.App;
using YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog;
using YMouseButtonControl.Core.ViewModels.Layer;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.ViewModels.ProfilesInformation;
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
            .AddScoped<IProcessSelectorDialogViewModel, ProcessSelectorDialogViewModel>()
            .AddScoped<IGlobalSettingsDialogViewModel, GlobalSettingsDialogViewModel>()
            .AddScoped<IProfilesInformationViewModel, ProfilesInformationViewModel>()
            .AddScoped<ILayerViewModel, LayerViewModel>()
            .AddScoped<IProfilesListViewModel, ProfilesListViewModel>()
            .AddScoped<IMainWindowViewModel, MainWindowViewModel>()
            .AddScoped<IAppViewModel, AppViewModel>();
    }
}
