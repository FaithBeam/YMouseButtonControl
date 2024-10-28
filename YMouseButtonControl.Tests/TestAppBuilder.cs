using Avalonia;
using Avalonia.Headless;
using Avalonia.ReactiveUI;
using YMouseButtonControl;
using YMouseButtonControl.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder
            .Configure<App>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions())
            .LogToTrace()
            .UseReactiveUI();
}
