using Moq.AutoMock;
using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

[TestClass]
public class SimulateKeyServiceTests
{
    private AutoMocker _autoMocker;

    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void TestSimulateKeyPress()
    {
        var key = "a";
        var keyCode = KeyCodeMappings.KeyCodes[key];

        _autoMocker.Use<IEventSimulator>(x => x.SimulateKeyPress(keyCode) == UioHookResult.Success);
        var simulateKeyService = _autoMocker.CreateInstance<SimulateKeyService>();
        simulateKeyService.SimulateKeyPress(key);
        _autoMocker.VerifyAll();
    }

    [TestMethod]
    public void TestSimulateKeyRelease()
    {
        var key = "a";
        var keyCode = KeyCodeMappings.KeyCodes[key];

        _autoMocker.Use<IEventSimulator>(x => x.SimulateKeyRelease(keyCode) == UioHookResult.Success);
        var simulateKeyService = _autoMocker.CreateInstance<SimulateKeyService>();
        simulateKeyService.SimulateKeyRelease(key);
        _autoMocker.VerifyAll();
    }
}