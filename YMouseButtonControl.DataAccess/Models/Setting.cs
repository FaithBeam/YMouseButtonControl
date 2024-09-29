namespace YMouseButtonControl.DataAccess.Models;

public abstract class Setting
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class SettingBool : Setting
{
    public bool Value { get; set; }
}

public class SettingString : Setting
{
    public string? Value { get; set; }
}

public class SettingInt : Setting
{
    public int Value { get; set; }
}
