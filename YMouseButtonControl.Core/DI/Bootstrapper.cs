using Splat;

namespace YMouseButtonControl.Core.DI;

public class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        ViewModelsBootstrapper.RegisterViewModels(services, resolver);
    }
}