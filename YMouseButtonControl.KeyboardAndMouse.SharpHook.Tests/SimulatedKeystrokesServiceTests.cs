using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;
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
    [DataRow("abc", "a", "b", "c")]
    public void SimulatedKeystrokes_StickyHoldPress(string keys, params string[] expected)
    {
        var args = new List<string>();
        _autoMocker.Setup<IParseKeysService, List<string>>(x => x.ParseKeys(keys))
            .Returns(expected.ToList());
        _autoMocker.Setup<ISimulateKeyService, SimulateKeyboardResult>(x => x.SimulateKeyPress(Capture.In(args)))
            .Returns(new SimulateKeyboardResult { Result = "Successful" });
        var simulatedKeystrokesService = _autoMocker.CreateInstance<SimulatedKeystrokesService>();

        var buttonMapping = new SimulatedKeystrokes
        {
            Keys = keys,
            SimulatedKeystrokesType = new StickyHoldActionType(),
            State = false
        };
        simulatedKeystrokesService.SimulatedKeystrokes(buttonMapping, true);
        CollectionAssert.AreEqual(expected, args);
    }
    
    [TestMethod]
    [DataRow("abc", "c", "b", "a")]
    public void SimulatedKeystrokes_StickyHoldRelease(string keys, params string[] expected)
    {
        var args = new List<string>();
        _autoMocker.Setup<IParseKeysService, List<string>>(x => x.ParseKeys(keys))
            .Returns(keys.Select(x => x.ToString()).ToList());
        _autoMocker.Setup<ISimulateKeyService, SimulateKeyboardResult>(x => x.SimulateKeyRelease(Capture.In(args)))
            .Returns(new SimulateKeyboardResult { Result = "Successful" });
        var simulatedKeystrokesService = _autoMocker.CreateInstance<SimulatedKeystrokesService>();

        var buttonMapping = new SimulatedKeystrokes
        {
            Keys = keys,
            SimulatedKeystrokesType = new StickyHoldActionType(),
            State = true
        };
        simulatedKeystrokesService.SimulatedKeystrokes(buttonMapping, true);
        CollectionAssert.AreEqual(expected, args);
    }
}