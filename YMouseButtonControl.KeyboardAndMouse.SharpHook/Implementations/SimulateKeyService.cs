using System;
using System.Collections.Generic;
using SharpHook;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulateKeyService : ISimulateKeyService
{
    private readonly IEventSimulator _keyboardSimulator;

    public SimulateKeyService(IEventSimulator keyboardSimulator)
    {
        _keyboardSimulator = keyboardSimulator;
    }

    public SimulateKeyboardResult SimulateKeyPress(string? key) =>
        new()
        {
            Result = _keyboardSimulator
                .SimulateKeyPress(
                    KeyCodeMappings.KeyCodes[key ?? throw new NullReferenceException(key)]
                )
                .ToString()
        };

    public SimulateKeyboardResult SimulateKeyRelease(string? key) =>
        new()
        {
            Result = _keyboardSimulator
                .SimulateKeyRelease(
                    KeyCodeMappings.KeyCodes[key ?? throw new NullReferenceException(key)]
                )
                .ToString()
        };

    /// <summary>
    /// Press then release
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public SimulateKeyboardResult SimulateKeyTap(string? key)
    {
        var keyCode = KeyCodeMappings.KeyCodes[key ?? throw new NullReferenceException(key)];
        _keyboardSimulator.SimulateKeyPress(keyCode);
        _keyboardSimulator.SimulateKeyRelease(keyCode);
        return new SimulateKeyboardResult { Result = "Success" };
    }

    /// <summary>
    /// Keys to be pressed in order.
    /// </summary>
    /// <param name="keys">Keys to be pressed</param>
    public void PressKeys(string? keys)
    {
        foreach (var pk in ParseKeys(keys))
        {
            SimulateKeyPress(pk.Key);
        }
    }

    /// <summary>
    /// Keys will be released in the reversed order they come. For example, sending abc to this method will release cba
    /// in that order.
    /// </summary>
    /// <param name="keys">Keys to be released</param>
    public void ReleaseKeys(string? keys)
    {
        var parsed = ParseKeys(keys);
        parsed.Reverse();
        foreach (var pk in parsed)
        {
            SimulateKeyRelease(pk.Key);
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="keys"></param>
    public void TapKeys(string? keys)
    {
        var parsed = ParseKeys(keys);
        var stack = new Stack<ParsedKey>();

        foreach (var pk in parsed)
        {
            // Pop the entire stack if the last key pressed is a normal key
            if (stack.TryPeek(out var peekPk) && !peekPk.IsModifier)
            {
                while (stack.TryPop(out var poppedPk))
                {
                    SimulateKeyRelease(poppedPk.Key);
                }
            }

            stack.Push(pk);
            SimulateKeyPress(pk.Key);
        }

        // Release any remaining keys in the stack
        while (stack.TryPop(out var poppedPk))
        {
            SimulateKeyRelease(poppedPk.Key);
        }
    }

    /// <summary>
    /// Splits a string of characters into a list of strings. Words surrounded by {} will be added as the whole word.
    /// For example, {shift} will be "shift" in the list.
    /// "{SHIFT}abc" -> "shift", "a", "b", "c"
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    private static List<ParsedKey> ParseKeys(string? keys)
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
