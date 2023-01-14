using Moq.AutoMock;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Tests;

[TestClass]
public class RouteMouseButtonServiceTests
{
    private AutoMocker _autoMocker;

    public static IEnumerable<object[]> Data
    {
        get
        {
            return new[]
            {
                new object[] { MouseButton.MouseButton1, new NothingMapping() },
                new object[] { MouseButton.MouseButton2, new NothingMapping() },
                new object[] { MouseButton.MouseButton3, new NothingMapping() },
                new object[] { MouseButton.MouseButton4, new NothingMapping() },
                new object[] { MouseButton.MouseButton5, new NothingMapping() },
                new object[] { MouseButton.MouseWheelUp, new NothingMapping() },
                new object[] { MouseButton.MouseWheelLeft, new NothingMapping() },
                new object[] { MouseButton.MouseWheelRight, new NothingMapping() },
                new object[] { MouseButton.MouseWheelDown, new NothingMapping() },
            };
        }
    }
    
    [TestInitialize]
    public void TestInitialize()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    [DynamicData(nameof(Data))]
    public void Route(MouseButton button, IButtonMapping mapping)
    {
        _autoMocker.Setup<IRouteButtonMappingService>(x => x.Route(mapping, true)).Verifiable();
        var rbms = _autoMocker.CreateInstance<RouteMouseButtonService>();
        rbms.Route(button, new Profile{MouseButton1 = mapping});
        _autoMocker.VerifyAll();
    }
}