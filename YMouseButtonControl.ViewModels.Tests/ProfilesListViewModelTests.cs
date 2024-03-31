using System.Net;
using System.Reactive;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using YMouseButtonControl.DataAccess.Configuration;
using YMouseButtonControl.DataAccess.LiteDb;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.ViewModels.Tests;

public class ProfilesListViewModelTests
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
    public void Ctor()
    {
        var pf = GetProfilesService();
        var psdvmMock = Substitute.For<IProcessSelectorDialogViewModel>();
        var sut = new ProfilesListViewModel(pf, psdvmMock);

        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf<IProfilesListViewModel>());
        });
    }

    [Test]
    public void AddButtonAddsProfileToProfileList()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        var profileToReturn = new Profile { Name = "WAHOO" };
        sut.ShowProcessSelectorInteraction.RegisterHandler(context =>
            context.SetOutput(profileToReturn)
        );

        sut.AddButtonCommand.Execute(null);

        pf.Profiles.Should().ContainEquivalentOf(profileToReturn);
    }

    [Test]
    public void AddButton_NullReturned()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        sut.ShowProcessSelectorInteraction.RegisterHandler(context => context.SetOutput(null));

        sut.AddButtonCommand.Execute(null);

        pf.Profiles.Should().NotContainNulls();
    }

    [Test]
    public void EditButton_UserSelectsProcess()
    {
        var pf = GetProfilesService(GetSeedData());
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        var profileToReturn = new Profile { Name = "WAHOO" };
        sut.ShowProcessSelectorInteraction.RegisterHandler(context =>
            context.SetOutput(profileToReturn)
        );
        var profileToEdit = pf.Profiles[1];
        var profileToEditIdx = pf.Profiles.IndexOf(profileToEdit);
        pf.CurrentProfileIndex = profileToEditIdx;

        sut.EditButtonCommand.Execute(Unit.Default).Subscribe();

        Assert.Multiple(() =>
        {
            pf.Profiles.Should().NotContainEquivalentOf(profileToEdit);
            pf.CurrentProfile.Name.Should().BeSameAs("WAHOO");
        });
    }

    [Test]
    public void EditButton_UserCancelsProcessSelectorDialog()
    {
        var pf = GetProfilesService(GetSeedData());
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        sut.ShowProcessSelectorInteraction.RegisterHandler(context => context.SetOutput(null));
        var profileToEdit = pf.Profiles[1];
        var profileToEditIdx = pf.Profiles.IndexOf(profileToEdit);
        pf.CurrentProfileIndex = profileToEditIdx;

        sut.EditButtonCommand.Execute(Unit.Default).Subscribe();

        Assert.Multiple(() =>
        {
            pf.Profiles.Should().NotContainNulls();
            pf.Profiles.Should().ContainEquivalentOf(profileToEdit);
        });
    }

    [Test]
    public void EditButton_DisabledWhenCurrentProfileIsDefault()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);

        sut.EditButtonCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void EditButton_EnabledWhenNonDefaultProfileSelected()
    {
        var pf = GetProfilesService(GetSeedData());
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(pf.Profiles[1]);

        sut.EditButtonCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.True);
        });
    }

    [Test]
    public void RemoveButton_RemoveProfile()
    {
        var pf = GetProfilesService(GetSeedData());
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        var profileToRemove = pf.Profiles[1];
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(profileToRemove);

        sut.RemoveButtonCommand.Execute(Unit.Default).Subscribe();

        pf.Profiles.Should().NotContainNulls();
        pf.Profiles.Should().NotContainEquivalentOf(profileToRemove);
    }

    [Test]
    public void RemoveButton_DisabledWhenDefaultProfileSelected()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);

        sut.RemoveButtonCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void RemoveButton_EnabledWhenNonDefaultProfileSelected()
    {
        var pf = GetProfilesService(GetSeedData());
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(pf.Profiles[1]);

        sut.RemoveButtonCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.True);
        });
    }

    [Test]
    public void UpButton_Enabled()
    {
        var pf = GetProfilesService(GetSeedData());
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        var profileToMove = pf.Profiles[2];
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(profileToMove);

        sut.UpCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.True);
        });
    }

    [Test]
    public void UpButton_DisabledForDefaultProfile()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);

        sut.UpCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void UpButton_DisabledForProfileImmediatelyBelowDefault()
    {
        var pf = GetProfilesService(GetSeedData(1));
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(pf.Profiles.Last());

        sut.UpCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void UpButton_MoveProfile()
    {
        var pf = GetProfilesService(GetSeedData());
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        var profileToMoveUp = pf.Profiles[2];
        var profileThatWillBeShiftedDown = pf.Profiles[1];
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(profileToMoveUp);

        sut.UpCommand.Execute(Unit.Default).Subscribe();

        Assert.Multiple(() =>
        {
            Assert.That(pf.Profiles.IndexOf(profileToMoveUp), Is.EqualTo(1));
            Assert.That(pf.Profiles.IndexOf(profileThatWillBeShiftedDown), Is.EqualTo(2));
        });
    }

    [Test]
    public void DownButton_DisabledForDefaultProfile()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);

        sut.DownCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void DownButton_DisabledForLastProfile()
    {
        var pf = GetProfilesService(GetSeedData(1));
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(pf.Profiles.Last());

        sut.DownCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void DownButton_Enabled()
    {
        var pf = GetProfilesService(GetSeedData(2));
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(pf.Profiles[1]);

        sut.DownCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.True);
        });
    }

    [Test]
    public void DownButton()
    {
        var pf = GetProfilesService(GetSeedData(2));
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        var profileToMove = pf.Profiles[1];
        var profileToBeShiftedUp = pf.Profiles[2];
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(profileToMove);

        sut.DownCommand.Execute(Unit.Default).Subscribe();

        Assert.Multiple(() =>
        {
            Assert.That(pf.Profiles.IndexOf(profileToMove), Is.EqualTo(2));
            Assert.That(pf.Profiles.IndexOf(profileToBeShiftedUp), Is.EqualTo(1));
        });
    }

    [Test]
    public void ExportButton()
    {
        var pf = GetProfilesService(GetSeedData(1));
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        var profileToExport = pf.Profiles[1];
        pf.CurrentProfileIndex = pf.Profiles.IndexOf(profileToExport);
        const string outputPath = "tmp.json";
        sut.ShowExportFileDialog.RegisterHandler(x => x.SetOutput(outputPath));

        sut.ExportCommand.Execute(Unit.Default).Subscribe();

        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(outputPath));
            var deserialized = JsonConvert.DeserializeObject<Profile>(
                File.ReadAllText(outputPath),
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
            );
            profileToExport.Should().BeEquivalentTo(deserialized);
        });

        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }
    }

    [Test]
    public void ExportButton_DisabledForDefaultProfile()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        sut.ExportCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void CopyButton_UserClicksCancel()
    {
        var pf = GetProfilesService(GetSeedData());
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        sut.ShowProcessSelectorInteraction.RegisterHandler(context => context.SetOutput(null));
        var numProfiles = pf.Profiles.Count;
        sut.CopyCommand.Execute(Unit.Default).Subscribe();

        pf.Profiles.Should().NotContainNulls();
        pf.Profiles.Count.Should().Be(numProfiles);
    }

    [Test]
    public void CopyButton_UserChoosesProcess()
    {
        var pf = GetProfilesService(GetSeedData(1));
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        var newProfile = new Profile { Name = "New Profile", Description = "Some description" };
        sut.ShowProcessSelectorInteraction.RegisterHandler(context =>
            context.SetOutput(newProfile)
        );
        pf.CurrentProfileIndex = 1;
        var profileToCopyFrom = pf.CurrentProfile;
        sut.CopyCommand.Execute(Unit.Default).Subscribe();

        Assert.Multiple(() =>
        {
            pf.Profiles.Should().NotContainNulls();
            pf.Profiles.Count.Should().Be(3);
            pf.Profiles.Should().ContainEquivalentOf(profileToCopyFrom);
            pf.Profiles.Should()
                .Contain(p => p.Name == newProfile.Name && p.Description == newProfile.Description);
        });
    }

    [Test]
    public void ImportButton()
    {
        const string data =
            "{\"Checked\":true,\"Name\":\"notepad++.exe\",\"MouseButton1\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":null,\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":null,\"MouseButtonDisabled\":false},\"MouseButton2\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":null,\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":null,\"MouseButtonDisabled\":false},\"MouseButton3\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":null,\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":null,\"MouseButtonDisabled\":false},\"MouseButton4\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokes, YMouseButtonControl.DataAccess.Models\",\"Index\":2,\"Description\":\"Simulated Keys (undefined)\",\"PriorityDescription\":\"\",\"Enabled\":false,\"Keys\":\"w\",\"State\":false,\"CanRaiseDialog\":true,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.DataAccess.Models\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton5\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":null,\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":null,\"MouseButtonDisabled\":false},\"MouseWheelUp\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":null,\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":null,\"MouseButtonDisabled\":false},\"MouseWheelDown\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":null,\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":null,\"MouseButtonDisabled\":false},\"MouseWheelLeft\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":null,\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":null,\"MouseButtonDisabled\":false},\"MouseWheelRight\":{\"$type\":\"YMouseButtonControl.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.DataAccess.Models\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":null,\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":null,\"MouseButtonDisabled\":false},\"Description\":\"*new 1 - Notepad++\",\"WindowCaption\":\"N/A\",\"Process\":\"notepad++.exe\",\"WindowClass\":\"N/A\",\"ParentClass\":\"N/A\",\"MatchType\":\"N/A\"}";
        const string path = "tmp.json";
        var deserialized = JsonConvert.DeserializeObject<Profile>(
            data,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
        );
        File.WriteAllText(path, data);
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        sut.ShowImportFileDialog.RegisterHandler(x => x.SetOutput(path));

        sut.ImportCommand.Execute(Unit.Default).Subscribe();

        pf.CurrentProfile.Should().BeEquivalentTo(deserialized);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    [Test]
    public void ImportButton_UserClicksCancel()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var sut = new ProfilesListViewModel(pf, psdvm);
        sut.ShowImportFileDialog.RegisterHandler(x => x.SetOutput(null));

        sut.ImportCommand.Execute(Unit.Default).Subscribe();

        Assert.That(pf.Profiles, Has.Count.EqualTo(1));
        pf.Profiles.Should().NotContainNulls();
    }

    private static ProfilesService GetProfilesService(IEnumerable<Profile>? seedData = null)
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
