using Newtonsoft.Json;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations;

[JsonObject(MemberSerialization.OptOut)]
public class SimulatedKeystrokes : ISequencedMapping, IEquatable<SimulatedKeystrokes>
{
    public int Index => 2;
    public string Description => "Simulated Keys (undefined)";
    public string PriorityDescription { get; set; } = string.Empty;
    public bool Enabled { get; }
    public bool CanRaiseDialog { get; } = true;
    public bool MouseButtonDisabled { get; set; } = true;
    public IEnumerable<IParsedEvent> Sequence { get; set; } = new List<IParsedEvent>();
    public bool State { get; set; }
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }

    public override string ToString()
    {
        var myStr = SimulatedKeystrokesType is not null
            ? $"Simulated Keys: ({SimulatedKeystrokesType.ShortDescription})"
            : Description;

        if (!string.IsNullOrWhiteSpace(PriorityDescription))
        {
            myStr += $"[{PriorityDescription}]";
        }
        else if (Sequence.Any())
        {
            myStr += $"[{string.Join("", Sequence)}]";
        }

        return myStr;
    }

    public bool Equals(SimulatedKeystrokes? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return PriorityDescription == other.PriorityDescription && Enabled == other.Enabled &&
               CanRaiseDialog == other.CanRaiseDialog && MouseButtonDisabled == other.MouseButtonDisabled &&
               Sequence.SequenceEqual(other.Sequence) &&
               State == other.State && Equals(SimulatedKeystrokesType, other.SimulatedKeystrokesType);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SimulatedKeystrokes)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = PriorityDescription.GetHashCode();
            hashCode = (hashCode * 397) ^ Enabled.GetHashCode();
            hashCode = (hashCode * 397) ^ CanRaiseDialog.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseButtonDisabled.GetHashCode();
            hashCode = (hashCode * 397) ^ Sequence.GetHashCode();
            hashCode = (hashCode * 397) ^ State.GetHashCode();
            hashCode = (hashCode * 397) ^ (SimulatedKeystrokesType != null ? SimulatedKeystrokesType.GetHashCode() : 0);
            return hashCode;
        }
    }
}