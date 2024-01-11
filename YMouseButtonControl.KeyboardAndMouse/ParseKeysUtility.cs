using System;
using System.Collections.Generic;
using System.Linq;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Models.Models;
using YMouseButtonControl.KeyboardAndMouse.Services;

namespace YMouseButtonControl.KeyboardAndMouse;

public static class ParseKeysUtility
{
    private static readonly string[] Modifiers = {
        "ctrl",
        "rctrl",
        "alt",
        "ralt",
        "shift",
        "rshift"
    };

    private static readonly string[] MovementTags =
    {
        "madd",
        "mset"
    };
    
    /// <summary>
    /// Splits a string of characters into a list of strings. Words surrounded by {} will be added as the whole word.
    /// For example, {shift} will be "shift" in the list.
    /// "{SHIFT}abc" -> "shift", "a", "b", "c"
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static IEnumerable<IParsedEvent> ParseKeys(string keys)
    {
        var newEvents = new List<IParsedEvent>();
        var i = 0;
        
        while (i < keys.Length)
        {
            if (keys[i] == '{')
            {
                for (var j = i; j < keys.Length; j++)
                {
                    if (keys[j] != '}')
                    {
                        continue;
                    }

                    var parsed = keys.Substring(i + 1, j - i - 1).ToLower();
                    var isModifier = Modifiers.Contains(parsed);
                    
                    if (KeyCodeMappings.KeyCodes.TryGetValue(parsed, out var kc))
                    {
                        newEvents.Add(new ParsedKey
                        {
                            Event = keys.Substring(i, j - i + 1).ToLower(),
                            IsModifier = isModifier,
                            KeyCode = kc
                        });
                    }
                    else if (MouseButtonCodeMappings.MouseButtonCodes.TryGetValue(parsed, out var mb))
                    {
                        newEvents.Add(new ParsedMouseButton
                        {
                            Event = keys.Substring(i, j - i + 1).ToLower(),
                            IsModifier = isModifier,
                            MouseButton = mb
                        });
                    }
                    else if (parsed.Contains(":"))
                    {
                        var indexOfColon = parsed.IndexOf(":", StringComparison.Ordinal);
                        var sub = parsed.Substring(i, indexOfColon).ToLower();
                        if (MovementTags.Contains(sub))
                        {
                            newEvents.Add(new ParsedMouseMovement(keys.Substring(i, j - i + 1).ToLower()));
                        }
                    }
                    else
                    {
                        throw new Exception($"Couldn't parse {parsed}");
                    }
                    
                    i = j;
                    
                    break;
                }

                i += 1;
            }
            else
            {
                var ev = keys[i].ToString().ToLower();
                if (KeyCodeMappings.KeyCodes.TryGetValue(ev, out var kc))
                {
                    newEvents.Add(new ParsedKey
                    {
                        Event = keys[i].ToString().ToLower(),
                        IsModifier = false,
                        KeyCode = kc
                    });
                    i++;
                }
                else
                {
                    throw new Exception($"Couldn't parse {ev}");
                }
            }
        }

        return newEvents;
    }
}