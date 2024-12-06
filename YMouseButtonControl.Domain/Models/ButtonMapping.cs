namespace YMouseButtonControl.Domain.Models;

public enum MouseButton
{
    Mb1,
    Mb2,
    Mb3,
    Mb4,
    Mb5,
    Mwu,
    Mwd,
    Mwl,
    Mwr,
}

public abstract class ButtonMapping
{
    public int Id { get; set; }
    public string? Keys { get; set; }
    public required MouseButton MouseButton { get; set; }
    public int ProfileId { get; set; }
    public virtual Profile? Profile { get; set; }
    public SimulatedKeystrokeType? SimulatedKeystrokeType { get; set; }
    public int? AutoRepeatDelay { get; set; }
    public bool? AutoRepeatRandomizeDelayEnabled { get; set; }
    public bool Selected { get; set; }
    public bool? BlockOriginalMouseInput { get; set; }
    public ButtonMappingType ButtonMappingType { get; set; }
}

public class DisabledMapping : ButtonMapping;

public class NothingMapping : ButtonMapping;

public class SimulatedKeystroke : ButtonMapping
{
    public SimulatedKeystroke()
    {
        BlockOriginalMouseInput = true;
    }
}

public class RightClick : ButtonMapping;

public enum ButtonMappingType
{
    Disabled,
    Nothing,
    SimulatedKeystroke,
    RightClick,
}
