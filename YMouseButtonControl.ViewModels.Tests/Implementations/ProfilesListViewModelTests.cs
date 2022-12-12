using Moq;
using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;

namespace YMouseButtonControl.ViewModels.Tests.Implementations;

[TestClass]
public class ProfilesListViewModelTests
{
    [TestMethod]
    public void TestRemoveButtonClicked()
    {
        var p1 = new Profile
        {
            Id = 1,
            Name = "Default"
        };
        var mocker = new AutoMocker();
        var psMock = mocker.GetMock<IProfilesService>();
        psMock.SetupGet(x => x.CurrentProfile).Returns(p1);
        mocker.Use(psMock.Object);
        var plvm = mocker.CreateInstance<ProfilesListViewModel>();
        plvm.RemoveButtonCommand.Execute(null);
        psMock.Verify(x => x.RemoveProfile(p1), Times.Once);
    }

    [TestMethod]
    public void TestEditButtonClickedAsync()
    {
        var mocker = new AutoMocker();
        var psMock = mocker.GetMock<IProfilesService>();
        var curProf = new Profile { Name = "Test" };
        psMock.SetupGet(x => x.CurrentProfile).Returns(curProf);
        mocker.Use(psMock);
        var plvm = mocker.CreateInstance<ProfilesListViewModel>();
        var newProf = new Profile { Name = "New", Process = "someproc.exe", Description = "some desc"};
        plvm.ShowProcessSelectorInteraction.RegisterHandler(x => x.SetOutput(newProf));
        plvm.EditButtonCommand.Execute().Subscribe();
        psMock.Verify(x => x.ReplaceProfile(curProf, newProf));
    }
}