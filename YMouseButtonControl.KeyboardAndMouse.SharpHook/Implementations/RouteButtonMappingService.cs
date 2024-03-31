﻿using Serilog;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations
{
    public class RouteButtonMappingService : IRouteButtonMappingService
    {
        private readonly ISimulatedKeystrokesService _simulatedKeystrokesService;
        private readonly ILogger _log = Log.Logger.ForContext<RouteButtonMappingService>();

        public RouteButtonMappingService(ISimulatedKeystrokesService simulatedKeystrokesService)
        {
            _simulatedKeystrokesService = simulatedKeystrokesService;
        }

        public void Route(IButtonMapping mapping, MouseButtonState state)
        {
            switch (mapping)
            {
                case SimulatedKeystrokes:
                    _log.Information("Route button mapping {Mapping}, {State}", mapping, state);
                    _simulatedKeystrokesService.SimulatedKeystrokes(mapping, state);
                    break;
            }
        }
    }
}
