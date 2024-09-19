using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Repositories;
using IUnitOfWork = YMouseButtonControl.Core.Repositories.IUnitOfWork;

namespace YMouseButtonControl.DependencyInjection;

public static class DataAccessBootstrapper
{
    public static void RegisterDataAccess(IServiceCollection services)
    {
        services
            .AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>))
            .AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
