using Avalonia.Animation;
using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
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
            .Returns(() => new List<Profile> { new() { Name = "Default" } });
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
    public void TestRemoveProfile()
    {
        var defaultProfile = _profilesService.Profiles[0];
        var newProfile = new Profile { Name = "NewProfile" };
        _profilesService!.AddProfile(newProfile);
        _profilesService.CurrentProfileIndex = 1;
        var removedProfile = _profilesService!.CurrentProfile;

        _profilesService!.RemoveProfile(_profilesService.CurrentProfile);
        CollectionAssert.DoesNotContain(_profilesService.Profiles, removedProfile);

        Assert.AreEqual(defaultProfile, _profilesService.CurrentProfile);
    }

    [TestMethod]
    public void TestGetProfiles()
    {
        CollectionAssert.AreEquivalent(_profilesService!.GetProfiles().ToList(),
            new List<Profile> { new() { Name = "Default" } });
    }

    [TestMethod]
    public void TestIsUnsavedChanges()
    {
        // Default test, should be no differences
        Assert.IsFalse(_profilesService!.IsUnsavedChanges());

        _profilesService.CurrentProfile.MouseButton1 = new DisabledMapping();
        Assert.IsTrue(_profilesService.IsUnsavedChanges());

        var defaultProfile = new Profile { Name = "Default" };
        var otherProfile = new Profile { Name = "Test" };
        var newMocker = new AutoMocker();
        var newRepoMock = new Mock<IRepository<Profile>>();
        newRepoMock
            .Setup(x => x.GetAll())
            .Returns(() => new List<Profile> { defaultProfile, otherProfile });
        var newUowMock = new Mock<IUnitOfWork>();
        newUowMock
            .Setup(x => x.GetRepository<Profile>())
            .Returns(newRepoMock.Object);
        newMocker
            .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
            .Returns(newUowMock.Object);
        var ps = newMocker.CreateInstance<ProfilesService>();
        ps.CurrentProfileIndex = 1;
        ps.MoveProfileUp(ps.CurrentProfile);
        Assert.IsTrue(ps.IsUnsavedChanges());
    }

    [TestMethod]
    public void TestApplyProfiles()
    {
        var newProfile = new Profile { Name = "NewProfile" };
        _profilesService!.AddProfile(newProfile);
        _profilesService.ApplyProfiles();
        _repoMock!.Verify(x => x.ApplyAction(It.IsAny<IEnumerable<Profile>>()), Times.Once());
    }

    [TestMethod]
    public void TestLoadProfilesFromDb()
    {
        CollectionAssert.AreEquivalent(_profilesService!.Profiles, new List<Profile> { new() { Name = "Default" } });
    }

    [TestMethod]
    public void TestCheckDefaultProfile()
    {
        // Default profile doesn't exist in db, add it
        var newAutoMocker = new AutoMocker();
        var newRepositoryMock = new Mock<IRepository<Profile>>();
        // WTF? Idk why this is necessary
        newRepositoryMock
            .SetupSequence(x => x.GetAll())
            .Returns(new List<Profile>())
            .Returns(new List<Profile> { new() { Name = "Default" } })
            .Returns(new List<Profile> { new() { Name = "Default" } })
            .Returns(new List<Profile> { new() { Name = "Default" } });
        var newUowMock = new Mock<IUnitOfWork>();
        newUowMock
            .Setup(x => x.GetRepository<Profile>())
            .Returns(newRepositoryMock.Object);
        newAutoMocker
            .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
            .Returns(newUowMock.Object);
        var newProfilesService = newAutoMocker.CreateInstance<ProfilesService>();
        newRepositoryMock.Verify(x => x.Add(It.IsAny<Profile>()), Times.Once);

        // Default profile already exists in db, don't add it
        newRepositoryMock = new Mock<IRepository<Profile>>();
        newRepositoryMock
            .Setup(x => x.GetAll())
            .Returns(() => new List<Profile> { new() { Name = "Default" } });
        newUowMock
            .Setup(x => x.GetRepository<Profile>())
            .Returns(newRepositoryMock.Object);
        newAutoMocker
            .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
            .Returns(newUowMock.Object);
        newProfilesService = newAutoMocker.CreateInstance<ProfilesService>();
        newRepositoryMock.Verify(x => x.Add(It.IsAny<Profile>()), Times.Never);
    }

    [TestMethod]
    public void TestReplaceProfile()
    {
        var ps = _mocker.CreateInstance<ProfilesService>();
        var dummyProfile = new Profile { Name = "Test" };
        ps.AddProfile(dummyProfile);
        var newProfile = new Profile { Name = "NewProfile" };
        ps.ReplaceProfile(dummyProfile, newProfile);
        CollectionAssert.DoesNotContain(ps.Profiles, dummyProfile);

        // Replace a profile that's the current profile
        ps = _mocker.CreateInstance<ProfilesService>();
        ps.AddProfile(dummyProfile);
        ps.CurrentProfileIndex = 1;
        ps.ReplaceProfile(dummyProfile, newProfile);
        CollectionAssert.DoesNotContain(ps.Profiles, dummyProfile);
        Assert.AreEqual(1, ps.CurrentProfileIndex);
        Assert.AreEqual(newProfile, ps.CurrentProfile);
    }

    [TestMethod]
    public void TestMoveProfileUp()
    {
        var ps = _mocker.CreateInstance<ProfilesService>();
        var dummyProfile = new Profile { Name = "Test" };
        ps.AddProfile(dummyProfile);
        ps.CurrentProfileIndex = 1;
        ps.MoveProfileUp(ps.CurrentProfile);
        CollectionAssert.Contains(ps.Profiles, dummyProfile);
        Assert.AreEqual(0, ps.CurrentProfileIndex);
        Assert.AreEqual(0, ps.Profiles.IndexOf(dummyProfile));
    }

    [TestMethod]
    public void TestMoveProfileDown()
    {
        var ps = _mocker.CreateInstance<ProfilesService>();
        var dummyProfile = new Profile { Name = "Test" };
        ps.AddProfile(dummyProfile);
        ps.CurrentProfileIndex = 0;
        var defaultProfile = ps.CurrentProfile;
        ps.MoveProfileDown(defaultProfile);
        CollectionAssert.Contains(ps.Profiles, dummyProfile);
        Assert.AreEqual(1, ps.CurrentProfileIndex);
        Assert.AreEqual(0, ps.Profiles.IndexOf(dummyProfile));
        CollectionAssert.Contains(ps.Profiles, defaultProfile);
        Assert.AreEqual(1, ps.Profiles.IndexOf(defaultProfile));
    }

    [TestMethod]
    public void TestCopyProfile()
    {
        var ps = _mocker.CreateInstance<ProfilesService>();
        var dummyProfile = new Profile
        {
            Name = "Test",
            MouseButton1 = new SimulatedKeystrokes { SimulatedKeystrokesType = new DuringMouseActionType(), Keys = "q" }
        };
        var copiedProfile = ps.CopyProfile(dummyProfile);
        Assert.AreEqual(dummyProfile, copiedProfile);
    }
}