using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Factories;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Tests;

[TestClass]
public class ButtonMappingFactoryTests
{
    [TestMethod]
    public void TestGetButtonMappings()
    {
        CollectionAssert.AllItemsAreInstancesOfType(ButtonMappingFactory.GetButtonMappings().ToList(), typeof(IButtonMapping));
    }

    [TestMethod]
    public void TestGetButtonMappingDescriptions()
    {
        CollectionAssert.AllItemsAreNotNull(ButtonMappingFactory.GetButtonMappingDescriptions().ToList());
    }

    [TestMethod]
    public void TestCreate()
    {
        Assert.IsInstanceOfType(ButtonMappingFactory.Create(ButtonMappings.Nothing), typeof(NothingMapping));
        Assert.IsInstanceOfType(ButtonMappingFactory.Create(ButtonMappings.SimulatedKeystrokes), typeof(SimulatedKeystrokes));
    }
}