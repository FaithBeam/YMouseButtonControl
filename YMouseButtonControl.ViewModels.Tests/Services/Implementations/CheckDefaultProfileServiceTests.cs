using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Repositories;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.ViewModels.Services.Implementations;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Tests.Services.Implementations;

[TestClass]
public class CheckDefaultProfileServiceTests
{
    private readonly AutoMocker _autoMocker;

    public CheckDefaultProfileServiceTests()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void TestCheckDefaultProfile()
    {
        var repository = new Mock<IRepository<Profile>>();
        repository
            .Setup(x => x.GetAll())
            .Returns(new List<Profile> {new() {Name = "Default"}});
        var uow = new Mock<IUnitOfWork>();
        uow
            .Setup(x => x.GetRepository<Profile>())
            .Returns(repository.Object);
        _autoMocker
            .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
            .Returns(uow.Object);
        var service = _autoMocker.CreateInstance<CheckDefaultProfileService>();
        service.CheckDefaultProfile();
        repository
            .Verify(x => x.Add(
                It.IsAny<Profile>()
            ), Times.Never);

        repository
            .Setup(x => x.GetAll())
            .Returns(new List<Profile>());
        service.CheckDefaultProfile();
        repository
            .Verify(x => x.Add(
                It.IsAny<Profile>()
            ), Times.Once);
    }
}