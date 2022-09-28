namespace YMouseButtonControl.DataAccess.Models;

public interface IButtonMapping
{
    public int Index { get; }
    public bool Enabled { get; }
    public string Description { get; }
    public string ToString();
    public void Run();
    public void Stop();
}