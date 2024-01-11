using NSubstitute;
using NUnit.Framework;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

public class SimulatedKeystrokesServiceTests
{
    [Test]
    public void SimulatedKeystrokes_StickyHold()
    {
        var mapping = new SimulatedKeystrokes
        {
            SimulatedKeystrokesType = new StickyHoldActionType()
        };
        var pressed = MouseButtonState.Pressed;
        var simKeyServiceM = Substitute.For<ISimulateKeyService>();
        var shsM = Substitute.For<IStickyHoldService>();
        var ampsM = Substitute.For<IAsMouseButtonPressedService>();
        var amrsM = Substitute.For<IAsMouseButtonReleasedService>();
        var dparsM = Substitute.For<IDuringMousePressAndReleaseService>();
        var rwdsM = Substitute.For<IRepeatedWhileButtonDownService>();
        var srsM = Substitute.For<IStickyRepeatService>();
        var sks = new SimulatedKeystrokesService(simKeyServiceM, shsM, ampsM, amrsM, dparsM, rwdsM, srsM);
        
        sks.SimulatedKeystrokes(mapping, pressed);
        
        shsM.ReceivedWithAnyArgs(1).StickyHold(default, default);
    }
}