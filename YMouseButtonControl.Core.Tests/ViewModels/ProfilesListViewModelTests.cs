using System.Reactive;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using YMouseButtonControl.Core.DataAccess.Configuration;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Profiles.Implementations;
using YMouseButtonControl.Core.ViewModels.Implementations;
using YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.Core.ViewModels.Interfaces;
using YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;
using YMouseButtonControl.Core.ViewModels.ProfilesList;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Features.Add;
using YMouseButtonControl.DataAccess.LiteDb;

namespace YMouseButtonControl.Core.Tests.ViewModels;

public class ProfilesListViewModelTests : BaseTest
{
    [Test]
    public void Ctor()
    {
        var pf = GetProfilesService();
        var psdvmMock = Substitute.For<IProcessSelectorDialogViewModel>();
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvmMock, addProfile);

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        sut.ShowProcessSelectorInteraction.RegisterHandler(context => context.SetOutput(null));

        sut.AddButtonCommand.Execute(null);

        pf.Profiles.Should().NotContainNulls();
    }

    [Test]
    public void EditButton_UserSelectsProcess()
    {
        var pf = GetProfilesService(GetSeedData());
        pf.CurrentProfile = pf.Profiles.First(x => !x.IsDefault);
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        var profileToReturn = new Profile { Name = "WAHOO" };
        sut.ShowProcessSelectorInteraction.RegisterHandler(context =>
            context.SetOutput(profileToReturn)
        );
        var profileToEdit = pf.Profiles[1];

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        sut.ShowProcessSelectorInteraction.RegisterHandler(context => context.SetOutput(null));
        var profileToEdit = pf.CurrentProfile;

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);

        sut.EditButtonCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void EditButton_EnabledWhenNonDefaultProfileSelected()
    {
        var pf = GetProfilesService(GetSeedData());
        pf.CurrentProfile = pf.Profiles.First(x => !x.IsDefault);
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        var profileToRemove = pf.Profiles.First(x => !x.IsDefault);
        pf.CurrentProfile = profileToRemove;

        sut.RemoveButtonCommand.Execute(Unit.Default).Subscribe();

        pf.Profiles.Should().NotContainNulls();
        pf.Profiles.Should().NotContainEquivalentOf(profileToRemove);
    }

    [Test]
    public void RemoveButton_DisabledWhenDefaultProfileSelected()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);

        sut.RemoveButtonCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void RemoveButton_EnabledWhenNonDefaultProfileSelected()
    {
        var pf = GetProfilesService(GetSeedData());
        pf.CurrentProfile = pf.Profiles.First(x => !x.IsDefault);
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        var profileToMove = pf.Profiles.Last(x => !x.IsDefault);
        pf.CurrentProfile = profileToMove;

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);

        sut.UpCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void UpButton_DisabledForProfileImmediatelyBelowDefault()
    {
        var pf = GetProfilesService(GetSeedData(1));
        pf.CurrentProfile = pf.Profiles.Where(x => !x.IsDefault).MinBy(x => x.DisplayPriority);
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        var profileToMoveUp = pf.Profiles.Last(x => !x.IsDefault);
        var p1p = profileToMoveUp.DisplayPriority;
        pf.CurrentProfile = profileToMoveUp;
        var profileThatWillBeShiftedDown = pf
            .Profiles.Where(x => x.DisplayPriority < profileToMoveUp.DisplayPriority)
            .MaxBy(x => x.DisplayPriority);
        var p2p = profileThatWillBeShiftedDown.DisplayPriority;

        sut.UpCommand.Execute(Unit.Default).Subscribe();

        Assert.Multiple(() =>
        {
            Assert.That(profileToMoveUp.DisplayPriority, Is.EqualTo(p2p));
            Assert.That(profileThatWillBeShiftedDown.DisplayPriority, Is.EqualTo(p1p));
        });
    }

    [Test]
    public void DownButton_DisabledForDefaultProfile()
    {
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        pf.CurrentProfile = pf.Profiles.Where(x => !x.IsDefault).MaxBy(x => x.DisplayPriority);

        sut.DownCommand.CanExecute.Subscribe(x =>
        {
            Assert.That(x, Is.False);
        });
    }

    [Test]
    public void DownButton_Enabled()
    {
        var pf = GetProfilesService(GetSeedData(2));
        pf.CurrentProfile = pf
            .Profiles.Where(p => p.DisplayPriority > 0)
            .MinBy(x => x.DisplayPriority);
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);

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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        var profileToMove = pf.Profiles.First(x => !x.IsDefault);
        pf.CurrentProfile = profileToMove;
        var priority1 = profileToMove.DisplayPriority;
        var profileToBeShiftedUp = pf
            .Profiles.Where(x => x.DisplayPriority > profileToMove.DisplayPriority)
            .MinBy(x => x.DisplayPriority);
        var priority2 = profileToBeShiftedUp!.DisplayPriority;

        Assert.That(priority1, Is.LessThan(priority2));

        sut.DownCommand.Execute(Unit.Default).Subscribe();

        Assert.Multiple(() =>
        {
            Assert.That(profileToMove.DisplayPriority, Is.EqualTo(priority2));
            Assert.That(profileToBeShiftedUp.DisplayPriority, Is.EqualTo(priority1));
        });
    }

    [Test]
    public void ExportButton()
    {
        var pf = GetProfilesService(GetSeedData(1));
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        var profileToExport = pf.Profiles[1];
        pf.CurrentProfile = profileToExport;
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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        var newProfile = new Profile { Name = "New Profile", Description = "Some description" };
        sut.ShowProcessSelectorInteraction.RegisterHandler(context =>
            context.SetOutput(newProfile)
        );
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
            "{\"Checked\":true,\"Name\":\"Name7c8fd081-5cc2-41ac-8daa-34fdb59024eb\",\"MouseButton1\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription7815a17c-db7b-44f1-8d37-4f4abe1a140a\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton2\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription6c5331ad-732e-4a2b-b566-7dc446b35f0d\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton3\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescriptionaabc8b2b-49ae-48dd-adcf-2fc2ef068d09\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton4\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription89e8664b-29c9-4d0b-a4f8-e86805ed0485\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseButton5\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription7b3d0fd6-3dc6-400a-bc99-fa46ca932095\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelUp\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription888080de-7fd7-4b9d-828d-584ce0bfdd8e\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelDown\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription4975e8aa-c9a2-49f7-a34d-47932cbbac5c\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelLeft\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription57cbfaf3-19a0-4d4f-976c-36dad6e95c6c\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"MouseWheelRight\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.NothingMapping, YMouseButtonControl.Core\",\"Index\":0,\"Enabled\":false,\"Description\":\"** No Change (Don't Intercept) **\",\"PriorityDescription\":\"PriorityDescription8d04cfce-5361-4027-a790-fa996eccab2a\",\"Keys\":null,\"State\":false,\"CanRaiseDialog\":false,\"SimulatedKeystrokesType\":{\"$type\":\"YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes.StickyHoldActionType, YMouseButtonControl.Core\",\"Index\":7,\"Description\":\"Sticky (held down until button is pressed again)\",\"ShortDescription\":\"sticky hold\",\"Enabled\":true},\"MouseButtonDisabled\":true},\"Description\":\"Descriptionb0ba4f6e-4929-433e-8cb1-a8aa256ef7f3\",\"WindowCaption\":\"WindowCaption41783e0a-bbd3-4c80-8053-c1662788849d\",\"Process\":\"Process9952adfe-db98-4cfc-90a7-a0e20f58aebc\",\"WindowClass\":\"WindowClass31f013da-d1bc-4c83-a67d-ee4175db575e\",\"ParentClass\":\"ParentClassb6be68c5-5be1-4ef6-928a-cb8e6e8426bf\",\"MatchType\":\"MatchTypedbacc055-e328-4a76-adcc-b29127ab72d1\"}";
        const string path = "tmp.json";
        var deserialized = JsonConvert.DeserializeObject<Profile>(
            data,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
        );
        File.WriteAllText(path, data);
        var pf = GetProfilesService();
        var psdvm = new ProcessSelectorDialogViewModel(Substitute.For<IProcessMonitorService>());
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
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
        var addProfile = new AddProfile(pf);
        var sut = new ProfilesListViewModel(pf, psdvm, addProfile);
        sut.ShowImportFileDialog.RegisterHandler(x => x.SetOutput(null));

        sut.ImportCommand.Execute(Unit.Default).Subscribe();

        Assert.That(pf.Profiles, Has.Count.EqualTo(1));
        pf.Profiles.Should().NotContainNulls();
    }
}
