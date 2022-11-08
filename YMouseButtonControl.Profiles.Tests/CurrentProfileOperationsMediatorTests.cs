using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Profiles.Implementations;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Tests;

[TestClass]
public class CurrentProfileOperationsMediatorTests
{
    [TestMethod]
    public void TestUpdateMouseButton1()
    {
        var events = new List<SelectedProfileEditedEventArgs>();
        var cpom = new CurrentProfileOperationsMediator();
        var profile = new Profile()
        {
            Name = "test",
            MouseButton1 = new DisabledMapping()
        };
        cpom.CurrentProfile = profile;
        cpom.CurrentProfileEdited += (sender, e) => events.Add(e);

        cpom.UpdateMouseButton1(new SimulatedKeystrokes{Keys = "w"});
        Assert.IsTrue(events.Count == 1);
        Assert.IsTrue(events[0].Button == MouseButton.MouseButton1);
        Assert.IsTrue(Equals(events[0].Mapping, profile.MouseButton1));
    }
}