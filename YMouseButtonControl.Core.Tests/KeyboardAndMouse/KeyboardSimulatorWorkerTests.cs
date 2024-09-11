using NSubstitute;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;
using SharpHook.Testing;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Core.KeyboardAndMouse;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Services.Windows;
using RightClick = YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedMousePressTypes.RightClick;

namespace YMouseButtonControl.Core.Tests.KeyboardAndMouse;

[Platform("Win")]
public class KeyboardSimulatorWorkerTests : BaseTest
{
    [Test]
    public void DefaultProfileSendsKeys()
    {
        var pf = GetProfilesService(GetSeedData());
        pf.CurrentProfile!.MouseButton3 = new SimulatedKeystrokes
        {
            SimulatedKeystrokesType = new MouseButtonPressedActionType(),
            BlockOriginalMouseInput = true,
            Keys = "wee",
        };
        var testProvider = new TestProvider();
        using var hook = new SimpleReactiveGlobalHook(globalHookProvider: testProvider);
        var curWinSvc = Substitute.For<ICurrentWindowService>();
        var mouseListener = new MouseListener(hook, pf, curWinSvc);
        var skipProfileSvc = Substitute.For<ISkipProfileService>();
        var eventSimulatorMock = Substitute.For<IEventSimulator>();
        var eventSimulatorSvc = new EventSimulatorService(eventSimulatorMock);
        var sut = new KeyboardSimulatorWorker(
            pf,
            mouseListener,
            skipProfileSvc,
            new AsMouseButtonPressedService(eventSimulatorSvc),
            new AsMouseButtonReleasedService(eventSimulatorSvc),
            new DuringMousePressAndReleaseService(eventSimulatorSvc),
            new RepeatedWhileButtonDownService(eventSimulatorSvc),
            new StickyRepeatService(eventSimulatorSvc),
            new StickyHoldService(eventSimulatorSvc),
            new RightClick(eventSimulatorSvc)
        );

        var evtDn = new UioHookEvent
        {
            Type = EventType.MousePressed,
            Mouse = new MouseEventData { Button = MouseButton.Button3 },
        };
        // var evtUp = new UioHookEvent
        // {
        //     Type = EventType.MouseReleased,
        //     Mouse = new MouseEventData { Button = MouseButton.Button3, }
        // };
        RunHookAndWaitForStart(hook, testProvider);

        sut.Run();
        testProvider.PostEvent(ref evtDn);
        // testProvider.PostEvent(ref evtUp);

        eventSimulatorMock.Received(1).SimulateKeyPress(Arg.Is(KeyCode.VcW));
        eventSimulatorMock.Received(2).SimulateKeyPress(Arg.Is(KeyCode.VcE));
        eventSimulatorMock.Received(2).SimulateKeyRelease(Arg.Is(KeyCode.VcE));
        eventSimulatorMock.Received(1).SimulateKeyRelease(Arg.Is(KeyCode.VcW));
    }

    [Test]
    public void NonDefaultProfileSendsKeys()
    {
        var pf = GetProfilesService(GetSeedData());
        pf.Profiles.First(x => !x.IsDefault).MouseButton3 = new SimulatedKeystrokes
        {
            SimulatedKeystrokesType = new MouseButtonPressedActionType(),
            BlockOriginalMouseInput = true,
            Keys = "wee",
        };
        var testProvider = new TestProvider();
        using var hook = new SimpleReactiveGlobalHook(globalHookProvider: testProvider);
        var curWinSvc = Substitute.For<ICurrentWindowService>();
        var mouseListener = new MouseListener(hook, pf, curWinSvc);
        var skipProfileSvc = Substitute.For<ISkipProfileService>();
        var eventSimulatorMock = Substitute.For<IEventSimulator>();
        var eventSimulatorSvc = new EventSimulatorService(eventSimulatorMock);
        var sut = new KeyboardSimulatorWorker(
            pf,
            mouseListener,
            skipProfileSvc,
            new AsMouseButtonPressedService(eventSimulatorSvc),
            new AsMouseButtonReleasedService(eventSimulatorSvc),
            new DuringMousePressAndReleaseService(eventSimulatorSvc),
            new RepeatedWhileButtonDownService(eventSimulatorSvc),
            new StickyRepeatService(eventSimulatorSvc),
            new StickyHoldService(eventSimulatorSvc),
            new RightClick(eventSimulatorSvc)
        );

        var evtDn = new UioHookEvent
        {
            Type = EventType.MousePressed,
            Mouse = new MouseEventData { Button = MouseButton.Button3 },
        };
        // var evtUp = new UioHookEvent
        // {
        //     Type = EventType.MouseReleased,
        //     Mouse = new MouseEventData { Button = MouseButton.Button3, }
        // };
        RunHookAndWaitForStart(hook, testProvider);

        sut.Run();
        testProvider.PostEvent(ref evtDn);
        // testProvider.PostEvent(ref evtUp);

        eventSimulatorMock.Received(1).SimulateKeyPress(Arg.Is(KeyCode.VcW));
        eventSimulatorMock.Received(2).SimulateKeyPress(Arg.Is(KeyCode.VcE));
        eventSimulatorMock.Received(2).SimulateKeyRelease(Arg.Is(KeyCode.VcE));
        eventSimulatorMock.Received(1).SimulateKeyRelease(Arg.Is(KeyCode.VcW));
    }

