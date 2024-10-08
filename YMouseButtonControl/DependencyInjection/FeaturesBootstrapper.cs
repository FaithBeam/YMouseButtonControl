﻿using Microsoft.Extensions.DependencyInjection;
using YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Features.Add;

namespace YMouseButtonControl.DependencyInjection;

public static class FeaturesBootstrapper
{
    public static void RegisterFeatures(IServiceCollection services)
    {
        services.AddTransient<IApply, Apply>().AddTransient<IAddProfile, AddProfile>();
    }
}
