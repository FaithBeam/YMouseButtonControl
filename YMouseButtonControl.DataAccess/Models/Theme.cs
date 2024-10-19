namespace YMouseButtonControl.DataAccess.Models;

public class Theme
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Background { get; set; }
    public required string Highlight { get; set; }
}
