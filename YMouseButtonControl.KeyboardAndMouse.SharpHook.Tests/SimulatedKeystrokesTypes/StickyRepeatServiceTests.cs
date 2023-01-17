using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests.SimulatedKeystrokesTypes;

[TestClass]
public class StickyRepeatServiceTests
{
    private AutoMocker _autoMocker;

    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void StickyRepeat()
    {
        var keys = "abc";
        var mapping = new SimulatedKeystrokes() { Keys = keys };
        _autoMocker.Setup<ISimulateKeyService>(x => x.TapKeys(keys)).Verifiable();
        var srs = _autoMocker.CreateInstance<StickyRepeatService>();
        srs.StickyRepeat(mapping, MouseButtonState.Pressed);
        Thread.Sleep(50);
        srs.StickyRepeat(mapping, MouseButtonState.Released);
        _autoMocker.VerifyAll();
    }
}