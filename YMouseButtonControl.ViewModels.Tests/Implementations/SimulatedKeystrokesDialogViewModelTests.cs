using System.Reactive.Linq;
using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Models.Models;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Models;
using FluentAssertions;


namespace YMouseButtonControl.ViewModels.Tests.Implementations;

public class SimulatedKeystrokesDialogViewModelTests
{
    [Test]
    public void TestInitialValue()
    {
        var skdvmt = new SimulatedKeystrokesDialogViewModel();
        
        Assert.That(skdvmt.CustomKeys, Is.EqualTo(""));
    }
    
    [Test]
    public void TestSplitButtonCommand_FromEmpty()
    {
        var skdvmt = new SimulatedKeystrokesDialogViewModel();
        const string input = "{}";
        const string expected = "{}";
        const int expectedCaretIdx = 2;
        
        skdvmt.SplitButtonCommand.Execute(input).Subscribe();
        
        Assert.Multiple(() =>
        {
            Assert.That(skdvmt.CustomKeys, Is.EqualTo(expected));
            Assert.That(skdvmt.CaretIndex, Is.EqualTo(expectedCaretIdx));
        });
    }

    [Test]
    public void TestSplitButtonCommand_FromPreviousData()
    {
        var skdvmt = new SimulatedKeystrokesDialogViewModel();
        skdvmt.CustomKeys = "{}";
        skdvmt.CaretIndex = 2;
        
        skdvmt.SplitButtonCommand.Execute("Shift").Subscribe();
        
        Assert.That(skdvmt.CustomKeys, Is.EqualTo("{}{SHIFT}"));
    }
    
    [Test]
    public async Task TestOkCommand()
    {
        var skdvmt = new SimulatedKeystrokesDialogViewModel
        {
            CustomKeys = "abc123"
        };
        var expected = new List<IParsedEvent>
        {
            new ParsedKey { Event = "a", KeyCode = KeyCode.VcA, IsModifier = false },
            new ParsedKey { Event = "b", KeyCode = KeyCode.VcB, IsModifier = false },
            new ParsedKey { Event = "c", KeyCode = KeyCode.VcC, IsModifier = false },
            new ParsedKey { Event = "1", KeyCode = KeyCode.Vc1, IsModifier = false },
            new ParsedKey { Event = "2", KeyCode = KeyCode.Vc2, IsModifier = false },
            new ParsedKey { Event = "3", KeyCode = KeyCode.Vc3, IsModifier = false },
        };
        skdvmt.Description = "my description";
        
        var result = await skdvmt.OkCommand.Execute();
        
        result.Events.Should().BeEquivalentTo(expected);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<SimulatedKeystrokesDialogModel>());
            Assert.That(result.Description, Is.EqualTo("my description"));
            Assert.That(result.SimulatedKeystrokesType, Is.InstanceOf<ISimulatedKeystrokesType>());
        });
    }
    
    [Test]
    public void TestSimulatedKeystrokesDialogViewModel()
    {
        var ib = new SimulatedKeystrokes
        {
            Sequence = new List<IParsedEvent>
            {
                new ParsedKey
                {
                    Event = "w",
                    KeyCode = KeyCode.VcW,
                    IsModifier = false
                }
            },
            PriorityDescription = "TEST"
        };

        var skdvmt = new SimulatedKeystrokesDialogViewModel(ib);
        
        Assert.Multiple(() =>
        {
            Assert.That(skdvmt.CustomKeys, Is.EqualTo("w"));
            Assert.That(skdvmt.Description, Is.EqualTo("TEST"));
        });
    }
}