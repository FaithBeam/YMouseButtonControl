using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;

public class StickyHoldActionType: ISimulatedKeystrokesType
{
    public int Index { get; } = 8;
    public string Description { get; } = "Sticky (held down until button is pressed again)";
    public string ShortDescription { get; } = "sticky hold";
    public bool Enabled { get; } = true;
    public override string ToString()
    {
        return Description;
    }
}