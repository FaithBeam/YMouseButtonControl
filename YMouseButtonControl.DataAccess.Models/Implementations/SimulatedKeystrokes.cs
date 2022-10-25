using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations;

public class SimulatedKeystrokes : IButtonMapping
{
    public int Index { get; } = 2;
    public string Description { get; }= "Simulated Keys (undefined)";
    public bool Enabled { get; }
    public bool CanRaiseDialog { get; set; }
    public string? Keys { get; set; }
    public bool State { get; set; }

    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }

    public override string ToString()
    {
        var myStr = SimulatedKeystrokesType is not null
            ? $"Simulated Keys: ({SimulatedKeystrokesType.ShortDescription})"
            : Description;

        if (!string.IsNullOrWhiteSpace(Keys))
        {
            myStr += $"[{Keys}]";
        }

        return myStr;
    }
}