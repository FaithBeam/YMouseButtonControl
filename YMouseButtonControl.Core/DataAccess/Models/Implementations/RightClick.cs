using System;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.Core.DataAccess.Models.Implementations;

public class RightClick : ReactiveObject, IButtonMapping, IEquatable<RightClick>
{
    public int Index => 3;
    public bool Enabled { get; }
    public string? Description { get; } = "Right Click";
    public string? PriorityDescription { get; set; }
    public string? Keys { get; } = null;
    public bool State { get; set; }
    public bool CanRaiseDialog { get; } = false;
    public bool BlockOriginalMouseInput { get; set; } = true;
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }

    public override string ToString() => "Right Click";

    public bool Equals(RightClick? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Enabled == other.Enabled
            && Description == other.Description
            && PriorityDescription == other.PriorityDescription
            && Keys == other.Keys
            && State == other.State
            && CanRaiseDialog == other.CanRaiseDialog
            && BlockOriginalMouseInput == other.BlockOriginalMouseInput
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
        return Equals((RightClick)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Enabled,
            Description,
            PriorityDescription,
            Keys,
            State,
            CanRaiseDialog,
            BlockOriginalMouseInput,
            SimulatedKeystrokesType
        );
    }
}
