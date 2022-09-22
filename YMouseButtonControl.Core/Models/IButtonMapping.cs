namespace YMouseButtonControl.Core.Models;

public interface IButtonMapping
{
    public int Index { get; }
    public bool Enabled { get; }
    public void Run();
    public void Stop();
}