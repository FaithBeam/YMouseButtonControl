using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

[TestClass]
public class SimulatedKeystrokesServiceTests
{
    private AutoMocker _autoMocker;

    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void SimulatedKeystrokes_StickyHold()
    {
        var mapping = new SimulatedKeystrokes()
        {
            SimulatedKeystrokesType = new StickyHoldActionType()
        };
        var pressed = MouseButtonState.Pressed;
        _autoMocker.Setup<IStickyHoldService>(x => x.StickyHold(mapping, pressed)).Verifiable();
        var sks = _autoMocker.CreateInstance<SimulatedKeystrokesService>();
        sks.SimulatedKeystrokes(mapping, pressed);
        _autoMocker.VerifyAll();
    }
}