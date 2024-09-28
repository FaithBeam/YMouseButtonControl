using System;
using Newtonsoft.Json;
using ReactiveUI;

namespace YMouseButtonControl.Core.ViewModels.Models;

[JsonObject(MemberSerialization.OptOut)]
public abstract class BaseSimulatedKeystrokeTypeVm
    : ReactiveObject,
        IEquatable<BaseSimulatedKeystrokeTypeVm>
{
    private int _index;
    private string? _description;
    private string? _shortDescription;
    private bool _enabled;
    public int Index
    {
        get => _index;
        set => this.RaiseAndSetIfChanged(ref _index, value);
    }
    public string? Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    public string? ShortDescription
    {
        get => _shortDescription;
        set => this.RaiseAndSetIfChanged(ref _shortDescription, value);
    }
    public bool Enabled
    {
        get => _enabled;
        set => this.RaiseAndSetIfChanged(ref _enabled, value);
    }

    public override string ToString() => $"{Index + 1} {Description}";

    public bool Equals(BaseSimulatedKeystrokeTypeVm? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Index == other.Index
            && Description == other.Description
            && ShortDescription == other.ShortDescription
            && Enabled == other.Enabled;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((BaseSimulatedKeystrokeTypeVm)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Index, Description, ShortDescription, Enabled);
    }
}

public class AsMousePressedAndReleasedActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public AsMousePressedAndReleasedActionTypeVm()
    {
        Index = 8;
        Description = "As mouse button is pressed & when released";
        ShortDescription = "pressed & released";
        Enabled = true;
    }
}

public class DuringMouseActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public DuringMouseActionTypeVm()
    {
        Index = 2;
        Description = "During (press on down, release on up)";
        ShortDescription = "during";
        Enabled = true;
    }
}

public class InAnotherThreadPressedActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public InAnotherThreadPressedActionTypeVm()
    {
        Index = 3;
        Description = "In another thread as mouse button is pressed";
        ShortDescription = "thread-down";
        Enabled = false;
    }
}

public class InAnotherThreadReleasedActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public InAnotherThreadReleasedActionTypeVm()
    {
        Index = 4;
        Description = "In another thread as mouse button is released";
        ShortDescription = "thread-up";
        Enabled = false;
    }
}

public class MouseButtonPressedActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public MouseButtonPressedActionTypeVm()
    {
        Index = 0;
        Description = "As mouse button is pressed";
        ShortDescription = "pressed";
        Enabled = true;
    }
}

public class MouseButtonReleasedActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public MouseButtonReleasedActionTypeVm()
    {
        Index = 1;
        Description = "As mouse button is released";
        ShortDescription = "released";
        Enabled = true;
    }
}

public class RepeatedlyWhileButtonDownActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public RepeatedlyWhileButtonDownActionTypeVm()
    {
        Index = 5;
        Description = "Repeatedly while the button is down";
        ShortDescription = "repeat";
        Enabled = true;
    }
}

public class StickyHoldActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public StickyHoldActionTypeVm()
    {
        Index = 7;
        Description = "Sticky (held down until button is pressed again)";
        ShortDescription = "sticky hold";
        Enabled = true;
    }
}

public class StickyRepeatActionTypeVm : BaseSimulatedKeystrokeTypeVm
{
    public StickyRepeatActionTypeVm()
    {
        Index = 6;
        Description = "Sticky (repeatedly until button is pressed again)";
        ShortDescription = "sticky repeat";
        Enabled = true;
    }
}
