using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Models;

public class ParsedMouseButton : IParsedEvent, IEquatable<ParsedMouseButton>
{
    public string Event { get; set; } = string.Empty;
    public bool IsModifier { get; set; }
    public MouseButton MouseButton { get; set; }

    public override string ToString()
    {
        return Event;
    }

    public bool Equals(ParsedMouseButton? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Event == other.Event && IsModifier == other.IsModifier && MouseButton == other.MouseButton;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ParsedMouseButton)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Event.GetHashCode();
            hashCode = (hashCode * 397) ^ IsModifier.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)MouseButton;
            return hashCode;
        }
    }
}