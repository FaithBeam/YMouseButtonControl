namespace YMouseButtonControl.Services.Abstractions.Models;

public class ProcessModel
{
    public string? ProcessName { get; set; }
    public string? WindowTitle { get; set; }
    public uint ProcessId { get; set; }
    public string? BitmapPath { get; set; }
}