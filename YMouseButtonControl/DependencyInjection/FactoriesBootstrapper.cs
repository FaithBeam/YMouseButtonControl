using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog;
using YMouseButtonControl.Core.ViewModels.Dialogs.SimulatedKeystrokesDialog;
using YMouseButtonControl.Core.ViewModels.MouseCombo;

namespace YMouseButtonControl.DependencyInjection;

public static class FactoriesBootstrapper
{
    public static void RegisterFactories(IServiceCollection services)
    {
        services
            .AddScoped<IMouseComboViewModelFactory, MouseComboViewModelFactory>()
            .AddScoped<ISimulatedKeystrokesDialogVmFactory, SimulatedKeystrokesDialogVmFactory>()
            .AddScoped<IProcessSelectorDialogVmFactory, ProcessSelectorDialogVmFactory>();
    }
}
