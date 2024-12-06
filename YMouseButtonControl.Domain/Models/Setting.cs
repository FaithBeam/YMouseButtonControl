namespace YMouseButtonControl.Domain.Models;

public abstract class Setting
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class SettingBool : Setting
{
    public bool BoolValue { get; set; }
}

public class SettingString : Setting
{
    public string? StringValue { get; set; }
}

public class SettingInt : Setting
{
    public int IntValue { get; set; }
}

public enum SettingType
{
    SettingBool = 1,
    SettingString = 2,
    SettingInt = 3,
}
