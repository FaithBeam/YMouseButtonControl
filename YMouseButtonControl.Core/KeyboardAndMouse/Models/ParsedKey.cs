namespace YMouseButtonControl.Core.KeyboardAndMouse.Models;

public class ParsedKey
{
    public string? Key { get; set; }
    public bool IsModifier { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var pk = (ParsedKey)obj;
        return (Key == pk.Key) && (IsModifier == pk.IsModifier);
    }

    protected bool Equals(ParsedKey other)
    {
        return Key == other.Key && IsModifier == other.IsModifier;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Key != null ? Key.GetHashCode() : 0) * 397) ^ IsModifier.GetHashCode();
        }
    }
}
