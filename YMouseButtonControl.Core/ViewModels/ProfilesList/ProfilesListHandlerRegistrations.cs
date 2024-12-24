using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Commands.Profiles;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Queries.Profiles;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList;

public static class ProfilesListHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services)
    {
        services
            .AddScoped<ListProfiles.Handler>()
            .AddScoped<GetCurrentProfile.Handler>()
            .AddScoped<SetCurrentProfile.Handler>()
            .AddScoped<AddProfile.Handler>()
            .AddScoped<RemoveProfile.Handler>()
            .AddScoped<ExportProfile.Handler>()
            .AddScoped<CopyProfile.Handler>();
    }
}
