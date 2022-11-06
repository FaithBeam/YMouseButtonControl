using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations;

public class SimulatedKeystrokes : IButtonMapping, IEquatable<SimulatedKeystrokes>
{
    public int Index { get; } = 2;
    public string Description { get; }= "Simulated Keys (undefined)";
    public bool Enabled { get; }
    public bool HasBeenRaised { get; set; } = false;
    public string? Keys { get; set; }
    public bool State { get; set; }

    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }

    public override string ToString()
    {
        var myStr = SimulatedKeystrokesType is not null
            ? $"Simulated Keys: ({SimulatedKeystrokesType.ShortDescription})"
            : Description;

        if (!string.IsNullOrWhiteSpace(Keys))
        {
            myStr += $"[{Keys}]";
        }

        return myStr;
    }

    public bool Equals(SimulatedKeystrokes? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Index == other.Index && Description == other.Description && Enabled == other.Enabled && HasBeenRaised == other.HasBeenRaised && Keys == other.Keys && State == other.State && Equals(SimulatedKeystrokesType, other.SimulatedKeystrokesType);
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
            var hashCode = Index;
            hashCode = (hashCode * 397) ^ Description.GetHashCode();
            hashCode = (hashCode * 397) ^ Enabled.GetHashCode();
            hashCode = (hashCode * 397) ^ HasBeenRaised.GetHashCode();
            hashCode = (hashCode * 397) ^ (Keys != null ? Keys.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ State.GetHashCode();
            hashCode = (hashCode * 397) ^ (SimulatedKeystrokesType != null ? SimulatedKeystrokesType.GetHashCode() : 0);
            return hashCode;
        }
    }
}