using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests.SimulatedKeystrokesTypes;

[TestClass]
public class AsMouseButtonPressedServiceTests
{
    private AutoMocker _autoMocker;

    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void AsMouseButtonPressed_NotPressed()
    {
        var mapping = new DisabledMapping();
        var state = MouseButtonState.Released;
        _autoMocker.Setup<ISimulateKeyService>(x => x.TapKeys(It.IsAny<string>())).Verifiable();
        var ambps = _autoMocker.CreateInstance<AsMouseButtonPressedService>();
        ambps.AsMouseButtonPressed(mapping, state);
        var sksMock = _autoMocker.GetMock<ISimulateKeyService>();
        sksMock.Verify(x => x.TapKeys(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void AsMouseButtonPressed_Pressed()
    {
        var keys = "abc";
        var mapping = new SimulatedKeystrokes() { Keys = keys };
        var state = MouseButtonState.Pressed;
        _autoMocker.Setup<ISimulateKeyService>(x => x.TapKeys(keys)).Verifiable();
        var ambps = _autoMocker.CreateInstance<AsMouseButtonPressedService>();
        ambps.AsMouseButtonPressed(mapping, state);
        _autoMocker.VerifyAll();
    }
}
