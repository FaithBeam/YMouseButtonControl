using System;
using Microsoft.Extensions.DependencyInjection;
using SharpHook;
using SharpHook.Reactive;
using YMouseButtonControl.Core.Services.KeyboardAndMouse;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedMousePressTypes;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Linux.Services;

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
            services.AddScoped<ISkipProfileService, Windows.Services.SkipProfileService>();
        }
        else if (OperatingSystem.IsMacOS())
        {
            services.AddScoped<ISkipProfileService, MacOS.Services.SkipProfileService>();
        }
        else if (OperatingSystem.IsLinux())
        {
            services.AddScoped<ISkipProfileService, SkipProfileService>();
        }
        else
        {
            throw new Exception("Unknown operating system");
        }
    }

    private static void RegisterCommonKeyboardAndMouseServices(IServiceCollection services)
    {
        services
            .AddScoped<IReactiveGlobalHook, SimpleReactiveGlobalHook>()
            .AddScoped<IMouseListener, MouseListener>()
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
