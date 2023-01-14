namespace YMouseButtonControl.DataAccess.Models.Interfaces;

public interface IButtonMapping
{
    public int Index { get; }
    public bool Enabled { get; }
    public string Description { get; }
    public string PriorityDescription { get; set; }
    public string Keys { get; }
    public bool State { get; set; }
    public bool CanRaiseDialog { get; }
    public bool MouseButtonDisabled { get; set; }
    
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }
}