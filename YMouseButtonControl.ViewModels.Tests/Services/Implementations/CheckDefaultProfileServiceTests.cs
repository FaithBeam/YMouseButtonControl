using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Repositories;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.ViewModels.Tests.Services.Implementations;

[TestClass]
public class CheckDefaultProfileServiceTests
{
    private readonly AutoMocker _autoMocker;

    public CheckDefaultProfileServiceTests()
    {
        _autoMocker = new AutoMocker();
    }

    // [TestMethod]
    // public void TestCheckDefaultProfile()
    // {
    //     // Test default profile already there, don't add a new one
    //     var repository = new Mock<IRepository<Profile>>();
    //     repository
    //         .Setup(x => x.GetAll())
    //         .Returns(new List<Profile> {new() {Name = "Default"}});
    //     _autoMocker
    //         .Use<ICurrentProfileOperationsMediator>(x => x.CurrentProfile == new Profile());
    //     var uow = new Mock<IUnitOfWork>();
    //     uow
    //         .Setup(x => x.GetRepository<Profile>())
    //         .Returns(repository.Object);
    //     _autoMocker
    //         .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
    //         .Returns(uow.Object);
    //     var service = _autoMocker.CreateInstance<CheckDefaultProfileService>();
    //     service.CheckDefaultProfile();
    //     repository
    //         .Verify(x => x.Add(
    //             It.IsAny<Profile>()
    //         ), Times.Never);
    //
    //     // Test default profile missing, add it
    //     repository
    //         .Setup(x => x.GetAll())
    //         .Returns(new List<Profile> {new()});
    //     service.CheckDefaultProfile();
    //     repository
    //         .Verify(x => x.Add(
    //             It.IsAny<Profile>()
    //         ), Times.Once);
    // }
}