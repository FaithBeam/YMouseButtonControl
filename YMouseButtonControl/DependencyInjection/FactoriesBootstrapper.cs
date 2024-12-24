using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.MouseComboViewModel;
using YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm;

namespace YMouseButtonControl.DependencyInjection;

public static class FactoriesBootstrapper
{
    public static void RegisterFactories(IServiceCollection services)
    {
        services
            .AddScoped<IMouseComboViewModelFactory, MouseComboViewModelFactory>()
            .AddScoped<IProcessSelectorDialogVmFactory, ProcessSelectorDialogVmFactory>();
    }
}
