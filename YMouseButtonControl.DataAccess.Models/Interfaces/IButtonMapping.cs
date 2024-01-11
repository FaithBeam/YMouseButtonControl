namespace YMouseButtonControl.DataAccess.Models.Interfaces;

public interface IButtonMapping
{
    public int Index { get; }
    public string Description { get; }
    public string PriorityDescription { get; }
    // public IEnumerable<IParsedEvent>? Sequence { get; }
    // public bool State { get; set; }
    public bool CanRaiseDialog { get; }
    public bool MouseButtonDisabled { get; }
    
    // public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }
}