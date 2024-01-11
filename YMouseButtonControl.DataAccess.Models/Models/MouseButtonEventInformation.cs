namespace YMouseButtonControl.DataAccess.Models.Models;

public class MouseButtonEventInformation : IEquatable<MouseButtonEventInformation>
{
    public short StartX { get; set; }
    public short EndX { get; set; }
    public short StartY { get; set; }
    public short EndY { get; set; }

    public bool Equals(MouseButtonEventInformation? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return StartX == other.StartX && EndX == other.EndX && StartY == other.StartY && EndY == other.EndY;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MouseButtonEventInformation)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = StartX.GetHashCode();
            hashCode = (hashCode * 397) ^ EndX.GetHashCode();
            hashCode = (hashCode * 397) ^ StartY.GetHashCode();
            hashCode = (hashCode * 397) ^ EndY.GetHashCode();
            return hashCode;
        }
    }
}