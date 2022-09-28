using Avalonia.Collections;
using Splat;
using YMouseButtonControl.Core.Models;
using YMouseButtonControl.Core.ViewModels;

namespace YMouseButtonControl.Core.DI;

public static class ViewModelsBootstrapper
{
    public static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver, AvaloniaList<Profile> profiles)
    {
        RegisterCommonViewModels(services, resolver, profiles);
    }

    public static void RegisterCommonViewModels(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver, AvaloniaList<Profile> profiles)
    {
        services.Register(() => new MainWindowViewModel(profiles));
    }
}