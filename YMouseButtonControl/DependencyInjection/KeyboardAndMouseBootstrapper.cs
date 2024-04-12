using System;
using Microsoft.Extensions.DependencyInjection;
using SharpHook;
using YMouseButtonControl.Core.KeyboardAndMouse;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse;
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
            services.AddTransient<
                ISkipProfileService,
                Services.Windows.Implementations.SkipProfileService
            >();
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            services.AddTransient<
                ISkipProfileService,
                Services.MacOS.Implementations.SkipProfileService
            >();
        }
        else
        {
            throw new Exception("Unknown operating system");
        }
    }

    private static void RegisterCommonKeyboardAndMouseServices(IServiceCollection services)
    {
        services.AddSingleton<IGlobalHook, SimpleGlobalHook>();
        services.AddTransient<IMouseListener, MouseListener>();
        services.AddTransient<IEventSimulator, EventSimulator>();
        services.AddTransient<ISimulateKeyService, SimulateKeyService>();
        services.AddTransient<IStickyHoldService, StickyHoldService>();
        services.AddTransient<IAsMouseButtonPressedService, AsMouseButtonPressedService>();
        services.AddTransient<IAsMouseButtonReleasedService, AsMouseButtonReleasedService>();
        services.AddTransient<
            IDuringMousePressAndReleaseService,
            DuringMousePressAndReleaseService
        >();
        services.AddTransient<IRepeatedWhileButtonDownService, RepeatedWhileButtonDownService>();
        services.AddTransient<IStickyRepeatService, StickyRepeatService>();
        services.AddTransient<ISimulateMouseService, SimulateMouseService>();
        services.AddTransient<IRightClick, RightClick>();
        services.AddTransient<KeyboardSimulatorWorker>();
    }
}
