namespace YMouseButtonControl.DataAccess.Models.Interfaces;

public interface IButtonMapping
{
    public int Index { get; }
    public bool Enabled { get; }
    public string Description { get; }
    public string ToString();
    public string Keys { get; }
    public bool State { get; set; }
}