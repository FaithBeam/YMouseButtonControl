using Splat;
using YMouseButtonControl.Core.Services;
using YMouseButtonControl.Core.ViewModels;

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
        services.Register(() => new MainWindowViewModel(resolver.GetRequiredService<IProfilesService>()));
    }
}