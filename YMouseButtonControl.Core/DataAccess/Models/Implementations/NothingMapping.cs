using System;
using Newtonsoft.Json;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.Core.DataAccess.Models.Implementations;

[JsonObject(MemberSerialization.OptOut)]
public class NothingMapping : IButtonMapping, IEquatable<NothingMapping>
{
    public int Index { get; } = 0;
    public bool Enabled { get; } = false;
    public string Description { get; } = "** No Change (Don't Intercept) **";
    public string? PriorityDescription { get; set; }

    public override string ToString()
    {
        return Description;
    }

    public string? Keys { get; }
    public bool State { get; set; }
    public bool CanRaiseDialog { get; } = false;
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }
    public bool MouseButtonDisabled { get; set; } = false;

    public bool Equals(NothingMapping? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Index == other.Index
            && Enabled == other.Enabled
            && Description == other.Description
            && CanRaiseDialog == other.CanRaiseDialog
            && Keys == other.Keys
            && State == other.State
            && Equals(SimulatedKeystrokesType, other.SimulatedKeystrokesType);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((NothingMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Index;
            hashCode = (hashCode * 397) ^ Enabled.GetHashCode();
            hashCode = (hashCode * 397) ^ Description.GetHashCode();
            hashCode = (hashCode * 397) ^ CanRaiseDialog.GetHashCode();
            hashCode = (hashCode * 397) ^ (Keys != null ? Keys.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ State.GetHashCode();
            hashCode =
                (hashCode * 397)
                ^ (SimulatedKeystrokesType != null ? SimulatedKeystrokesType.GetHashCode() : 0);
            return hashCode;
        }
    }
}
