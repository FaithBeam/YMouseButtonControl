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

public class StickyRepeatServiceTests
{
    [Test]
    public void StickyRepeat()
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
        var mapping = new SimulatedKeystrokes { Sequence = keys };
        var sksM = Substitute.For<ISimulateKeyService>();
        var sut = new StickyRepeatService(sksM);
        
        sut.StickyRepeat(mapping, MouseButtonState.Pressed);
        Thread.Sleep(50);
        sut.StickyRepeat(mapping, MouseButtonState.Pressed);
        
        sksM.ReceivedWithAnyArgs().TapKeys(default);
    }
}