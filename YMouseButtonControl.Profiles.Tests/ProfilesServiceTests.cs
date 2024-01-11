using Avalonia.Animation;
using FluentAssertions;
using LiteDB;
using NSubstitute;
using SharpHook.Native;
using YMouseButtonControl.DataAccess.LiteDb;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Models.Models;
using YMouseButtonControl.DataAccess.Repositories;
using YMouseButtonControl.DataAccess.UnitOfWork;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.Profiles.Tests;

public class ProfilesServiceTests
{
    [Test]
    public void TestCurrentProfileIndex_DefaultProfile()
    {
        var (sut, _, _, _) = GetSut();
        
        sut.CurrentProfileIndex = 0;

        Assert.That(sut.CurrentProfileIndex, Is.EqualTo(0));
    }
    
    [Test]
    public void TestCurrentProfileIndex_AddedProfile()
    {
        var (sut, _, _, _) = GetSut();
        // Profile must exist in profiles list otherwise error
        sut.AddProfile(new Profile());

        sut.CurrentProfileIndex = 1;
        
        Assert.That(sut.CurrentProfileIndex, Is.EqualTo(1));
    }

    private (ProfilesService sut, IUnitOfWorkFactory uowfM, IUnitOfWork uowM, IRepository<Profile> rM) GetSut()
    {
        var rM = Substitute.For<IRepository<Profile>>();
        var uowM = Substitute.For<IUnitOfWork>();
        var uowfM = Substitute.For<IUnitOfWorkFactory>();
        rM.GetAll().Returns(new List<Profile> { new() { Name = "Default", Description = "Default"} });
        uowM.GetRepository<Profile>().Returns(rM);
        uowfM.Create().Returns(uowM);
        var sut = new ProfilesService(uowfM);
        return (sut, uowfM, uowM, rM);
    }

    [Test]
    public void TestChangingCurrentProfiles()
    {
        var (sut, _, _, _) = GetSut();
        sut.AddProfile(new Profile());

        sut.CurrentProfileIndex = 1;
        
        Assert.That(sut.CurrentProfile.MouseButton1, Is.InstanceOf<NothingMapping>());
    }
    
    [Test]
    public void TestUpdateMouseButton()
    {
        var (sut, _, _, _) = GetSut();
        sut.CurrentProfile.MouseButton1 = new DisabledMapping();
        sut.CurrentProfile.MouseButton2 = new DisabledMapping();
        sut.CurrentProfile.MouseButton3 = new DisabledMapping();
        sut.CurrentProfile.MouseButton4 = new DisabledMapping();
        sut.CurrentProfile.MouseButton5 = new DisabledMapping();
        sut.CurrentProfile.MouseWheelUp = new DisabledMapping();
        sut.CurrentProfile.MouseWheelDown = new DisabledMapping();
        sut.CurrentProfile.MouseWheelLeft = new DisabledMapping();
        sut.CurrentProfile.MouseWheelRight = new DisabledMapping();
        
        Assert.Multiple(() =>
        {
            Assert.That(sut.CurrentProfile.MouseButton1 is DisabledMapping);
            Assert.That(sut.CurrentProfile.MouseButton2 is DisabledMapping);
            Assert.That(sut.CurrentProfile.MouseButton3 is DisabledMapping);
            Assert.That(sut.CurrentProfile.MouseButton4 is DisabledMapping);
            Assert.That(sut.CurrentProfile.MouseButton5 is DisabledMapping);
            Assert.That(sut.CurrentProfile.MouseWheelUp is DisabledMapping);
            Assert.That(sut.CurrentProfile.MouseWheelDown is DisabledMapping);
            Assert.That(sut.CurrentProfile.MouseWheelLeft is DisabledMapping);
            Assert.That(sut.CurrentProfile.MouseWheelRight is DisabledMapping);
        });
    }
    
