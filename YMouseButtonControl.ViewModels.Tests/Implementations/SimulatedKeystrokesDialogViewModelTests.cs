using System.Reactive.Linq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Factories;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Models;

namespace YMouseButtonControl.ViewModels.Tests.Implementations;

[TestClass]
public class SimulatedKeystrokesDialogViewModelTests
{
    private AutoMocker _autoMocker;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }
    
    [TestMethod]
    public void TestSplitButtonCommand()
    {
        var skdvmt = _autoMocker.CreateInstance<SimulatedKeystrokesDialogViewModel>();
        Assert.AreEqual("", skdvmt.CustomKeys);
        
        skdvmt.SplitButtonCommand.Execute(@"{}").Subscribe();
        Assert.AreEqual(@"{}", skdvmt.CustomKeys);
        Assert.AreEqual(2, skdvmt.CaretIndex);

        skdvmt.CustomKeys = string.Empty;
        skdvmt.CustomKeys = @"{}";
        skdvmt.CaretIndex = 2;
        skdvmt.SplitButtonCommand.Execute(@"Shift").Subscribe();
        Assert.AreEqual(@"{}{SHIFT}", skdvmt.CustomKeys);
    }

    [TestMethod]
    public async Task TestOkCommand()
    {
        // Test default case
        var skdvmt = _autoMocker.CreateInstance<SimulatedKeystrokesDialogViewModel>();
        var result = await skdvmt.OkCommand.Execute();
        Assert.IsInstanceOfType(result, typeof(SimulatedKeystrokesDialogModel));
        Assert.AreEqual(string.Empty, result.CustomKeys);
        Assert.IsInstanceOfType(result.SimulatedKeystrokesType, typeof(ISimulatedKeystrokesType));
        Assert.AreEqual(string.Empty, result.Description);
        
        // Test more interesting case
        skdvmt = _autoMocker.CreateInstance<SimulatedKeystrokesDialogViewModel>();
        skdvmt.CustomKeys = "abc123";
        skdvmt.Description = "my description";
        result = await skdvmt.OkCommand.Execute();
        Assert.IsInstanceOfType(result, typeof(SimulatedKeystrokesDialogModel));
        Assert.AreEqual("abc123", result.CustomKeys);
        Assert.AreEqual("my description", result.Description);
        Assert.IsInstanceOfType(result.SimulatedKeystrokesType, typeof(ISimulatedKeystrokesType));
    }
}