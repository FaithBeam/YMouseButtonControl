using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations
{
    public class RouteButtonMappingService : IRouteButtonMappingService
    {
        private readonly ISimulatedKeystrokesService _simulatedKeystrokesService;

        public RouteButtonMappingService(ISimulatedKeystrokesService simulatedKeystrokesService)
        {
            _simulatedKeystrokesService = simulatedKeystrokesService;
        }

        public void Route(IButtonMapping mapping, bool pressed)
        {
            switch (mapping)
            {
                case SimulatedKeystrokes:
                    _simulatedKeystrokesService.SimulatedKeystrokes(mapping, pressed);
                    break;
            }
        }
    }
}