    [Test]
    public void TestAddProfile()
    {
        var (sut, _, _, _) = GetSut();
        var newProfile = new Profile { Name = "NewProfile" };
        
        sut.AddProfile(newProfile);
        
        CollectionAssert.Contains(sut.Profiles, newProfile);
    }
    
    [Test]
    public void TestRemoveProfile()
    {
        var (sut, _, _, _) = GetSut();
        var defaultProfile = sut.Profiles[0];
        var newProfile = new Profile { Name = "NewProfile" };
        sut.AddProfile(newProfile);
        sut.CurrentProfileIndex = 1;
        var removedProfile = sut.CurrentProfile;
    
        sut.RemoveProfile(sut.CurrentProfile);
        
        CollectionAssert.DoesNotContain(sut.Profiles, removedProfile);
        Assert.That(sut.CurrentProfile, Is.EqualTo(defaultProfile));
    }
    
    [Test]
    public void TestGetProfiles()
    {
        var (sut, _, _, _) = GetSut();
        var expected = new List<Profile> { new() { Name = "Default", Description = "Default"} };
        
        var result = sut.GetProfiles();
        
        result.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void TestIsUnsavedChanges_Default()
    {
        var (sut, _, _, _) = GetSut();
        
        Assert.That(sut.IsUnsavedChanges(), Is.False);
    }
    
    [Test]
    public void TestIsUnsavedChanges_MouseButtonChanged()
    {
        var (sut, _, _, _) = GetSut();
        
        sut.CurrentProfile.MouseButton1 = new DisabledMapping();
        
        Assert.That(sut.IsUnsavedChanges());
    }

    [Test]
    public void TestIsUnsavedChanges_AddProfile()
    {
        var (sut, _, _, _) = GetSut();
        
        sut.AddProfile(new Profile { Name = "Test" });
        
        Assert.That(sut.IsUnsavedChanges());
    }
    
    [Test]
    public void TestIsUnsavedChanges_MoveProfileUp()
    {
        var (sut, _, _, rM) = GetSut();
        var extraProfile = new Profile { Name = "Test" };
        rM.GetAll().Returns(rM.GetAll().Append(extraProfile));
        sut.AddProfile(extraProfile);
        Assert.That(sut.IsUnsavedChanges, Is.False);
        
        sut.MoveProfileUp(extraProfile);
        
        Assert.That(sut.IsUnsavedChanges());
    }
    
    
    [Test]
    public void TestApplyProfiles_ApplyActionCalled()
    {
        var (sut, _, _, rM) = GetSut();
        var newProfile = new Profile { Name = "NewProfile" };
        sut.AddProfile(newProfile);
        
        sut.ApplyProfiles();
        
        rM.ReceivedWithAnyArgs(1).ApplyAction(default);
    }
    
    [Test]
    public void TestLoadProfilesFromDb()
    {
        var (sut, _, _, _) = GetSut();
        
        CollectionAssert.AreEquivalent(sut.Profiles, new List<Profile> { new() { Name = "Default", Description = "Default"} });
    }

    [Test]
    public void TestAddDefaultProfile_AlreadyExists()
    {
        using var ms = new MemoryStream(File.ReadAllBytes("DefaultData.db"));
        using var db = new LiteDatabase(ms);
        var collection = db.GetCollection<Profile>();
        var rM = new Repository<Profile>(collection);
        var uowM = Substitute.For<IUnitOfWork>();
        var uowfM = Substitute.For<IUnitOfWorkFactory>();
        uowM.GetRepository<Profile>().Returns(rM);
        uowfM.Create().Returns(uowM);
        
        var sut = new ProfilesService(uowfM);
        
        Assert.That(sut.Profiles.Any);
    }
    
    // [Test]
    // public void TestCheckDefaultProfile()
    // {
    //     // Default profile already exists in db, don't add it
    //     newRepositoryMock = new Mock<IRepository<Profile>>();
    //     newRepositoryMock
    //         .Setup(x => x.GetAll())
    //         .Returns(() => new List<Profile> { new() { Name = "Default" } });
    //     newUowMock
    //         .Setup(x => x.GetRepository<Profile>())
    //         .Returns(newRepositoryMock.Object);
    //     newAutoMocker
    //         .Setup<IUnitOfWorkFactory, IUnitOfWork>(x => x.Create())
    //         .Returns(newUowMock.Object);
    //     newProfilesService = newAutoMocker.CreateInstance<ProfilesService>();
    //     newRepositoryMock.Verify(x => x.Add(It.IsAny<Profile>()), Times.Never);
    // }
    //
    // [Test]
    // public void TestReplaceProfile()
    // {
    //     var ps = _mocker.CreateInstance<ProfilesService>();
    //     var dummyProfile = new Profile { Name = "Test" };
    //     ps.AddProfile(dummyProfile);
    //     var newProfile = new Profile { Name = "NewProfile" };
    //     ps.ReplaceProfile(dummyProfile, newProfile);
    //     CollectionAssert.DoesNotContain(ps.Profiles, dummyProfile);
    //
    //     // Replace a profile that's the current profile
    //     ps = _mocker.CreateInstance<ProfilesService>();
    //     ps.AddProfile(dummyProfile);
    //     ps.CurrentProfileIndex = 1;
    //     ps.ReplaceProfile(dummyProfile, newProfile);
    //     CollectionAssert.DoesNotContain(ps.Profiles, dummyProfile);
    //     Assert.AreEqual(1, ps.CurrentProfileIndex);
    //     Assert.AreEqual(newProfile, ps.CurrentProfile);
    // }
    //
    // [Test]
    // public void TestMoveProfileUp()
    // {
    //     var ps = _mocker.CreateInstance<ProfilesService>();
    //     var dummyProfile = new Profile { Name = "Test" };
    //     ps.AddProfile(dummyProfile);
    //     ps.CurrentProfileIndex = 1;
    //     ps.MoveProfileUp(ps.CurrentProfile);
    //     CollectionAssert.Contains(ps.Profiles, dummyProfile);
    //     Assert.AreEqual(0, ps.CurrentProfileIndex);
    //     Assert.AreEqual(0, ps.Profiles.IndexOf(dummyProfile));
    // }
    //
    // [Test]
    // public void TestMoveProfileDown()
    // {
    //     var ps = _mocker.CreateInstance<ProfilesService>();
    //     var dummyProfile = new Profile { Name = "Test" };
    //     ps.AddProfile(dummyProfile);
    //     ps.CurrentProfileIndex = 0;
    //     var defaultProfile = ps.CurrentProfile;
    //     ps.MoveProfileDown(defaultProfile);
    //     CollectionAssert.Contains(ps.Profiles, dummyProfile);
    //     Assert.AreEqual(1, ps.CurrentProfileIndex);
    //     Assert.AreEqual(0, ps.Profiles.IndexOf(dummyProfile));
    //     CollectionAssert.Contains(ps.Profiles, defaultProfile);
    //     Assert.AreEqual(1, ps.Profiles.IndexOf(defaultProfile));
    // }
    //
    // [Test]
    // public void TestCopyProfile()
    // {
    //     var ps = _mocker.CreateInstance<ProfilesService>();
    //     var sequence = new List<IParsedEvent>
    //     {
    //         new ParsedKey
    //         {
    //             Event = "q",
    //             KeyCode = KeyCode.VcQ,
    //             IsModifier = false
    //         }
    //     };
    //     var dummyProfile = new Profile
    //     {
    //         Name = "Test",
    //         MouseButton1 = new SimulatedKeystrokes { SimulatedKeystrokesType = new DuringMouseActionType(), Sequence = sequence }
    //     };
    //     var copiedProfile = ps.CopyProfile(dummyProfile);
    //     // Assert.AreEqual(copiedProfile, dummyProfile);
    //     copiedProfile.Should().BeEquivalentTo(dummyProfile);
    // }
}