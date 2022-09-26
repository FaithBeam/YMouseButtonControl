using Splat;

namespace YMouseButtonControl.Core.DI;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        ViewModelsBootstrapper.RegisterViewModels(services, resolver);
    }
}