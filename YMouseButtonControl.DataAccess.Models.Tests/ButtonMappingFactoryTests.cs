using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Factories;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Tests;

public class ButtonMappingFactoryTests
{
    [Test]
    public void TestGetButtonMappings()
    {
        CollectionAssert.AllItemsAreInstancesOfType(ButtonMappingFactory.GetButtonMappings().ToList(), typeof(IButtonMapping));
    }

    [Test]
    public void TestGetButtonMappingDescriptions()
    {
        CollectionAssert.AllItemsAreNotNull(ButtonMappingFactory.GetButtonMappingDescriptions().ToList());
    }

    [Test]
    public void TestCreate()
    {
        Assert.Multiple(() =>
        {
            Assert.That(ButtonMappingFactory.Create(ButtonMappings.Nothing), Is.InstanceOf<NothingMapping>());
            Assert.That(ButtonMappingFactory.Create(ButtonMappings.SimulatedKeystrokes), Is.InstanceOf<SimulatedKeystrokes>());
        });
    }
}