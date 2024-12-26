namespace YMouseButtonControl.Domain.Models;

public class Profile
{
    public int Id { get; set; }
    public bool IsDefault { get; set; }
    public bool Checked { get; set; }
    public int DisplayPriority { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string WindowCaption { get; set; }
    public required string Process { get; set; }
    public required string WindowClass { get; set; }
    public required string ParentClass { get; set; }
    public required string MatchType { get; set; }

    public virtual ICollection<ButtonMapping> ButtonMappings { get; set; } = [];
}
