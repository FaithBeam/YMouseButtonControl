namespace YMouseButtonControl.Core.Models;

public interface ISimulatedKeystrokesType
{
    public int Index { get; }
    public string Description { get; }
    public string ShortDescription { get; }
    public bool Enabled { get; }
}