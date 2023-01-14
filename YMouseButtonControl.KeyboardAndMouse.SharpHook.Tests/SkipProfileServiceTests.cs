using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

[TestClass]
public class SkipProfileServiceTests
{
    private AutoMocker _autoMocker;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }
    
    [TestMethod]
    public void TestShouldSkipProfile()
    {
        var skipProfilesService = _autoMocker.CreateInstance<SkipProfileService>();
        
        var p = new Profile
        {
            Process = "*",
            Checked = true
        };
        Assert.IsFalse(skipProfilesService.ShouldSkipProfile(p));

        p = new Profile
        {
            Process = "*",
            Checked = false
        };
        Assert.IsTrue(skipProfilesService.ShouldSkipProfile(p));

        var testProc = "testprocess.exe";
        _autoMocker.Use<ICurrentWindowService>(x => x.ForegroundWindow == testProc);
        skipProfilesService = _autoMocker.CreateInstance<SkipProfileService>();
        
        // Test profile who's process doesn't match the current foreground window
        p = new Profile
        {
            Process = "something.exe",
            Checked = true
        };
        Assert.IsTrue(skipProfilesService.ShouldSkipProfile(p));
        
        // Test profile who's process matches the current foreground window
        p = new Profile
        {
            Process = testProc,
            Checked = true
        };
        Assert.IsFalse(skipProfilesService.ShouldSkipProfile(p));
    }
}