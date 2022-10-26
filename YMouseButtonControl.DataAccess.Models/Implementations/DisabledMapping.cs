using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations;

public class DisabledMapping : IButtonMapping
{
    public int Index { get; } = 1;
    public bool Enabled { get; }
    public string Description { get; } = "Disabled";
    public bool CanRaiseDialog { get; set; } = false;
    public string Keys { get; }
    public bool State { get; set; }
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }
    
    public override string ToString()
    {
        return Description;
    }
}