using AutoFixture.NUnit3;
using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Models.Models;
using FluentAssertions;

namespace YMouseButtonControl.KeyboardAndMouse.Tests;

public class ParseKeysHelperTests
{
    [SetUp]
    public void Setup()
    {
    }

    private static readonly object[] SourceList =
    {
        new object[]
        {
            "w",
            new List<IParsedEvent>{new ParsedKey{Event = "w", KeyCode = KeyCode.VcW, IsModifier = false}}
        },
        new object[]
        {
            "{SHIFT}w",
            new List<IParsedEvent>
            {
                new ParsedKey{Event = "{shift}", KeyCode = KeyCode.VcLeftShift, IsModifier = true},
                new ParsedKey{Event = "w", KeyCode = KeyCode.VcW, IsModifier = false}
            }
        },
        new object[]
        {
            "{SHIFT}w{SHIFT}e",
            new List<IParsedEvent>
            {
                new ParsedKey{Event = "{shift}", KeyCode = KeyCode.VcLeftShift, IsModifier = true},
                new ParsedKey{Event = "w", KeyCode = KeyCode.VcW, IsModifier = false},
                new ParsedKey{Event = "{shift}", KeyCode = KeyCode.VcLeftShift, IsModifier = true},
                new ParsedKey{Event = "e", KeyCode = KeyCode.VcE, IsModifier = false},
            }
        },
        new object[]
        {
            "{ALT}{F4}",
            new List<IParsedEvent>
            {
                new ParsedKey{Event = "{alt}", KeyCode = KeyCode.VcLeftAlt, IsModifier = true},
                new ParsedKey{Event = "{f4}", KeyCode = KeyCode.VcF4, IsModifier = false}
            }
        },
        new object[]
        {
            "{MADD:1920,1080}",
            new List<IParsedEvent>
            {
                new ParsedMouseMovement("{madd:1920,1080}"),
            }
        },
    };
    
    [Test]
    [TestCaseSource(nameof(SourceList))]
    public void TestParseKeys(string input, List<IParsedEvent> expected)
    {
        var result = ParseKeysUtility.ParseKeys(input);
        result.Should().BeEquivalentTo(expected);
    }
}