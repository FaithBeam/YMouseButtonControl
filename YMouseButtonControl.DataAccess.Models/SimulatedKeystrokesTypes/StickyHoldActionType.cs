namespace YMouseButtonControl.DataAccess.Models.SimulatedKeystrokesTypes;

public class StickyHoldActionType: ISimulatedKeystrokesType
{
    public int Index { get; } = 8;
    public string Description { get; } = "Sticky (held down until button is pressed again)";
    public string ShortDescription { get; } = "sticky hold";
    public bool Enabled { get; } = true;
}