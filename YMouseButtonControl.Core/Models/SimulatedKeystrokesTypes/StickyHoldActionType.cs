namespace YMouseButtonControl.Core.Models.SimulatedKeystrokesTypes;

public class StickyHoldActionType: ISimulatedKeystrokesType
{
    public int Index { get; } = 8;
    public string Description { get; } = "Sticky (held down until button is pressed again)";
    public string ShortDescription { get; } = "sticky hold";
    public bool Enabled { get; } = true;
    public void Run()
    {
        throw new System.NotImplementedException();
    }

    public void Stop()
    {
        throw new System.NotImplementedException();
    }
}