using System;
using System.Collections.Generic;
using YMouseButtonControl.Core.Models;

namespace YMouseButtonControl.Core.Factories;

public static class ButtonMappingFactory
{
    public static readonly List<string> ButtonMappings = new()
    {
        "** No Change (Don't Intercept) **",
        "Disable",
        "Simulated Keys (undefined)"
    };
}