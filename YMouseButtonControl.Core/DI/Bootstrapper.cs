using Avalonia.Collections;
using Splat;
using YMouseButtonControl.Core.Models;

namespace YMouseButtonControl.Core.DI;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver, AvaloniaList<Profile> profiles)
    {
        ViewModelsBootstrapper.RegisterViewModels(services, resolver, profiles);
    }
}