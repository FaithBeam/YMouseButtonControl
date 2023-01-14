using System;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class RouteMouseButtonService : IRouteMouseButtonService
{
    private readonly IRouteButtonMappingService _routeButtonMappingService;

    public RouteMouseButtonService(IRouteButtonMappingService routeButtonMappingService)
    {
        _routeButtonMappingService = routeButtonMappingService;
    }

    public void Route(MouseButton b, Profile p)
    {
        switch (b)
        {
            case MouseButton.MouseButton1:
                _routeButtonMappingService.Route(p.MouseButton1, true);
                break;
            case MouseButton.MouseButton2:
                _routeButtonMappingService.Route(p.MouseButton2, true);
                break;
            case MouseButton.MouseButton3:
                _routeButtonMappingService.Route(p.MouseButton3, true);
                break;
            case MouseButton.MouseButton4:
                _routeButtonMappingService.Route(p.MouseButton4, true);
                break;
            case MouseButton.MouseButton5:
                _routeButtonMappingService.Route(p.MouseButton5, true);
                break;
            case MouseButton.MouseWheelUp:
                _routeButtonMappingService.Route(p.MouseWheelUp, true);
                break;
            case MouseButton.MouseWheelDown:
                _routeButtonMappingService.Route(p.MouseWheelDown, true);
                break;
            case MouseButton.MouseWheelLeft:
                _routeButtonMappingService.Route(p.MouseWheelLeft, true);
                break;
            case MouseButton.MouseWheelRight:
                _routeButtonMappingService.Route(p.MouseWheelRight, true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}