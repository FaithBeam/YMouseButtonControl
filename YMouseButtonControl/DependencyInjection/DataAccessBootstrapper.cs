using Splat;
using YMouseButtonControl.DataAccess.Configuration;
using YMouseButtonControl.DataAccess.LiteDb;
using YMouseButtonControl.DataAccess.UnitOfWork;

namespace YMouseButtonControl.DependencyInjection;

public static class DataAccessBootstrapper
{
    public static void RegisterDataAccess(
        IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver
    )
    {
        services.RegisterLazySingleton<IUnitOfWorkFactory>(
            () => new LiteDbUnitOfWorkFactory(resolver.GetRequiredService<DatabaseConfiguration>())
        );
    }
}
