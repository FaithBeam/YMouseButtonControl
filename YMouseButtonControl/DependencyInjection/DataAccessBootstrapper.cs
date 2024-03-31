using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.DataAccess.LiteDb;
using YMouseButtonControl.DataAccess.UnitOfWork;

namespace YMouseButtonControl.DependencyInjection;

public static class DataAccessBootstrapper
{
    public static void RegisterDataAccess(IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWorkFactory, LiteDbUnitOfWorkFactory>();
    }
}
