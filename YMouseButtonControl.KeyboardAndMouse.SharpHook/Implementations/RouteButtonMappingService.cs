using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations
{
    public class RouteButtonMappingService : IRouteButtonMappingService
    {
        private readonly ISimulatedKeystrokesService _simulatedKeystrokesService;
        private readonly IMouseCoordinatesService _mouseCoordinatesService;

        public RouteButtonMappingService(ISimulatedKeystrokesService simulatedKeystrokesService, IMouseCoordinatesService mouseCoordinatesService)
        {
            _simulatedKeystrokesService = simulatedKeystrokesService;
            _mouseCoordinatesService = mouseCoordinatesService;
        }

        public void Route(IButtonMapping mapping, MouseButtonState state)
        {
            switch (mapping)
            {
                case SimulatedKeystrokes keystrokes:
                    _simulatedKeystrokesService.SimulatedKeystrokes(keystrokes, state);
                    break;
            }
        }
    }
}