using Newtonsoft.Json;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;

[JsonObject(MemberSerialization.OptOut)]
public class InAnotherThreadPressedActionType
    : ISimulatedKeystrokesType,
        IEquatable<InAnotherThreadPressedActionType>
{
    public int Index { get; } = 3;
    public string Description { get; } = "In another thread as mouse button is pressed";
    public string ShortDescription { get; } = "thread-down";
    public bool Enabled { get; } = false;

    public override string ToString()
    {
        return $"{Index + 1} {Description}";
    }

    public bool Equals(InAnotherThreadPressedActionType? other)
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
        return Equals((InAnotherThreadPressedActionType)obj);
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
