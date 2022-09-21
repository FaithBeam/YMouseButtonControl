namespace YMouseButtonControl.Core.Models;

public interface ISimulatedKeystrokesType
{
    public int Index { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public bool Enabled { get; set; }
    public void Run();
    public void Stop();
}