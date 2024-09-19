using System;
using ReactiveUI;

namespace YMouseButtonControl.Core.ViewModels.Models;

public abstract class BaseSettingVm : ReactiveObject, IEquatable<BaseSettingVm>
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public bool Equals(BaseSettingVm? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id == other.Id && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((BaseSettingVm)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }
}

public class SettingBoolVm : BaseSettingVm, IEquatable<SettingBoolVm>
{
    private bool _value;
    public bool Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public bool Equals(SettingBoolVm? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return base.Equals(other) && _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((SettingBoolVm)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _value);
    }
}

public class SettingStringVm : BaseSettingVm, IEquatable<SettingStringVm>
{
    private string? _value;
    public string? Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public bool Equals(SettingStringVm? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return base.Equals(other) && _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((SettingStringVm)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _value);
    }
}
