using YMouseButtonControl.KeyboardAndMouse.Models;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

[TestClass]
public class ParseKeysServiceTests
{
    public static IEnumerable<object[]> Data
    {
        get
        {
            return new[]
            {
                new object[]
                {
                    "abc", new List<ParsedKey>
                    {
                        new() { Key = "a", IsModifier = false },
                        new() { Key = "b", IsModifier = false },
                        new() { Key = "c", IsModifier = false },
                    }
                },
                new object[]
                {
                    "{shift}w", new List<ParsedKey>
                    {
                        new() { Key = "shift", IsModifier = true },
                        new() { Key = "w", IsModifier = false }
                    }
                },
                new object[]
                {
                    "{shift}w{shift}e", new List<ParsedKey>()
                    {
                        new() { Key = "shift", IsModifier = true },
                        new() { Key = "w", IsModifier = false },
                        new() { Key = "shift", IsModifier = true },
                        new() { Key = "e", IsModifier = false },
                    }
                },
                new object[]
                {
                    "{SHIFT}W", new List<ParsedKey>()
                    {
                        new() { Key = "shift", IsModifier = true },
                        new() { Key = "w", IsModifier = false },
                    }
                },
                new object[]
                {
                    "w", new List<ParsedKey>()
                    {
                        new() { Key = "w", IsModifier = false },
                    }
                },
            };
        }
    }
    
    [TestMethod]
    [DynamicData(nameof(Data))]
    public void TestParseKeys(string keys, List<ParsedKey> expected)
    {
        var pks = new ParseKeysService();
        var actual = pks.ParseKeys(keys);
        CollectionAssert.AreEquivalent(expected, actual);
    }
}