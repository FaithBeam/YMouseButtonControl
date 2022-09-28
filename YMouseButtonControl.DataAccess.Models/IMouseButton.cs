namespace YMouseButtonControl.DataAccess.Models;

public interface IMouseButton
{
    public int Index { get; set; }
    public string Description { get; set; }
    public bool CanRaiseDialog { get; set; }
    public string Keys { get; set; }
    public IButtonMapping ButtonMapping { get; set; }
}