using NSubstitute;
using NUnit.Framework;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

public class SkipProfileServiceTests
{
    [Test]
    public void TestShouldSkipProfile_False()
    {
        var cwsM = Substitute.For<ICurrentWindowService>();
        var sut = new SkipProfileService(cwsM);
        var p = new Profile
        {
            Process = "*",
            Checked = true
        };

        var result = sut.ShouldSkipProfile(p);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void TestShouldSkipProfile_True()
    {
        var cwsM = Substitute.For<ICurrentWindowService>();
        var sut = new SkipProfileService(cwsM);
        var p = new Profile
        {
            Process = "*",
            Checked = false
        };

        var result = sut.ShouldSkipProfile(p);
        
        Assert.That(result);
    }
    
    [Test]
    public void TestShouldSkipProfile_DoesNotMatchForeGroundWindow()
    {
        const string testProc = "testprocess.exe";
        var cwsM = Substitute.For<ICurrentWindowService>();
        cwsM.ForegroundWindow.Returns(testProc);
        var sut = new SkipProfileService(cwsM);
        var p = new Profile
        {
            Process = "*",
            Checked = false
        };

        var result = sut.ShouldSkipProfile(p);
        
        Assert.That(result);
    }
    
    [Test]
    public void TestShouldSkipProfile_MatchesForeGroundWindow()
    {
        const string testProc = "testprocess.exe";
        var cwsM = Substitute.For<ICurrentWindowService>();
        cwsM.ForegroundWindow.Returns(testProc);
        var sut = new SkipProfileService(cwsM);
        var p = new Profile
        {
            Process = testProc,
            Checked = true
        };

        var result = sut.ShouldSkipProfile(p);
        
        Assert.That(result, Is.False);
    }
}