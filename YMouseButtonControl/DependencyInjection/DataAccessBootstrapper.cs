using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Queries;

namespace YMouseButtonControl.DependencyInjection;

public static class DataAccessBootstrapper
{
    public static void RegisterDataAccess(IServiceCollection services)
    {
        services
            .AddScoped<ProfileQueries>()
            .AddScoped<ButtonMappingQueries>()
            .AddScoped<SettingQueries>()
            .AddScoped<IRepository<Profile, ProfileVm>, ProfileRepository>()
            .AddScoped<IRepository<ButtonMapping, BaseButtonMappingVm>, ButtonMappingRepository>()
            .AddScoped<IRepository<Setting, BaseSettingVm>, SettingRepository>();
    }
}
