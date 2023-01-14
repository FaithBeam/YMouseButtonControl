using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

[TestClass]
public class RouteButtonMappingServiceTests
{
    private AutoMocker _autoMocker;

    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void Route()
    {
        var mapping = new SimulatedKeystrokes();
        _autoMocker.Setup<ISimulatedKeystrokesService>(x => x.SimulatedKeystrokes(mapping, true)).Verifiable();
        var rbms = _autoMocker.CreateInstance<RouteButtonMappingService>();
        rbms.Route(mapping, true);
        _autoMocker.VerifyAll();
    }
}