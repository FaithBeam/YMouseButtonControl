using Moq;
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
        var sks = _autoMocker.CreateInstance<SimulateKeyService>();
        sks.SimulateKeyRelease(key);
        _autoMocker.VerifyAll();
    }

    [TestMethod]
    public void SimulateKeyTap()
    {
        var key = "a";
        var kc = KeyCodeMappings.KeyCodes[key];
        _autoMocker.Use<IEventSimulator>(x => x.SimulateKeyPress(kc) == UioHookResult.Success);
        _autoMocker.Use<IEventSimulator>(x => x.SimulateKeyRelease(kc) == UioHookResult.Success);
        var sks = _autoMocker.CreateInstance<SimulateKeyService>();
        sks.SimulateKeyTap(key);
        _autoMocker.VerifyAll();
    }

    [TestMethod]
    public void PressKeys()
    {
        var args = new List<KeyCode>();
        _autoMocker.Setup<IEventSimulator, UioHookResult>(x => x.SimulateKeyPress(Capture.In(args)))
            .Returns(UioHookResult.Success);
        var keys = "abc";
        var expected = GetExpected(keys);
        var sks = _autoMocker.CreateInstance<SimulateKeyService>();
        sks.PressKeys(keys);
        CollectionAssert.AreEqual(expected, args);
    }
    
    [TestMethod]
    public void ReleaseKeys()
    {
        var args = new List<KeyCode>();
        _autoMocker.Setup<IEventSimulator, UioHookResult>(x => x.SimulateKeyRelease(Capture.In(args)))
            .Returns(UioHookResult.Success);
        var keys = "abc";
        var expected = GetExpected(keys);
        expected.Reverse();
        var sks = _autoMocker.CreateInstance<SimulateKeyService>();
        sks.ReleaseKeys(keys);
        CollectionAssert.AreEqual(expected, args);
    }

    private KeyCode GetKeyCode(string k)
    {
        return KeyCodeMappings.KeyCodes[k];
    }

    private List<KeyCode> GetExpected(string keys)
    {
        return keys.Select(x => GetKeyCode(x.ToString())).ToList();
    }
}