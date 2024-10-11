using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;
using IUnitOfWork = YMouseButtonControl.Core.Repositories.IUnitOfWork;

namespace YMouseButtonControl.DependencyInjection;

public static class DataAccessBootstrapper
{
    public static void RegisterDataAccess(IServiceCollection services)
    {
        services
            .AddScoped<IGenericRepository<Profile, ProfileVm>, ProfileRepository>()
            .AddScoped<
                IGenericRepository<ButtonMapping, BaseButtonMappingVm>,
                ButtonMappingRepository
            >()
            .AddScoped<IGenericRepository<Setting, BaseSettingVm>, SettingRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
