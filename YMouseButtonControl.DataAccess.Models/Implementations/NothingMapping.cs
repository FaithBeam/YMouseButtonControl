using Newtonsoft.Json;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations;

[JsonObject(MemberSerialization.OptOut)]
public class NothingMapping : INoSequenceMapping, IEquatable<NothingMapping>
{
    public int Index { get; }
    public string Description { get; } = "** No Change (Don't Intercept) **";
    public string PriorityDescription { get; } = string.Empty;
    public bool CanRaiseDialog { get; }
    public bool MouseButtonDisabled { get; }

    public override string ToString()
    {
        return Description;
    }

    public bool Equals(NothingMapping? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Index == other.Index && Description == other.Description &&
               PriorityDescription == other.PriorityDescription && CanRaiseDialog == other.CanRaiseDialog &&
               MouseButtonDisabled == other.MouseButtonDisabled;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((NothingMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Index;
            hashCode = (hashCode * 397) ^ Description.GetHashCode();
            hashCode = (hashCode * 397) ^ PriorityDescription.GetHashCode();
            hashCode = (hashCode * 397) ^ CanRaiseDialog.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseButtonDisabled.GetHashCode();
            return hashCode;
        }
    }
}