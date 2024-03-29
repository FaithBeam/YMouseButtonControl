﻿using Newtonsoft.Json;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations;

[JsonObject(MemberSerialization.OptOut)]
public class DisabledMapping : INoSequenceMapping, IEquatable<DisabledMapping>
{
    public int Index { get; } = 1;
    public bool Enabled { get; }
    public string Description { get; } = "Disabled";
    public string PriorityDescription { get; set; } = string.Empty;
    public bool CanRaiseDialog { get; }
    public bool MouseButtonDisabled { get; set; } = true;

    public override string ToString()
    {
        return Description;
    }

    public bool Equals(DisabledMapping? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Index == other.Index && Enabled == other.Enabled && Description == other.Description &&
               PriorityDescription == other.PriorityDescription && CanRaiseDialog == other.CanRaiseDialog &&
               MouseButtonDisabled == other.MouseButtonDisabled;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DisabledMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Index;
            hashCode = (hashCode * 397) ^ Enabled.GetHashCode();
            hashCode = (hashCode * 397) ^ Description.GetHashCode();
            hashCode = (hashCode * 397) ^ PriorityDescription.GetHashCode();
            hashCode = (hashCode * 397) ^ CanRaiseDialog.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseButtonDisabled.GetHashCode();
            return hashCode;
        }
    }
}