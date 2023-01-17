using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests.SimulatedKeystrokesTypes;

[TestClass]
public class StickyHoldServiceTests
{
    private AutoMocker _autoMocker;

    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void StickyHold_Press()
    {
        var keys = "abc";
        var state = false;
        var pressed = MouseButtonState.Pressed;
        var mapping = new SimulatedKeystrokes
        {
            Keys = keys,
            State = state
        };
        _autoMocker
            .Setup<ISimulateKeyService>(x => x.PressKeys(keys))
            .Verifiable();
        var shs = _autoMocker.CreateInstance<StickyHoldService>();
        shs.StickyHold(mapping, pressed);
        _autoMocker.VerifyAll();
        Assert.IsTrue(mapping.State);
    }
    
    [TestMethod]
    public void StickyHold_Release()
    {
        var keys = "abc";
        var state = true;
        var pressed = MouseButtonState.Pressed;
        var mapping = new SimulatedKeystrokes
        {
            Keys = keys,
            State = state
        };
        _autoMocker
            .Setup<ISimulateKeyService>(x => x.ReleaseKeys(keys))
            .Verifiable();
        var shs = _autoMocker.CreateInstance<StickyHoldService>();
        shs.StickyHold(mapping, pressed);
        _autoMocker.VerifyAll();
        Assert.IsFalse(mapping.State);
    }
}