using Moq.AutoMock;
using YMouseButtonControl.Services.Windows.Implementations;

namespace YMouseButtonControl.Services.Windows.Tests;

[TestClass]
public class ProcessMonitorServiceTests
{
    private readonly AutoMocker _autoMocker;

    public ProcessMonitorServiceTests()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void TestGetProcesses()
    {
        var pms = _autoMocker.CreateInstance<ProcessMonitorService>();
        var procs = pms.RunningProcesses;
        Assert.IsTrue(procs.Any());
        foreach (var p in procs)
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(p.ProcessName));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(p.WindowTitle));
        }
    }
}