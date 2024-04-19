using System;
using Microsoft.Extensions.DependencyInjection;
using SharpHook;
using SharpHook.Reactive;
using YMouseButtonControl.Core.KeyboardAndMouse;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedMousePressTypes;

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
            services.AddSingleton<ISkipProfileService, Services.Windows.SkipProfileService>();
        }
        else if (OperatingSystem.IsMacOS())
        {
            services.AddSingleton<ISkipProfileService, Services.MacOS.SkipProfileService>();
        }
        else if (OperatingSystem.IsLinux())
        {
            services.AddSingleton<ISkipProfileService, Services.Linux.SkipProfileService>();
        }
        else
        {
            throw new Exception("Unknown operating system");
        }
    }

    private static void RegisterCommonKeyboardAndMouseServices(IServiceCollection services)
    {
        services.AddSingleton<IReactiveGlobalHook, SimpleReactiveGlobalHook>();
        services.AddSingleton<IMouseListener, MouseListener>();
        services.AddSingleton<IEventSimulator, EventSimulator>();
        services.AddSingleton<IEventSimulatorService, EventSimulatorService>();
        services.AddSingleton<IStickyHoldService, StickyHoldService>();
        services.AddSingleton<IAsMouseButtonPressedService, AsMouseButtonPressedService>();
        services.AddSingleton<IAsMouseButtonReleasedService, AsMouseButtonReleasedService>();
        services.AddSingleton<
            IDuringMousePressAndReleaseService,
            DuringMousePressAndReleaseService
        >();
        services.AddSingleton<IRepeatedWhileButtonDownService, RepeatedWhileButtonDownService>();
        services.AddSingleton<IStickyRepeatService, StickyRepeatService>();
        services.AddSingleton<IRightClick, RightClick>();
        services.AddSingleton<KeyboardSimulatorWorker>();
    }
}
