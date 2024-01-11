using NSubstitute;
using NUnit.Framework;
using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Models.Models;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests.SimulatedKeystrokesTypes;

public class AsMouseButtonPressedServiceTests
{
    [Test]
    public void AsMouseButtonPressed_NotPressed()
    {
        var mapping = new SimulatedKeystrokes();
        const MouseButtonState state = MouseButtonState.Released;
        var sksM = Substitute.For<ISimulateKeyService>();
        var sut = new AsMouseButtonPressedService(sksM);
        
        sut.AsMouseButtonPressed(mapping, state);
        
        sksM.DidNotReceiveWithAnyArgs().TapKeys(default);
    }

    [Test]
    public void AsMouseButtonPressed_Pressed()
    {
        var keys = new List<IParsedEvent>
        {
            new ParsedKey
            {
                Event = "a",
                IsModifier = false,
                KeyCode = KeyCode.VcA
            },
            new ParsedKey
            {
                Event = "b",
                IsModifier = false,
                KeyCode = KeyCode.VcB
            },
            new ParsedKey
            {
                Event = "c",
                IsModifier = false,
                KeyCode = KeyCode.VcC
            },
        };
        var mapping = new SimulatedKeystrokes
        {
            Sequence = keys
        };
        const MouseButtonState state = MouseButtonState.Pressed;
        var sksM = Substitute.For<ISimulateKeyService>();
        var sut = new AsMouseButtonPressedService(sksM);
        
        sut.AsMouseButtonPressed(mapping, state);
        
        sksM.ReceivedWithAnyArgs(1).TapKeys(default);
    }
}