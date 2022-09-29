using System.Collections.Generic;

namespace YMouseButtonControl.ViewModels.Factories;

public static class ButtonMappingFactory
{
    public static readonly List<string> ButtonMappings = new()
    {
        "** No Change (Don't Intercept) **",
        "Disable",
        "Simulated Keys (undefined)"
    };
}