    [Test]
    public void NonDefaultProfileNotSendKeys()
    {
        var pf = GetProfilesService(GetSeedData());
        pf.Profiles.First(x => !x.IsDefault).MouseButton3 = new SimulatedKeystrokes
        {
            SimulatedKeystrokesType = new MouseButtonPressedActionType(),
            BlockOriginalMouseInput = true,
            Keys = "wee",
        };
        var testProvider = new TestProvider();
        using var hook = new SimpleReactiveGlobalHook(globalHookProvider: testProvider);
        var curWinSvc = Substitute.For<ICurrentWindowService>();
        curWinSvc.ForegroundWindow.Returns("U can't touch this");
        var mouseListener = new MouseListener(hook, pf, curWinSvc);
        var skipProfileService = GetSkipProfileService();
        var eventSimulatorMock = Substitute.For<IEventSimulator>();
        var eventSimulatorSvc = new EventSimulatorService(eventSimulatorMock);
        var sut = new KeyboardSimulatorWorker(
            pf,
            mouseListener,
            skipProfileService,
            new AsMouseButtonPressedService(eventSimulatorSvc),
            new AsMouseButtonReleasedService(eventSimulatorSvc),
            new DuringMousePressAndReleaseService(eventSimulatorSvc),
            new RepeatedWhileButtonDownService(eventSimulatorSvc),
            new StickyRepeatService(eventSimulatorSvc),
            new StickyHoldService(eventSimulatorSvc),
            new RightClick(eventSimulatorSvc)
        );

        var evtDn = new UioHookEvent
        {
            Type = EventType.MousePressed,
            Mouse = new MouseEventData { Button = MouseButton.Button3 },
        };
        // var evtUp = new UioHookEvent
        // {
        //     Type = EventType.MouseReleased,
        //     Mouse = new MouseEventData { Button = MouseButton.Button3, }
        // };
        RunHookAndWaitForStart(hook, testProvider);

        sut.Run();
        testProvider.PostEvent(ref evtDn);
        // testProvider.PostEvent(ref evtUp);

        eventSimulatorMock.DidNotReceive().SimulateKeyPress(Arg.Any<KeyCode>());
        eventSimulatorMock.DidNotReceive().SimulateKeyRelease(Arg.Any<KeyCode>());
    }

