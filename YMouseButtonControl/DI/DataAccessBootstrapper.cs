using Splat;
using YMouseButtonControl.Configuration;
using YMouseButtonControl.DataAccess.Configuration;
using YMouseButtonControl.DataAccess.LiteDb;
using YMouseButtonControl.DataAccess.UnitOfWork;

namespace YMouseButtonControl.DI;

public static class DataAccessBootstrapper
{
    public static void RegisterDataAccess(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IUnitOfWorkFactory>(() => new LiteDbUnitOfWorkFactory(
            resolver.GetRequiredService<DatabaseConfiguration>()
        ));
    }    
}
