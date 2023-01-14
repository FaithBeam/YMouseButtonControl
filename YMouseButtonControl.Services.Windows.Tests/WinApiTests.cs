using YMouseButtonControl.Services.Windows.Implementations;
using YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

namespace YMouseButtonControl.Services.Windows.Tests;

// Doesn't really test anything other than me testing PInvoke
[TestClass]
public class WinApiTests
{
    [TestMethod]
    public void TestGetHandleFromProcessId()
    {
        var winApi = new WinApi();
        // var handle = winApi.GetHWndFromProcessId("2208");
        //
        // var newHandle = winApi.GetHWndFromProcessId(2208);
        ;
    }

    [TestMethod]
    public void TestGetBitmapPathFromProcessId()
    {
        var winApi = new WinApi();
        var ico = winApi.GetBitmapPathFromProcessId("23104");

        var newIco = winApi.GetBitmapPathFromProcessId(uint.Parse("23104"));
        ;
    }

    [TestMethod]
    public void TestGetBitmapFromPath()
    {
        var winApi = new WinApi();
        var ico = winApi.GetBitmapFromPath(@"C:\Program Files\Streamlink Twitch GUI\streamlink-twitch-gui.exe");
        ;
    }
}