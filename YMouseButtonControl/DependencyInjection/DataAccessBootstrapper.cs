using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.DependencyInjection;

public static class DataAccessBootstrapper
{
    public static void RegisterDataAccess(IServiceCollection services)
    {
        services
            .AddScoped<IRepository<Profile, ProfileVm>, ProfileRepository>()
            .AddScoped<IRepository<ButtonMapping, BaseButtonMappingVm>, ButtonMappingRepository>()
            .AddScoped<IRepository<Setting, BaseSettingVm>, SettingRepository>()
            .AddScoped<IRepository<Theme, ThemeVm>, ThemeRepository>();
    }
}
