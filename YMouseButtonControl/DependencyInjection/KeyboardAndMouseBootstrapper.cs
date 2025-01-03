﻿using System;
using Microsoft.Extensions.DependencyInjection;
using SharpHook;
using SharpHook.Reactive;
using SharpHook.Testing;
using YMouseButtonControl.Core.Services.KeyboardAndMouse;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.SkipProfile;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedMousePressTypes;

namespace YMouseButtonControl.DependencyInjection;

public static class KeyboardAndMouseBootstrapper
{
    public static void RegisterKeyboardAndMouse(IServiceCollection services)
    {
        RegisterOsSpecificKeyboardAndMouseServices(services);
        RegisterCommonKeyboardAndMouseServices(services);
    }

    private static void RegisterOsSpecificKeyboardAndMouseServices(IServiceCollection services)
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600))
        {
            services.AddScoped<ISkipProfile, SkipProfileWindows>();
        }
        else if (OperatingSystem.IsMacOS())
        {
            services.AddScoped<ISkipProfile, SkipProfileOsx>();
        }
        else if (OperatingSystem.IsLinux())
        {
            services.AddScoped<ISkipProfile, SkipProfileLinux>();
        }
        else
        {
            throw new Exception("Unknown operating system");
        }
    }

    private static void RegisterCommonKeyboardAndMouseServices(IServiceCollection services)
    {
        services
            .AddScoped<IMouseButtonMappingService, MouseButtonMappingService>()
#if DEBUG
            .AddScoped<IReactiveGlobalHook>(
                (_) => new SimpleReactiveGlobalHook(globalHookProvider: new TestProvider())
            )
#else
            .AddScoped<IReactiveGlobalHook, SimpleReactiveGlobalHook>()
#endif
            .AddScoped<IMouseListener, MouseListenerService>()
            .AddScoped<IEventSimulator, EventSimulator>()
            .AddScoped<IEventSimulatorService, EventSimulatorService>()
            .AddScoped<IStickyHoldService, StickyHoldService>()
            .AddScoped<IAsMouseButtonPressedService, AsMouseButtonPressedService>()
            .AddScoped<IAsMouseButtonReleasedService, AsMouseButtonReleasedService>()
            .AddScoped<IDuringMousePressAndReleaseService, DuringMousePressAndReleaseService>()
            .AddScoped<IRepeatedWhileButtonDownService, RepeatedWhileButtonDownService>()
            .AddScoped<IStickyRepeatService, StickyRepeatService>()
            .AddScoped<IRightClick, RightClick>()
            .AddScoped<KeyboardSimulatorWorker>();
    }
}
