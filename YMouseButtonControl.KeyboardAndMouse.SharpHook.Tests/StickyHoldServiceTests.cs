using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

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
        var pressed = true;
        var mapping = new SimulatedKeystrokes
        {
            Keys = keys,
            State = state
        };
        var pks = _autoMocker.CreateInstance<ParseKeysService>();
        var pksReturn = pks.ParseKeys(keys);
        _autoMocker
            .Setup<IParseKeysService, List<string>>(x => x.ParseKeys(keys))
            .Returns(pksReturn);
        _autoMocker
            .Setup<ISimulateKeyService>(x => x.PressKeys(pksReturn))
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
        var pressed = true;
        var mapping = new SimulatedKeystrokes
        {
            Keys = keys,
            State = state
        };
        var pks = _autoMocker.CreateInstance<ParseKeysService>();
        var pksReturn = pks.ParseKeys(keys);
        _autoMocker
            .Setup<IParseKeysService, List<string>>(x => x.ParseKeys(keys))
            .Returns(pksReturn);
        _autoMocker
            .Setup<ISimulateKeyService>(x => x.ReleaseKeys(pksReturn))
            .Verifiable();
        var shs = _autoMocker.CreateInstance<StickyHoldService>();
        shs.StickyHold(mapping, pressed);
        _autoMocker.VerifyAll();
        Assert.IsFalse(mapping.State);
    }
}