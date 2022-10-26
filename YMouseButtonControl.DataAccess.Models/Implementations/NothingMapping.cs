using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations;

public class NothingMapping : IButtonMapping
{
    public int Index { get; } = 0;
    public bool Enabled { get; } = false;
    public string Description { get; } = "** No Change (Don't Intercept) **";
    public bool CanRaiseDialog { get; } = false;

    public override string ToString()
    {
        return Description;
    }

    public string? Keys { get; }
    public bool State { get; set; }
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }
}