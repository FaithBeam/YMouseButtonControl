using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using YMouseButtonControl.Core.DataAccess.Configuration;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Core.Profiles.Exceptions;
using YMouseButtonControl.Core.Profiles.Implementations;
using YMouseButtonControl.Core.Profiles.Interfaces;
using YMouseButtonControl.DataAccess.LiteDb;

namespace YMouseButtonControl.Core.Tests.Profiles;

public class ProfilesServiceTests
{
    private const string ConnStr = "tmp.db";

    [SetUp]
    public void Setup()
    {
        if (File.Exists(ConnStr))
        {
            File.Delete(ConnStr);
        }
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(ConnStr))
        {
            File.Delete(ConnStr);
        }
    }

    [Test]
    public void TestCtor()
    {
        var sut = GetSut();

        Assert.That(sut, Is.InstanceOf<IProfilesService>());
    }

    [Test]
    public void HasDefaultDatabase()
    {
        var sut = GetSut();

        Assert.Multiple(() =>
        {
            Assert.That(sut.Profiles, Has.Count.EqualTo(1));
            Assert.That(sut.CurrentProfileIndex, Is.EqualTo(0));
            Assert.That(sut.CurrentProfile.Name, Is.EqualTo("Default"));
            Assert.That(sut.UnsavedChanges, Is.False);
        });
    }

    [Test]
    public void AddProfile()
    {
        var newProfile = new Profile { Name = "NEW" };
        var sut = GetSut(new[] { newProfile });

        Assert.Multiple(() =>
        {
            Assert.That(sut.Profiles, Has.Count.EqualTo(2));
            Assert.That(sut.UnsavedChanges);
            sut.CurrentProfile.Should().BeEquivalentTo(newProfile);
        });
    }

    [Test]
    public void RemoveProfile()
    {
        var sut = GetSut(GetSeedData());
        var profileToRemove = sut.Profiles[5];

        sut.RemoveProfile(profileToRemove);

        sut.Profiles.Should().NotContainEquivalentOf(profileToRemove);
    }

    [Test]
    public void RemoveDefaultProfile_ThrowsException()
    {
        var sut = GetSut();

        Assert.Throws<Exception>(() => sut.RemoveProfile(sut.CurrentProfile));
    }

    [Test]
    public void MoveDefaultProfile_ThrowsException()
    {
        var sut = GetSut();

        Assert.Throws<InvalidMoveException>(() => sut.MoveProfileDown(sut.CurrentProfile));
        Assert.Throws<InvalidMoveException>(() => sut.MoveProfileUp(sut.CurrentProfile));
    }

    [Test]
    public void MoveProfileAboveDefault_ThrowsException()
    {
        var sut = GetSut(GetSeedData());

        Assert.Throws<InvalidMoveException>(() => sut.MoveProfileUp(sut.Profiles[1]));
    }

    [Test]
    public void MoveProfileDown()
    {
        var sut = GetSut(GetSeedData());
        var profileToMoveDown = sut.Profiles[5];
        var initialIdx = sut.Profiles.IndexOf(profileToMoveDown);
        sut.MoveProfileDown(profileToMoveDown);

        var newIdx = sut.Profiles.IndexOf(profileToMoveDown);

        Assert.That(newIdx, Is.EqualTo(initialIdx + 1));
    }

    [Test]
    public void MoveProfileUp()
    {
        var sut = GetSut(GetSeedData());
        var profile = sut.Profiles[5];
        var initialIdx = sut.Profiles.IndexOf(profile);
        sut.MoveProfileUp(profile);

        var newIdx = sut.Profiles.IndexOf(profile);

        Assert.That(newIdx, Is.EqualTo(initialIdx - 1));
    }

    [Test]
    public void CopyProfile()
    {
        var sut = GetSut(GetSeedData());
        var profile = sut.Profiles[1];

        var profileCopy = sut.CopyProfile(profile);

        profileCopy.Should().BeEquivalentTo(profile);
    }

    [Test]
    public void WriteProfileToPath()
    {
        const string output = "tmp.json";
        var sut = GetSut(GetSeedData());
        var profile = sut.Profiles[1];

        sut.WriteProfileToFile(profile, output);

        Assert.That(File.Exists(output));

        if (File.Exists(output))
        {
            File.Delete(output);
        }
    }

    [Test]
    public void ImportProfileFromPath()
    {
        const string json =
            "{\"Checked\":true,\"Name\":\"Name7c8fd081-5cc2-41ac-8daa-34fdb59024eb\",\"MouseButton1\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription7815a17c-db7b-44f1-8d37-4f4abe1a140a\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton2\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription6c5331ad-732e-4a2b-b566-7dc446b35f0d\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton3\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescriptionaabc8b2b-49ae-48dd-adcf-2fc2ef068d09\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton4\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription89e8664b-29c9-4d0b-a4f8-e86805ed0485\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton5\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription7b3d0fd6-3dc6-400a-bc99-fa46ca932095\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelUp\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription888080de-7fd7-4b9d-828d-584ce0bfdd8e\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelDown\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription4975e8aa-c9a2-49f7-a34d-47932cbbac5c\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelLeft\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription57cbfaf3-19a0-4d4f-976c-36dad6e95c6c\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelRight\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription8d04cfce-5361-4027-a790-fa996eccab2a\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"Description\":\"Descriptionb0ba4f6e-4929-433e-8cb1-a8aa256ef7f3\",\"WindowCaption\":\"WindowCaption41783e0a-bbd3-4c80-8053-c1662788849d\",\"Process\":\"Process9952adfe-db98-4cfc-90a7-a0e20f58aebc\",\"WindowClass\":\"WindowClass31f013da-d1bc-4c83-a67d-ee4175db575e\",\"ParentClass\":\"ParentClassb6be68c5-5be1-4ef6-928a-cb8e6e8426bf\",\"MatchType\":\"MatchTypedbacc055-e328-4a76-adcc-b29127ab72d1\"}";
        const string path = "tmp.json";
        File.WriteAllText(path, json);
        var sut = GetSut();

        sut.ImportProfileFromPath(path);

        var imported = sut.Profiles[1];

        Assert.Multiple(() =>
        {
            Assert.That(imported, Is.Not.Null);
            Assert.That(imported.Name, Is.EqualTo("Name7c8fd081-5cc2-41ac-8daa-34fdb59024eb"));
            Assert.That(imported.Id, Is.EqualTo(2));
        });

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    [Test]
    public void GetProfiles()
    {
        var sut = GetSut(GetSeedData());

        var profiles = sut.GetProfiles();

        Assert.That(profiles, Is.Not.Null);
    }

    [Test]
    public void ReplaceProfile()
    {
        var sut = GetSut(GetSeedData());
        var toBeReplaced = sut.Profiles[1];
        var replacement = new Profile { Name = "WAHOO" };

        sut.ReplaceProfile(toBeReplaced, replacement);

        Assert.Multiple(() =>
        {
            sut.Profiles.Should().NotContainEquivalentOf(toBeReplaced);
            sut.Profiles.Should().ContainEquivalentOf(replacement);
            sut.Profiles.IndexOf(replacement).Should().Be(1);
        });
    }

    [Test]
    public void ReplaceDefaultProfile_ThrowsException()
    {
        var sut = GetSut();
        var replacement = new Profile { Name = "WAHOO" };

        Assert.Throws<InvalidReplaceException>(
            () => sut.ReplaceProfile(sut.CurrentProfile, replacement)
        );
    }

    private static ProfilesService GetSut()
    {
        return GetSut(Array.Empty<Profile>());
    }

    private static ProfilesService GetSut(IEnumerable<Profile>? seedData)
    {
        var dbConfig = new DatabaseConfiguration { ConnectionString = $"Filename={ConnStr}" };
        var uowF = new LiteDbUnitOfWorkFactory(dbConfig);
        var pf = new ProfilesService(uowF);
        if (seedData is null)
            return pf;
        foreach (var seed in seedData)
        {
            pf.AddProfile(seed);
        }

        return pf;
    }

    private static List<Profile> GetSeedData(int count = 10)
    {
        var fixture = new Fixture();
        fixture.Customizations.Add(
            new TypeRelay(
                typeof(DataAccess.Models.Interfaces.ISimulatedKeystrokesType),
                typeof(StickyHoldActionType)
            )
        );
        fixture.Customizations.Add(
            new TypeRelay(
                typeof(DataAccess.Models.Interfaces.IButtonMapping),
                typeof(NothingMapping)
            )
        );
        return fixture.CreateMany<Profile>(count).ToList()
            ?? throw new Exception("Error creating test profiles");
    }
}
