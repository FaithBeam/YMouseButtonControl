using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.Profiles;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;

public static class MouseListenerHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services) =>
        services.AddScoped<ListProfiles.Handler>();
}