    [Test]
    public void DefaultProfileSendsKeysReleased()
    {
        var pf = GetProfilesService(GetSeedData());
        pf.CurrentProfile!.MouseButton3 = new SimulatedKeystrokes
        {
            SimulatedKeystrokesType = new MouseButtonReleasedActionType(),
            BlockOriginalMouseInput = true,
            Keys = "wee",
        };
        var testProvider = new TestProvider();
        using var hook = new SimpleReactiveGlobalHook(globalHookProvider: testProvider);
        var curWinSvc = Substitute.For<ICurrentWindowService>();
        var mouseListener = new MouseListener(hook, pf, curWinSvc);
        var skipProfileSvc = Substitute.For<ISkipProfileService>();
        var eventSimulatorMock = Substitute.For<IEventSimulator>();
        var eventSimulatorSvc = new EventSimulatorService(eventSimulatorMock);
        var sut = new KeyboardSimulatorWorker(
            pf,
            mouseListener,
            skipProfileSvc,
            new AsMouseButtonPressedService(eventSimulatorSvc),
            new AsMouseButtonReleasedService(eventSimulatorSvc),
            new DuringMousePressAndReleaseService(eventSimulatorSvc),
            new RepeatedWhileButtonDownService(eventSimulatorSvc),
            new StickyRepeatService(eventSimulatorSvc),
            new StickyHoldService(eventSimulatorSvc),
            new RightClick(eventSimulatorSvc)
        );

        var evtUp = new UioHookEvent
        {
            Type = EventType.MouseReleased,
            Mouse = new MouseEventData { Button = MouseButton.Button3 },
        };
        RunHookAndWaitForStart(hook, testProvider);

        sut.Run();
        testProvider.PostEvent(ref evtUp);

        eventSimulatorMock.Received(1).SimulateKeyPress(Arg.Is(KeyCode.VcW));
        eventSimulatorMock.Received(2).SimulateKeyPress(Arg.Is(KeyCode.VcE));
        eventSimulatorMock.Received(2).SimulateKeyRelease(Arg.Is(KeyCode.VcE));
        eventSimulatorMock.Received(1).SimulateKeyRelease(Arg.Is(KeyCode.VcW));
    }

    // [Test]
    // public void DefaultProfileSendsKeysReleased()
    // {
    //     var pf = GetProfilesService(GetSeedData());
    //     pf.CurrentProfile!.MouseButton3 = new SimulatedKeystrokes
    //     {
    //         // SimulatedKeystrokesType = new(),
    //         BlockOriginalMouseInput = true,
    //         Keys = "wee"
    //     };
    //     var testProvider = new TestProvider();
    //     using var hook = new SimpleReactiveGlobalHook(globalHookProvider: testProvider);
    //     var curWinSvc = Substitute.For<ICurrentWindowService>();
    //     var mouseListener = new MouseListener(hook, pf, curWinSvc);
    //     var skipProfileSvc = Substitute.For<ISkipProfileService>();
    //     var eventSimulatorMock = Substitute.For<IEventSimulator>();
    //     var eventSimulatorSvc = new EventSimulatorService(eventSimulatorMock);
    //     var sut = new KeyboardSimulatorWorker(
    //         pf,
    //         mouseListener,
    //         skipProfileSvc,
    //         new AsMouseButtonPressedService(eventSimulatorSvc),
    //         new AsMouseButtonReleasedService(eventSimulatorSvc),
    //         new DuringMousePressAndReleaseService(eventSimulatorSvc),
    //         new RepeatedWhileButtonDownService(eventSimulatorSvc),
    //         new StickyRepeatService(eventSimulatorSvc),
    //         new StickyHoldService(eventSimulatorSvc),
    //         new RightClick(eventSimulatorSvc)
    //     );
    //
    //     var evtUp = new UioHookEvent
    //     {
    //         Type = EventType.MouseReleased,
    //         Mouse = new MouseEventData { Button = MouseButton.Button3, }
    //     };
    //     RunHookAndWaitForStart(hook, testProvider);
    //
    //     sut.Run();
    //     testProvider.PostEvent(ref evtUp);
    //
    //     eventSimulatorMock.Received(1).SimulateKeyPress(Arg.Is(KeyCode.VcW));
    //     eventSimulatorMock.Received(2).SimulateKeyPress(Arg.Is(KeyCode.VcE));
    //     eventSimulatorMock.Received(2).SimulateKeyRelease(Arg.Is(KeyCode.VcE));
    //     eventSimulatorMock.Received(1).SimulateKeyRelease(Arg.Is(KeyCode.VcW));
    // }

    private static ISkipProfileService GetSkipProfileService()
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600))
        {
            return new SkipProfileService();
        }
        else if (OperatingSystem.IsMacOS())
        {
            // return new YMouseButtonControl.Services.MacOS.SkipProfileService();
        }
        else
        {
            // return new YMouseButtonControl.Services.Linux.SkipProfileService();
        }

        throw new Exception("Bad state");
    }

    private static void RunHookAndWaitForStart(IReactiveGlobalHook hook, TestProvider provider)
    {
        hook.RunAsync();

        while (!provider.IsRunning)
        {
            Thread.Yield();
        }
    }
}
