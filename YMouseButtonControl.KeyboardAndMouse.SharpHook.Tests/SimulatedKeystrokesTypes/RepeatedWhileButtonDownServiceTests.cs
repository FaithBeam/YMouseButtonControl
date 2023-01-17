using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests.SimulatedKeystrokesTypes;

[TestClass]
public class RepeatedWhileButtonDownServiceTests
{
    private AutoMocker _autoMocker;

    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void RepeatWhileDown()
    {
        var keys = "abc";
        var mapping = new SimulatedKeystrokes() { Keys = keys };
        var state = MouseButtonState.Pressed;
        _autoMocker.Setup<ISimulateKeyService>(x => x.TapKeys(keys)).Verifiable();
        var rwbds = _autoMocker.CreateInstance<RepeatedWhileButtonDownService>();
        rwbds.RepeatWhileDown(mapping, state);
        // Need to wait for the thread to send keys. Is there a better way to do this?
        Thread.Sleep(50);
        rwbds.RepeatWhileDown(mapping, MouseButtonState.Released);
        _autoMocker.VerifyAll();
    }
}