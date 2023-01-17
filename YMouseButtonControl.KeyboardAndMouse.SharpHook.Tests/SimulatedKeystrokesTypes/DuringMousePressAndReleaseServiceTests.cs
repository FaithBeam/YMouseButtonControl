using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests.SimulatedKeystrokesTypes;

[TestClass]
public class DuringMousePressAndReleaseServiceTests
{
    private AutoMocker _autoMocker;

    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void DuringMousePressAndRelease_Press()
    {
        var keys = "abc";
        var mapping = new SimulatedKeystrokes() { Keys = keys };
        var state = MouseButtonState.Pressed;
        _autoMocker.Setup<ISimulateKeyService>(x => x.PressKeys(keys)).Verifiable();
        var dmpars = _autoMocker.CreateInstance<DuringMousePressAndReleaseService>();
        dmpars.DuringMousePressAndRelease(mapping, state);
        _autoMocker.VerifyAll();
    }
    
    [TestMethod]
    public void DuringMousePressAndRelease_Release()
    {
        var keys = "abc";
        var mapping = new SimulatedKeystrokes() { Keys = keys };
        var state = MouseButtonState.Released;
        _autoMocker.Setup<ISimulateKeyService>(x => x.ReleaseKeys(keys)).Verifiable();
        var dmpars = _autoMocker.CreateInstance<DuringMousePressAndReleaseService>();
        dmpars.DuringMousePressAndRelease(mapping, state);
        _autoMocker.VerifyAll();
    }
}