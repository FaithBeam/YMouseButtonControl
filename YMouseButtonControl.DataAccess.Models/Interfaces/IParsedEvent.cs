namespace YMouseButtonControl.DataAccess.Models.Interfaces;

public interface IParsedEvent
{
    string Event { get; set; }
    bool IsModifier { get; set; }
}