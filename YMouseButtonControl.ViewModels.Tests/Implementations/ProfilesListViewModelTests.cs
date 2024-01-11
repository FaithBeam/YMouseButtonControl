using System.Collections.ObjectModel;
using NSubstitute;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.ViewModels.Tests.Implementations;

public class ProfilesListViewModelTests
{
    [Test]
    public void TestRemoveButtonClicked_RemoveProfileCalled()
    {
        var p1 = new Profile
        {
            Id = 1,
            Name = "Default"
        };
        var psM = Substitute.For<IProfilesService>();
        psM.CurrentProfile.Returns(p1);
        psM.Profiles.Returns(new ObservableCollection<Profile>{p1});
        psM.CurrentProfileIndex.Returns(0);
        var plvm = new ProfilesListViewModel(psM, Substitute.For<IProcessSelectorDialogViewModel>());
        
        plvm.RemoveButtonCommand.Execute().Subscribe();
        
        psM.Received(1).RemoveProfile(Arg.Is(p1));
    }
    
    [Test]
    public void TestEditButtonClickedAsync_ReplaceProfileCalled()
    {
        var curProf = new Profile { Name = "Test" };
        var newProf = new Profile { Name = "New", Process = "someproc.exe", Description = "some desc"};
        var psM = Substitute.For<IProfilesService>();
        psM.CurrentProfile.Returns(curProf);
        psM.Profiles.Returns(new ObservableCollection<Profile>{curProf});
        psM.CurrentProfileIndex.Returns(0);
        var plvm = new ProfilesListViewModel(psM, Substitute.For<IProcessSelectorDialogViewModel>());
        plvm.ShowProcessSelectorInteraction.RegisterHandler(x => x.SetOutput(newProf));
        
        plvm.EditButtonCommand.Execute().Subscribe();
        
        psM.ReceivedWithAnyArgs(1).ReplaceProfile(default, default);
    }
}