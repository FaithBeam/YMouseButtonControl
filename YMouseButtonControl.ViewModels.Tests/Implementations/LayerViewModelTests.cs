using Avalonia.Collections;
using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Repositories;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;

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
        _lvm = _autoMocker.CreateInstance<LayerViewModel>();
    }
    
    [TestMethod]
    public void TestChangeMouseButtonIndex()
    {
        // Test changing mb1 to disabled mapping
        var psMock = _autoMocker!.GetMock<IProfilesService>();
        _lvm!.Mb1Index = 1;
        Assert.IsTrue(_lvm.Mb1Index == 1);
        Assert.IsTrue(_lvm.Mb1 is DisabledMapping);
        psMock.Verify(x => x.UpdateCurrentMouse(It.IsAny<DisabledMapping>(), MouseButton.MouseButton1), Times.Once);
        // _autoMocker.VerifyAll();
        ;
        // _psMock.Verify(x => x.UpdateCurrentMouse(It.IsAny<DisabledMapping>(), MouseButton.MouseButton1), Times.Once);
    }
}