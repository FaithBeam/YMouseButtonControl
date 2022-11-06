using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.DataAccess.Models.Tests;

[TestClass]
public class ProfileTests
{
    [TestMethod]
    public void TestProfile()
    {
        var p1 = new Profile
        {
            Name = "MyName"
        };
        var p2 = new Profile
        {
            Name = "MyName"
        };
        
        Assert.AreEqual(p1, p2);

        p2.Name = "New Name";
        Assert.AreNotEqual(p1, p2);

        p1.MouseButton1 = new DisabledMapping();
        p2.Name = "MyName";
        Assert.AreNotEqual(p1, p2);

        p2.MouseButton1 = new DisabledMapping();
        Assert.AreEqual(p1, p2);

        p1.MouseButton2 = new SimulatedKeystrokes
        {
            Keys = "w",
            State = false,
            HasBeenRaised = true,
            SimulatedKeystrokesType = new StickyHoldActionType()
        };
        Assert.AreNotEqual(p1, p2);
        
        p2.MouseButton2 = new SimulatedKeystrokes
        {
            Keys = "w",
            State = false,
            HasBeenRaised = true,
            SimulatedKeystrokesType = new StickyHoldActionType()
        };
        Assert.AreEqual(p1, p2);

        p2.MouseButton2 = new SimulatedKeystrokes
        {
            Keys = "W",
            State = false,
            HasBeenRaised = true,
            SimulatedKeystrokesType = new StickyHoldActionType()
        };
        Assert.AreNotEqual(p1, p2);
    }
}