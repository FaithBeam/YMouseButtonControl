using System;
using ReactiveUI;

namespace YMouseButtonControl.Core.DataAccess.Models.Implementations;

public class Setting : ReactiveObject, IEquatable<Setting>
{
    private string? _value;

    public int Id { get; set; }
    public required string Name { get; set; }

    public string? Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public bool Equals(Setting? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id == other.Id && Name == other.Name && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Setting)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Value);
    }
}
