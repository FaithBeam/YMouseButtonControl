using NSubstitute;
using NUnit.Framework;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

public class RouteButtonMappingServiceTests
{
    [Test]
    public void Route()
    {
        var sksM = Substitute.For<ISimulatedKeystrokesService>();
        var mcsM = Substitute.For<IMouseCoordinatesService>();
        var rbms = new RouteButtonMappingService(sksM, mcsM);
        var mapping = new SimulatedKeystrokes();
        
        rbms.Route(mapping, MouseButtonState.Pressed);

        sksM.ReceivedWithAnyArgs(1).SimulatedKeystrokes(default, default);
    }
}