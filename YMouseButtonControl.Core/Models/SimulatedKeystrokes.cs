namespace YMouseButtonControl.Core.Models;

public class SimulatedKeystrokes : IButtonMapping
{
    public int Index { get; } = 2;
    public string Description = "Simulated Keys (undefined)";
    public bool Enabled { get; }

    public void Run()
    {
        throw new System.NotImplementedException();
    }

    public void Stop()
    {
        throw new System.NotImplementedException();
    }

    public bool CanRaiseDialog { get; set; }
    public string Keys { get; set; }

    public ISimulatedKeystrokesType SimulatedKeystrokesType { get; set; }

    public override string ToString()
    {
        var myStr = "";
        myStr = SimulatedKeystrokesType is not null
            ? $"Simulated Keys: ({SimulatedKeystrokesType.ShortDescription})"
            : Description;

        if (!string.IsNullOrWhiteSpace(Keys))
        {
            myStr += $"[{Keys}]";
        }

        return myStr;
    }
}