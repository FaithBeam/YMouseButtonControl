using System.Text.RegularExpressions;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Models;

public enum MovementRelativeTo
{
    Cursor,
    PrimaryMonitor,
    ActiveWindow,
    PrimaryProfileWindow
}

public class ParsedMouseMovement : IParsedEvent
{
    public ParsedMouseMovement(string ev)
    {
        Event = ev;
        
        SetRelativity();
    }

    public string Event { get; set; }
    public bool IsModifier { get; set; }
    public MovementRelativeTo MovementRelativeTo { get; private set; }
    public short X { get; private set; }
    public short Y { get; private set; }

    public override string ToString()
    {
        return Event;
    }

    private void SetRelativity()
    {
        // Event can be null at startup, somehow
        if (Event is null)
        {
            return;
        }
        if (!Rx.IsMatch(Event))
        {
            return;
        }
        
        var match = Rx.Match(Event);
            
        if (MovementTagDict.TryGetValue(match.Groups[1].Value, out var value))
        {
            MovementRelativeTo = value;
        }

        X = short.Parse(match.Groups[2].Value);
        Y = short.Parse(match.Groups[3].Value);
    }
    
    private static Dictionary<string, MovementRelativeTo> MovementTagDict => new()
    {
        {"madd", MovementRelativeTo.Cursor},
        {"mset", MovementRelativeTo.PrimaryMonitor}
    };
    
    private static readonly Regex Rx = new(@"{(\w+):(\d+),(\d+)}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
}