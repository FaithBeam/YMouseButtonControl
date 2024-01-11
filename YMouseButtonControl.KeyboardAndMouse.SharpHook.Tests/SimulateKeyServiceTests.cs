using System.Text.RegularExpressions;
using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Models.Models;
using YMouseButtonControl.KeyboardAndMouse.Models;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

public class SimulateKeyServiceTests
{
    private Fixture _fixture = new();

    [Test]
    public void TestSimulateMousePress()
    {
        const MouseButton mb = MouseButton.Button1;
        var esM = Substitute.For<IEventSimulator>();
        var sks = new SimulateKeyService(esM);
        
        var result = sks.SimulateMousePress(mb);

        Assert.That(result, Is.EqualTo(UioHookResult.Success));
        esM.ReceivedWithAnyArgs(1).SimulateMousePress(default);
    }
    
    [Test]
    public void TestSimulateMouseRelease()
    {
        const MouseButton mb = MouseButton.Button1;
        var esM = Substitute.For<IEventSimulator>();
        var sks = new SimulateKeyService(esM);
        
        var result = sks.SimulateMouseRelease(mb);

        Assert.That(result, Is.EqualTo(UioHookResult.Success));
        esM.ReceivedWithAnyArgs(1).SimulateMouseRelease(default);
    }

    [Test]
    public void TestSimulateKeyPress()
    {
        var keyCode = _fixture.Create<KeyCode>();
        var esM = Substitute.For<IEventSimulator>();
        var sks = new SimulateKeyService(esM);
        
        var result = sks.SimulateKeyPress(keyCode);

        Assert.That(result.Result, Is.EqualTo("Success"));
        esM.Received(1).SimulateKeyPress(Arg.Is(keyCode));
    }

    [Test]
    public void TestSimulateKeyRelease()
    {
        var keyCode = _fixture.Create<KeyCode>();
        var esM = Substitute.For<IEventSimulator>();
        var sks = new SimulateKeyService(esM);
        
        var result = sks.SimulateKeyRelease(keyCode);

        Assert.That(result.Result, Is.EqualTo("Success"));
        esM.Received(1).SimulateKeyRelease(Arg.Is(keyCode));
    }

    [Test]
    public void SimulateKeyTap()
    {
        const KeyCode keyCode = KeyCode.VcW;
        const string key = "w";
        var esM = Substitute.For<IEventSimulator>();
        var sks = new SimulateKeyService(esM);
        
        var result = sks.SimulateKeyTap(key);

        Assert.That(result.Result, Is.EqualTo("Success"));
        esM.Received(1).SimulateKeyPress(Arg.Is(keyCode));
        esM.Received(1).SimulateKeyRelease(Arg.Is(keyCode));
    }

    [Test]
    public void PressKeys()
    {
        var args = new List<KeyCode>();
        var esM = Substitute.For<IEventSimulator>();
        esM.When(x => x.SimulateKeyPress(Arg.Any<KeyCode>()))
            .Do(x => args.Add(x.ArgAt<KeyCode>(0)));
        var sks = new SimulateKeyService(esM);
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
        var expected = new List<KeyCode> { KeyCode.VcA, KeyCode.VcB, KeyCode.VcC };
        
        sks.PressKeys(keys);
        
        CollectionAssert.AreEqual(expected, args);
    }

    [Test]
    public void PressKeys_MouseButton()
    {
        var events = new List<IParsedEvent>
        {
            new ParsedMouseButton
            {
                MouseButton = MouseButton.Button1
            }
        };
        var args = new List<MouseButton>();
        var esM = Substitute.For<IEventSimulator>();
        esM.When(x => x.SimulateMousePress(Arg.Any<MouseButton>()))
            .Do(x => args.Add(x.ArgAt<MouseButton>(0)));
        var expected = new List<MouseButton> { MouseButton.Button1 };
        var sks = new SimulateKeyService(esM);
        
        sks.PressKeys(events);
        
        CollectionAssert.AreEqual(expected, args);
    }
    
    [Test]
    public void ReleaseKeys()
    {
        var args = new List<KeyCode>();
        var esM = Substitute.For<IEventSimulator>();
        esM.When(x => x.SimulateKeyRelease(Arg.Any<KeyCode>()))
            .Do(x => args.Add(x.ArgAt<KeyCode>(0)));
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
        var expected = new List<KeyCode> { KeyCode.VcA, KeyCode.VcB, KeyCode.VcC };
        expected.Reverse();
        var sks = new SimulateKeyService(esM);
        
        sks.ReleaseKeys(keys);
        
        CollectionAssert.AreEqual(expected, args);
    }

    [Test]
    public void ReleaseKeys_MouseButton()
    {
        var events = new List<IParsedEvent>
        {
            new ParsedMouseButton
            {
                MouseButton = MouseButton.Button1
            }
        };
        var args = new List<MouseButton>();
        var esM = Substitute.For<IEventSimulator>();
        esM.When(x => x.SimulateMouseRelease(Arg.Any<MouseButton>()))
            .Do(x => args.Add(x.ArgAt<MouseButton>(0)));
        var expected = new List<MouseButton> { MouseButton.Button1 };
        var sks = new SimulateKeyService(esM);
        
        sks.ReleaseKeys(events);
        
        CollectionAssert.AreEqual(expected, args);
    }
}