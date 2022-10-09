using YMouseButtonControl.KeyboardAndMouse.SharpHook.Services;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests.Services;

[TestClass]
public class ParseKeysServiceTests
{
    [TestMethod]
    public void TestParseKeys()
    {
        var keys = "abc";
        var expected = new List<string> { "a", "b", "c" };
        CollectionAssert.AreEqual(ParseKeysService.ParseKeys(keys), expected);

        keys = "{shift}w";
        expected = new List<string>{ "shift", "w" };
        CollectionAssert.AreEqual(ParseKeysService.ParseKeys(keys), expected);
        
        keys = "{SHIFT}W";
        expected = new List<string>{ "shift", "w" };
        CollectionAssert.AreEqual(ParseKeysService.ParseKeys(keys), expected);
        
        keys = "w";
        expected = new List<string>{ "w" };
        CollectionAssert.AreEqual(ParseKeysService.ParseKeys(keys), expected);
    }
}