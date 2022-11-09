using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;

namespace YMouseButtonControl.ViewModels.Tests.Implementations;

[TestClass]
public class LayerViewModelTests
{
    private readonly AutoMocker _autoMocker;

    public LayerViewModelTests()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void TestChangeCurrentMouseButton()
    {
        // Test changing mb1 to disabled mapping
        var lvm = _autoMocker.CreateInstance<LayerViewModel>();
        lvm.CurrentMouseButton1ComboIndex = 1;
        var cpom = _autoMocker.GetMock<ICurrentProfileOperationsMediator>();
        cpom.Verify(x => x.UpdateMouseButton1(It.IsAny<DisabledMapping>()), Times.Once);

        lvm = _autoMocker.CreateInstance<LayerViewModel>();
        lvm.CurrentMouseButton1ComboIndex = 2;
        var skdvm = _autoMocker.GetMock<SimulatedKeystrokesDialogViewModel>();
        skdvm.Verify();
    }
}