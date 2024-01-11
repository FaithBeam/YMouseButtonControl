using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Models.Models;
using FluentAssertions;

namespace YMouseButtonControl.DataAccess.Models.Tests;

public class ProfileTests
{
    public static IEnumerable<object[]> AreEquivalentData
    {
        get
        {
            return new[]
            {
                new object[]
                {
                    new Profile
                    {
                        Name = "MyName",
                        MouseButton1 = new SimulatedKeystrokes
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
                            SimulatedKeystrokesType = new StickyHoldActionType(),
                        }
                    },
                    new Profile
                    {
                        Name = "MyName",
                        MouseButton1 = new SimulatedKeystrokes
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
                            SimulatedKeystrokesType = new StickyHoldActionType(),
                        }
                    }
                }
            };
        }
    }

    [Test]
    [TestCaseSource(nameof(AreEquivalentData))]
    public void TestProfile_AreEquivalent(Profile p1, Profile p2)
    {
        p1.Should().BeEquivalentTo(p2);
    }
    
    public static IEnumerable<object[]> AreNotEquivalentData
    {
        get
        {
            return new[]
            {
                new object[]
                {
                    new Profile
                    {
                        Name = "MyName",
                        MouseButton1 = new SimulatedKeystrokes
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
                            SimulatedKeystrokesType = new StickyHoldActionType(),
                        }
                    },
                    new Profile
                    {
                        Name = "MyName",
                        MouseButton2 = new SimulatedKeystrokes
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
                            SimulatedKeystrokesType = new StickyHoldActionType(),
                        }
                    }
                },
            };
        }
    }

    [Test]
    [TestCaseSource(nameof(AreNotEquivalentData))]
    public void TestProfile_AreNotEquivalent(Profile p1, Profile p2)
    {
        p1.Should().NotBeEquivalentTo(p2);
    }
}