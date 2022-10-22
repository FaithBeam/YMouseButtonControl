using Moq.AutoMock;
using YMouseButtonControl.Processes.Implementations;

namespace YMouseButtonControl.Processes.Tests;

[TestClass]
public class ProcessesServiceTests
{
    private readonly AutoMocker _autoMocker;

    public ProcessesServiceTests()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void TestGetProcesses()
    {
        var vm = _autoMocker.CreateInstance<ProcessesService>();
        var procs = vm.GetProcesses();
        Assert.IsTrue(procs.Any());
        foreach (var p in procs)
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(p.ProcessName));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(p.WindowTitle));
        }
    }
}