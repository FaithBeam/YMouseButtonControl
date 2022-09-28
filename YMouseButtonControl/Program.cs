using Avalonia;
using Avalonia.Collections;
using Avalonia.ReactiveUI;
using Splat;
using YMouseButtonControl.Core.DI;
using YMouseButtonControl.Core.Models;
using YMouseButtonControl.Core.Models.SimulatedKeystrokesTypes;
using YMouseButtonControl.UI;

namespace YMouseButtonControl;

internal static class Program
{
    public static void Main(string[] args)
    {
        var profiles = new AvaloniaList<Profile>
        {
            new()
            {
                Name = "Default",
                Checked = true,
                Description = "N/A",
                Process = "N/A",
                MatchType = "N/A",
                ParentClass = "N/A",
                WindowCaption = "N/A",
                WindowClass = "N/A",
                MouseButton4 = new SimulatedKeystrokes()
                {
                    CanRaiseDialog = true,
                    Keys = "w",
                    SimulatedKeystrokesType = new StickyHoldActionType(),
                },
                MouseButton4LastIndex = 2
            },
            new()
            {
                Name = "Profile 2",
                Checked = true,
                Description = "N/A",
                Process = "OG",
                MatchType = "N/A",
                ParentClass = "N/A",
                WindowCaption = "N/A",
                WindowClass = "N/A",
                MouseButton4 = new NothingMapping(),
                MouseButton4LastIndex = 0
            }
        };
        RegisterDependencies(profiles);
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    
    private static void RegisterDependencies(AvaloniaList<Profile> profiles) =>
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current, profiles);

}