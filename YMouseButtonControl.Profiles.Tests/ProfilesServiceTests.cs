using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using YMouseButtonControl.DataAccess.Configuration;
using YMouseButtonControl.DataAccess.LiteDb;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Profiles.Exceptions;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.Profiles.Tests;

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
            "{\"Checked\":true,\"Name\":\"Name05916a2d-1437-48e2-ad77-373f021dfac2\",\"MouseButton1\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescriptione7013a77-87bf-4988-90b0-159de92cdbd1\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton2\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription2249e789-8730-44f7-8636-c3f426a44cde\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton3\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescriptione8423bb3-4a06-4c4b-9379-7b602e4b47f9\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton4\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription0fdfd770-dda4-4589-a728-d67a88c1f201\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton5\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription1be45aad-4a58-4694-920a-2ce79f8a2ce5\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelUp\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription9754fafe-7592-403d-8769-72bbbcbcf4d5\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelDown\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription13554aa1-88f6-4781-b978-f0c0a9c11b7f\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelLeft\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescriptione2cfc5e1-9e59-4f37-9632-76c2d9294557\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelRight\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription8b5ee944-4dae-404a-b074-0b7ce4b54560\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"Description\":\"Description32c05896-5b9e-4bb4-a98d-230a4351f4c5\",\"WindowCaption\":\"WindowCaption69c7f90a-c297-4bdd-98e5-66d95d2f2b43\",\"Process\":\"Process7af874b2-ec15-4166-8e74-8c1733c612b3\",\"WindowClass\":\"WindowClass5fd1ee8d-c60f-4fd4-9f31-dec888a353d4\",\"ParentClass\":\"ParentClass44d32cf9-12a0-4748-b27a-ed6f89726d2c\",\"MatchType\":\"MatchTypef6473311-57de-474d-8571-cb06fa665216\"}";
        const string path = "tmp.json";
        File.WriteAllText(path, json);
        var sut = GetSut();

        sut.ImportProfileFromPath(path);

        var imported = sut.Profiles[1];

        Assert.Multiple(() =>
        {
            Assert.That(imported, Is.Not.Null);
            Assert.That(imported.Name, Is.EqualTo("Name05916a2d-1437-48e2-ad77-373f021dfac2"));
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
                typeof(YMouseButtonControl.DataAccess.Models.Interfaces.ISimulatedKeystrokesType),
                typeof(StickyHoldActionType)
            )
        );
        fixture.Customizations.Add(
            new TypeRelay(
                typeof(YMouseButtonControl.DataAccess.Models.Interfaces.IButtonMapping),
                typeof(NothingMapping)
            )
        );
        return fixture.CreateMany<Profile>(count).ToList()
            ?? throw new Exception("Error creating test profiles");
    }
}
