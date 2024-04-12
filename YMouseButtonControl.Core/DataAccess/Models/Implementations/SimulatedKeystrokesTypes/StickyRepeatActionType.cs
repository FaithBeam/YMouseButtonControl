using System;
using Newtonsoft.Json;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;

[JsonObject(MemberSerialization.OptOut)]
public class StickyRepeatActionType : ISimulatedKeystrokesType, IEquatable<StickyRepeatActionType>
{
    public int Index { get; } = 6;
    public string Description { get; } = "Sticky (repeatedly until button is pressed again)";
    public string ShortDescription { get; } = "sticky repeat";
    public bool Enabled { get; } = true;

    public override string ToString()
    {
        return $"{Index + 1} {Description}";
    }

    public bool Equals(StickyRepeatActionType? other)
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
        return Equals((StickyRepeatActionType)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Index;
            hashCode = (hashCode * 397) ^ Description.GetHashCode();
            hashCode = (hashCode * 397) ^ ShortDescription.GetHashCode();
            hashCode = (hashCode * 397) ^ Enabled.GetHashCode();
            return hashCode;
        }
    }
}
