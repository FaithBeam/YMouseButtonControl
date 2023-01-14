using System.Collections.Generic;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public static class ParseKeysService
{
    public static List<string> ParseKeys(string keys)
    {
        var newKeys = new List<string>();
        var i = 0;
        while (i < keys.Length)
        {
            if (keys[i] == '{')
            {
                for (var j = i; j < keys.Length; j++)
                {
                    if (keys[j] != '}') continue;
                    newKeys.Add(keys.Substring(i + 1, j - 1).ToLower());
                    i = j;
                    break;
                }

                i += 1;
            }
            else
            {
                newKeys.Add(keys[i].ToString().ToLower());
                i++;
            }
        }

        return newKeys;
    }
}