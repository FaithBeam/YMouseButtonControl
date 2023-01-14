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
                new object[] { MouseButton.MouseButton1, new DisabledMapping() },
                new object[] { MouseButton.MouseButton2, new DisabledMapping() },
                new object[] { MouseButton.MouseButton3, new DisabledMapping() },
                new object[] { MouseButton.MouseButton4, new DisabledMapping() },
                new object[] { MouseButton.MouseButton5, new DisabledMapping() },
                new object[] { MouseButton.MouseWheelUp, new DisabledMapping() },
                new object[] { MouseButton.MouseWheelLeft, new DisabledMapping() },
                new object[] { MouseButton.MouseWheelRight, new DisabledMapping() },
                new object[] { MouseButton.MouseWheelDown, new DisabledMapping() },
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
        var p = new Profile();
        switch (button)
        {
            case MouseButton.MouseButton1:
                p.MouseButton1 = mapping;
                break;
            case MouseButton.MouseButton2:
                p.MouseButton2 = mapping;
                break;
            case MouseButton.MouseButton3:
                p.MouseButton3 = mapping;
                break;
            case MouseButton.MouseButton4:
                p.MouseButton4 = mapping;
                break;
            case MouseButton.MouseButton5:
                p.MouseButton5 = mapping;
                break;
            case MouseButton.MouseWheelUp:
                p.MouseWheelUp = mapping;
                break;
            case MouseButton.MouseWheelDown:
                p.MouseWheelDown = mapping;
                break;
            case MouseButton.MouseWheelLeft:
                p.MouseWheelLeft = mapping;
                break;
            case MouseButton.MouseWheelRight:
                p.MouseWheelRight = mapping;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }
        rbms.Route(button, p);
        _autoMocker.VerifyAll();
    }
}