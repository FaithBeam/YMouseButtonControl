using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.DataAccess.UnitOfWork;
using YMouseButtonControl.DataAccess.LiteDb;

namespace YMouseButtonControl.DependencyInjection;

public static class DataAccessBootstrapper
{
    public static void RegisterDataAccess(IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWorkFactory, LiteDbUnitOfWorkFactory>();
    }
}
