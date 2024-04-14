using System;
using Newtonsoft.Json;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.Core.DataAccess.Models.Implementations;

[JsonObject(MemberSerialization.OptOut)]
public class SimulatedKeystrokes : ReactiveObject, IButtonMapping, IEquatable<SimulatedKeystrokes>
{
    public int Index => 2;
    public string Description => "Simulated Keys (undefined)";
    public string? PriorityDescription { get; set; }
    public bool Enabled { get; }
    public string? Keys { get; set; }
    public bool State { get; set; }

    public bool CanRaiseDialog { get; } = true;
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }

    /// <summary>
    /// Prevent the original mouse button from going through (not YMouseButtonControl's simulated keystroke)
    /// </summary>
    public bool BlockOriginalMouseInput { get; set; } = true;

    public override string ToString()
    {
        var myStr = SimulatedKeystrokesType is not null
            ? $"Simulated Keys: ({SimulatedKeystrokesType.ShortDescription})"
            : Description;

        if (!string.IsNullOrWhiteSpace(PriorityDescription))
        {
            myStr += $"[{PriorityDescription}]";
        }
        else if (!string.IsNullOrWhiteSpace(Keys))
        {
            myStr += $"[{Keys}]";
        }

        return myStr;
    }

    public bool Equals(SimulatedKeystrokes? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Index == other.Index
            && Description == other.Description
            && Enabled == other.Enabled
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
        return Equals((SimulatedKeystrokes)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Index;
            hashCode = (hashCode * 397) ^ Description.GetHashCode();
            hashCode = (hashCode * 397) ^ Enabled.GetHashCode();
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
