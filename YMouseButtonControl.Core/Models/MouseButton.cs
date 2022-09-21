namespace YMouseButtonControl.Core.Models;

public class SimulatedKeystrokes : IMouseButton
{
    public int Index { get; set; }
    public string Description { get; set; }
    public bool CanRaiseDialog { get; set; }
    public string Keys { get; set; }
    public ISimulatedKeystrokesType SimulatedKeystrokesType { get; set; }
}