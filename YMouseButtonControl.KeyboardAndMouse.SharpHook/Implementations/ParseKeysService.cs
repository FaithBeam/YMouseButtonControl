﻿using System.Collections.Generic;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

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
    public List<string> ParseKeys(string keys)
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