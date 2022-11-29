using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Repositories;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.Profiles.Tests;

[TestClass]
public class ProfilesServiceTests
{
    private AutoMocker? _mocker;
    private Mock<IRepository<Profile>>? _repoMock;
    private Mock<IUnitOfWork>? _uowMock;
    private IProfilesService? _profilesService;

    [TestInitialize]
    public void TestInitialize()
    {
        _mocker = new AutoMocker();
        _repoMock = new Mock<IRepository<Profile>>();
        _repoMock
            .Setup(x => x.GetAll())
            // Return a function that creates a new list of profile because edits to the profile in the profile service,
            // will edit this profile as well.
            .Returns(() => new List<Profile> {new() {Name = "Default"}});
        _uowMock = new Mock<IUnitOfWork>();
        _uowMock
            .Setup(x => x.GetRepository<Profile>())
            .Returns(_repoMock.Object);
        _mocker
            .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
            .Returns(_uowMock.Object);
        _profilesService = _mocker.CreateInstance<ProfilesService>();
    }

    [TestMethod]
    public void TestCurrentProfileIndex()
    {
        _profilesService!.CurrentProfileIndex = 0;
        Assert.AreEqual(_profilesService.CurrentProfileIndex, 0);

        // Profile must exist in profiles list otherwise error
        _profilesService.AddProfile(new Profile());
        _profilesService.CurrentProfileIndex = 1;
        Assert.AreEqual(_profilesService.CurrentProfileIndex, 1);

        Assert.ThrowsException<ReactiveUI.UnhandledErrorException>(() => _profilesService.CurrentProfileIndex = 2);
    }

    [TestMethod]
    public void TestChangingCurrentProfiles()
    {
        _profilesService!.CurrentProfile.MouseButton1 = new DisabledMapping();
        var newProfile = new Profile();
        _profilesService.AddProfile(newProfile);
        Assert.IsTrue(_profilesService.CurrentProfile.MouseButton1 is DisabledMapping);
        _profilesService.CurrentProfileIndex = 1;
        Assert.IsTrue(_profilesService.CurrentProfile.MouseButton1 is NothingMapping);
    }
    
    [TestMethod]
    public void TestUpdateMouseButton()
    {
        _profilesService!.CurrentProfile.MouseButton1 = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseButton1 is DisabledMapping);

        _profilesService.CurrentProfile.MouseButton2 = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseButton2 is DisabledMapping);

        _profilesService!.CurrentProfile.MouseButton3 = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseButton3 is DisabledMapping);

        _profilesService!.CurrentProfile.MouseButton4 = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseButton4 is DisabledMapping);

        _profilesService!.CurrentProfile.MouseButton5 = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseButton5 is DisabledMapping);

        _profilesService!.CurrentProfile.MouseWheelUp = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseWheelUp is DisabledMapping);

        _profilesService!.CurrentProfile.MouseWheelDown = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseWheelDown is DisabledMapping);

        _profilesService!.CurrentProfile.MouseWheelLeft = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseWheelLeft is DisabledMapping);

        _profilesService!.CurrentProfile.MouseWheelRight = new DisabledMapping();
        Assert.IsTrue(_profilesService.CurrentProfile.MouseWheelRight is DisabledMapping);
    }

    [TestMethod]
    public void TestAddProfile()
    {
        var newProfile = new Profile { Name = "NewProfile" };
        _profilesService!.AddProfile(newProfile);
        CollectionAssert.Contains(_profilesService.Profiles, newProfile);
    }

    [TestMethod]
    public void TestGetProfiles()
    {
        CollectionAssert.AreEquivalent(_profilesService!.GetProfiles().ToList(), new List<Profile>{new(){Name = "Default"}});
    }

    [TestMethod]
    public void TestIsUnsavedChanges()
    {
        // Default test, should be no differences
        Assert.IsFalse(_profilesService!.IsUnsavedChanges());

        _profilesService.CurrentProfile.MouseButton1 = new DisabledMapping();
        Assert.IsTrue(_profilesService.IsUnsavedChanges());
    }

    [TestMethod]
    public void TestApplyProfiles()
    {
        var newProfile = new Profile { Name = "NewProfile" };
        var newList = new List<Profile> { newProfile };
        _profilesService!.AddProfile(newProfile);
        _profilesService.ApplyProfiles();
        _repoMock!.Verify(x => x.ApplyAction(It.IsAny<IEnumerable<Profile>>()), Times.Once());
    }

    [TestMethod]
    public void TestLoadProfilesFromDb()
    {
        CollectionAssert.AreEquivalent(_profilesService!.Profiles, new List<Profile> {new() {Name = "Default"}});
    }
}