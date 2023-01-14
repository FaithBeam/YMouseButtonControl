using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

[TestClass]
public class ParseKeysServiceTests
{
    [TestMethod]
    [DataRow("abc", "a", "b", "c")]
    [DataRow("{shift}w", "shift", "w")]
    [DataRow("{SHIFT}W", "shift", "w")]
    [DataRow("w", "w")]
    public void TestParseKeys(string keys, params string[] expected)
    {
        var pks = new ParseKeysService();
        
        CollectionAssert.AreEqual(expected, pks.ParseKeys(keys));
    }
}