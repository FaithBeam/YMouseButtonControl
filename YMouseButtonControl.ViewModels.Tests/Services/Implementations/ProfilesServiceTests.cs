using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Repositories;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.ViewModels.Services.Implementations;

namespace YMouseButtonControl.ViewModels.Tests.Services.Implementations;

[TestClass]
public class ProfilesServiceTests
{
    private readonly AutoMocker _autoMocker;

    public ProfilesServiceTests()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void TestGetProfiles()
    {
        var repository = new Mock<IRepository<Profile>>();
        repository
            .Setup(x => x.GetAll())
            .Returns(new List<Profile> {new(){Name = "Test"}});
        var uow = new Mock<IUnitOfWork>();
        uow
            .Setup(x => x.GetRepository<Profile>())
            .Returns(repository.Object);
        _autoMocker
            .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
            .Returns(uow.Object);
        var service = _autoMocker.CreateInstance<ProfilesService>();
        var profiles = service.GetProfiles();
        Assert.IsTrue(profiles.Count() == 1);
        Assert.IsTrue(profiles.First().Name == "Test");
    }

    [TestMethod]
    public void TestAddProfile()
    {
        var repository = new Mock<IRepository<Profile>>();
        var uow = new Mock<IUnitOfWork>();
        uow
            .Setup(x => x.GetRepository<Profile>())
            .Returns(repository.Object);
        _autoMocker
            .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
            .Returns(uow.Object);
        var service = _autoMocker.CreateInstance<ProfilesService>();
        var testProfile = new Profile {Name = "Test"};
        service.AddProfile(testProfile);
        repository
            .Verify(x => x.Add(
            It.Is<Profile>(x => x.Name == testProfile.Name)
            ), Times.Once());
    }
}