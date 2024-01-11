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

public class StickyHoldServiceTests
{
    [Test]
    public void StickyHold_Press()
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
        const bool state = false;
        var mapping = new SimulatedKeystrokes
        {
            Sequence = keys,
            State = state
        };
        var sksM = Substitute.For<ISimulateKeyService>();
        var sut = new StickyHoldService(sksM);
        
        sut.StickyHold(mapping, MouseButtonState.Pressed);
        
        sksM.ReceivedWithAnyArgs().PressKeys(default);
    }
    
    [Test]
    public void StickyHold_Release()
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
        const bool state = true;
        var mapping = new SimulatedKeystrokes
        {
            Sequence = keys,
            State = state
        };
        var sksM = Substitute.For<ISimulateKeyService>();
        var sut = new StickyHoldService(sksM);
        
        sut.StickyHold(mapping, MouseButtonState.Pressed);
        
        sksM.ReceivedWithAnyArgs().ReleaseKeys(default);
    }
}