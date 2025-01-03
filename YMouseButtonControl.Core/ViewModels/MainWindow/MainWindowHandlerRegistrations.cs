﻿using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.MainWindow.Commands.Profiles;
using YMouseButtonControl.Core.ViewModels.MainWindow.Queries.Profiles;
using YMouseButtonControl.Core.ViewModels.MainWindow.Queries.Theme;

namespace YMouseButtonControl.Core.ViewModels.MainWindow;

public static class MainWindowHandlerRegistrations
{
    public static void RegisterCommon(IServiceCollection services) =>
        services
            .AddScoped<IsCacheDirty.Handler>()
            .AddScoped<ApplyProfiles.Handler>()
            .AddScoped<GetThemeVariant.Handler>();
}
