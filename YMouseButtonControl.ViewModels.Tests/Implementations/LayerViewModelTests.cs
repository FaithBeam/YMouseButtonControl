using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Repositories;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Services;

namespace YMouseButtonControl.ViewModels.Tests.Implementations;

[TestClass]
public class LayerViewModelTests
{
    private AutoMocker? _autoMocker;
    private LayerViewModel? _lvm;
    private Mock<IRepository<Profile>> _repoMock;
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IProfilesService> _psMock;


    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }
    
    [TestMethod]
    public void TestChangeMouseButtonIndex()
    {
        // Test changing mb1 to disabled mapping
        var dialog = _autoMocker.GetMock<IShowSimulatedKeystrokesDialogService>();
        var sk = new SimulatedKeystrokes { Keys = "w", SimulatedKeystrokesType = new StickyHoldActionType() };
        dialog.Setup(x => x.ShowSimulatedKeystrokesDialog()).ReturnsAsync(sk);
        _autoMocker.Use(dialog.Object);
        _autoMocker!.Use<IProfilesService>(x => x.CurrentProfile == new Profile());
        _lvm = _autoMocker.CreateInstance<LayerViewModel>();
        _lvm!.Mb1Index = 1;
        Assert.IsTrue(_lvm.Mb1Index == 1);
        Assert.IsTrue(_lvm.MouseButton1Combo[_lvm.Mb1Index] is DisabledMapping);
        dialog.Verify(x => x.ShowSimulatedKeystrokesDialog(), Times.Never);

        _lvm.Mb1Index = 2;
        Assert.IsTrue(_lvm.Mb1Index == 2);
        Assert.IsTrue(_lvm.MouseButton1Combo[_lvm.Mb1Index] is SimulatedKeystrokes);
        dialog.Verify(x => x.ShowSimulatedKeystrokesDialog(It.IsAny<IButtonMapping>()), Times.Once);

        _lvm.Mb2Index = 1;
        Assert.IsTrue(_lvm.Mb2Index == 1);
        Assert.IsTrue(_lvm.MouseButton2Combo[_lvm.Mb2Index] is DisabledMapping);
        
        _lvm.Mb3Index = 1;
        Assert.IsTrue(_lvm.Mb3Index == 1);
        Assert.IsTrue(_lvm.MouseButton3Combo[_lvm.Mb3Index] is DisabledMapping);
        
        _lvm.Mb4Index = 1;
        Assert.IsTrue(_lvm.Mb4Index == 1);
        Assert.IsTrue(_lvm.MouseButton4Combo[_lvm.Mb4Index] is DisabledMapping);
        
        _lvm.Mb5Index = 1;
        Assert.IsTrue(_lvm.Mb5Index == 1);
        Assert.IsTrue(_lvm.MouseButton5Combo[_lvm.Mb5Index] is DisabledMapping);
        
        _lvm.MwuIndex = 1;
        Assert.IsTrue(_lvm.MwuIndex == 1);
        Assert.IsTrue(_lvm.MouseWheelUpCombo[_lvm.MwuIndex] is DisabledMapping);
        
        _lvm.MwdIndex = 1;
        Assert.IsTrue(_lvm.MwdIndex == 1);
        Assert.IsTrue(_lvm.MouseWheelDownCombo[_lvm.MwdIndex] is DisabledMapping);
        
        _lvm.MwlIndex = 1;
        Assert.IsTrue(_lvm.MwlIndex == 1);
        Assert.IsTrue(_lvm.MouseWheelLeftCombo[_lvm.MwlIndex] is DisabledMapping);
        
        _lvm.MwrIndex = 1;
        Assert.IsTrue(_lvm.MwrIndex == 1);
        Assert.IsTrue(_lvm.MouseWheelRightCombo[_lvm.MwrIndex] is DisabledMapping);
    }
}