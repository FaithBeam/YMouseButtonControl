using System.Collections.Generic;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class ParseKeysService : IParseKeysService
{
    /// <summary>
    /// Splits a string of characters into a list of strings. Words surrounded by {} will be added as the whole word.
    /// For example, {shift} will be "shift" in the list.
    /// "{SHIFT}abc" -> "shift", "a", "b", "c"
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public List<ParsedKey> ParseKeys(string? keys)
    {
        var newKeys = new List<ParsedKey>();
        var i = 0;
        while (i < keys?.Length)
        {
            if (keys[i] == '{')
            {
                for (var j = i; j < keys.Length; j++)
                {
                    if (keys[j] != '}')
                        continue;
                    newKeys.Add(
                        new ParsedKey
                        {
                            Key = keys.Substring(i + 1, j - i - 1).ToLower(),
                            IsModifier = true
                        }
                    );
                    i = j;
                    break;
                }

                i += 1;
            }
            else
            {
                newKeys.Add(
                    new ParsedKey { Key = keys[i].ToString().ToLower(), IsModifier = false }
                );
                i++;
            }
        }

        return newKeys;
    }
}
