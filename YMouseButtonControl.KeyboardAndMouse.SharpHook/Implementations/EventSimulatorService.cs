using System;
using System.Collections.Generic;
using System.Threading;
using Serilog;
using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class EventSimulatorService(IEventSimulator eventSimulator) : IEventSimulatorService
{
    private readonly ILogger _logger = Log.Logger.ForContext<EventSimulatorService>();

    public void SimulateMousePress(MouseButton mb)
    {
        var t = new Thread(() => eventSimulator.SimulateMousePress(mb));
        t.Start();
    }

    public void SimulateMouseRelease(MouseButton mb)
    {
        var t = new Thread(() => eventSimulator.SimulateMouseRelease(mb));
        t.Start();
    }

    public SimulateKeyboardResult SimulateKeyPress(string? key)
    {
        _logger.Information("Simulate press {Key}", key);
        return new SimulateKeyboardResult
        {
            Result = eventSimulator
                .SimulateKeyPress(KeyCodes[key ?? throw new NullReferenceException(key)])
                .ToString()
        };
    }

    public SimulateKeyboardResult SimulateKeyRelease(string? key)
    {
        _logger.Information("Simulate release {Key}", key);
        return new SimulateKeyboardResult
        {
            Result = eventSimulator
                .SimulateKeyRelease(KeyCodes[key ?? throw new NullReferenceException(key)])
                .ToString()
        };
    }

    /// <summary>
    /// Press then release
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public SimulateKeyboardResult SimulateKeyTap(string? key)
    {
        _logger.Information("Simulate key tap {Key}", key);
        var keyCode = KeyCodes[key ?? throw new NullReferenceException(key)];
        eventSimulator.SimulateKeyPress(keyCode);
        eventSimulator.SimulateKeyRelease(keyCode);
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
            switch (pk.Value)
            {
                case <= 5:
                    SimulateMousePress((MouseButton)pk.Value);
                    break;
                case >= (int)KeyCode.VcEscape:
                    SimulateKeyPress(pk.Key);
                    break;
                default:
                    throw new Exception($"Unknown key value {pk.Value}");
            }
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
            switch (pk.Value)
            {
                case <= 5:
                    SimulateMouseRelease((MouseButton)pk.Value);
                    break;
                case >= (int)KeyCode.VcEscape:
                    SimulateKeyRelease(pk.Key);
                    break;
                default:
                    throw new Exception($"Unknown key value {pk.Value}");
            }
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
                    switch (poppedPk.Value)
                    {
                        case <= 5:
                            SimulateMouseRelease((MouseButton)poppedPk.Value);
                            break;
                        case >= (int)KeyCode.VcEscape:
                            SimulateKeyRelease(poppedPk.Key);
                            break;
                        default:
                            throw new Exception($"Unknown key value {poppedPk.Value}");
                    }
                }
            }

            stack.Push(pk);
            switch (pk.Value)
            {
                case <= (ushort)MouseButton.Button5:
                    SimulateMousePress((MouseButton)pk.Value);
                    break;
                case >= (int)KeyCode.VcEscape:
                    SimulateKeyPress(pk.Key);
                    break;
                default:
                    throw new Exception($"Unknown key value {pk.Value}");
            }
        }

        // Release any remaining keys in the stack
        while (stack.TryPop(out var poppedPk))
        {
            switch (poppedPk.Value)
            {
                case <= 5:
                    SimulateMouseRelease((MouseButton)poppedPk.Value);
                    break;
                case >= (int)KeyCode.VcEscape:
                    SimulateKeyRelease(poppedPk.Key);
                    break;
                default:
                    throw new Exception($"Unknown key value {poppedPk.Value}");
            }
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
                    {
                        continue;
                    }

                    var substr = keys.Substring(i + 1, j - i - 1).ToLower();
                    var pk = new ParsedKey { Key = substr };
                    if (MouseButtons.TryGetValue(substr, out var mbVal))
                    {
                        pk.Value = (ushort)mbVal;
                        pk.IsModifier = false;
                    }
                    else if (KeyCodes.TryGetValue(substr, out var kcVal))
                    {
                        pk.Value = (ushort)kcVal;
                        pk.IsModifier = true;
                    }
                    else
                    {
                        throw new Exception($"Error parsing keys, {substr}");
                    }
                    newKeys.Add(pk);
                    i = j;
                    break;
                }

                i += 1;
            }
            else
            {
                var substr = keys[i].ToString().ToLower();
                newKeys.Add(
                    new ParsedKey
                    {
                        Key = substr,
                        IsModifier = false,
                        Value = (ushort)KeyCodes[substr]
                    }
                );
                i++;
            }
        }

        return newKeys;
    }

    private static readonly Dictionary<string, KeyCode> KeyCodes =
        new()
        {
            { "esc", KeyCode.VcEscape },
            { "f1", KeyCode.VcF1 },
            { "f2", KeyCode.VcF2 },
            { "f3", KeyCode.VcF3 },
            { "f4", KeyCode.VcF4 },
            { "f5", KeyCode.VcF5 },
            { "f6", KeyCode.VcF6 },
            { "f7", KeyCode.VcF7 },
            { "f8", KeyCode.VcF8 },
            { "f9", KeyCode.VcF9 },
            { "f10", KeyCode.VcF10 },
            { "f11", KeyCode.VcF11 },
            { "f12", KeyCode.VcF12 },
            { "f13", KeyCode.VcF13 },
            { "f14", KeyCode.VcF14 },
            { "f15", KeyCode.VcF15 },
            { "f16", KeyCode.VcF16 },
            { "f17", KeyCode.VcF17 },
            { "f18", KeyCode.VcF18 },
            { "f19", KeyCode.VcF19 },
            { "f20", KeyCode.VcF20 },
            { "f21", KeyCode.VcF21 },
            { "f22", KeyCode.VcF22 },
            { "f23", KeyCode.VcF23 },
            { "f24", KeyCode.VcF24 },
            // {"`", KeyCode.VcBackquote},

            { "1", KeyCode.Vc1 },
            { "2", KeyCode.Vc2 },
            { "3", KeyCode.Vc3 },
            { "4", KeyCode.Vc4 },
            { "5", KeyCode.Vc5 },
            { "6", KeyCode.Vc6 },
            { "7", KeyCode.Vc7 },
            { "8", KeyCode.Vc8 },
            { "9", KeyCode.Vc9 },
            { "0", KeyCode.Vc0 },
            { "-", KeyCode.VcMinus },
            { "=", KeyCode.VcEquals },
            { "backspace", KeyCode.VcBackspace },
            { "tab", KeyCode.VcTab },
            { "capslock", KeyCode.VcCapsLock },
            { "a", KeyCode.VcA },
            { "b", KeyCode.VcB },
            { "c", KeyCode.VcC },
            { "d", KeyCode.VcD },
            { "e", KeyCode.VcE },
            { "f", KeyCode.VcF },
            { "g", KeyCode.VcG },
            { "h", KeyCode.VcH },
            { "i", KeyCode.VcI },
            { "j", KeyCode.VcJ },
            { "k", KeyCode.VcK },
            { "l", KeyCode.VcL },
            { "m", KeyCode.VcM },
            { "n", KeyCode.VcN },
            { "o", KeyCode.VcO },
            { "p", KeyCode.VcP },
            { "q", KeyCode.VcQ },
            { "r", KeyCode.VcR },
            { "s", KeyCode.VcS },
            { "t", KeyCode.VcT },
            { "u", KeyCode.VcU },
            { "v", KeyCode.VcV },
            { "w", KeyCode.VcW },
            { "x", KeyCode.VcX },
            { "y", KeyCode.VcY },
            { "z", KeyCode.VcZ },
            { "[", KeyCode.VcOpenBracket },
            { "]", KeyCode.VcCloseBracket },
            // {"\\", KeyCode.VcBackSlash},

            { ";", KeyCode.VcSemicolon },
            { "\"", KeyCode.VcQuote },
            { "return", KeyCode.VcEnter },
            { ",", KeyCode.VcComma },
            { ".", KeyCode.VcPeriod },
            { "/", KeyCode.VcSlash },
            { "space", KeyCode.VcSpace },
            { " ", KeyCode.VcSpace },
            { "prtscn", KeyCode.VcPrintScreen },
            { "scrolllock", KeyCode.VcScrollLock },
            { "pause", KeyCode.VcPause },
            // {"<", KeyCode.VcLesserGreater},

            { "ins", KeyCode.VcInsert },
            { "del", KeyCode.VcDelete },
            { "home", KeyCode.VcHome },
            { "end", KeyCode.VcEnd },
            { "pgup", KeyCode.VcPageUp },
            { "pgdn", KeyCode.VcPageDown },
            { "up", KeyCode.VcUp },
            { "left", KeyCode.VcLeft },
            // {"clear", KeyCode.VcClear},
            { "right", KeyCode.VcRight },
            { "down", KeyCode.VcDown },
            { "numlock", KeyCode.VcNumLock },
            { "num/", KeyCode.VcNumPadDivide },
            { "num*", KeyCode.VcNumPadMultiply },
            { "num-", KeyCode.VcNumPadSubtract },
            { "num=", KeyCode.VcNumPadEquals },
            { "num+", KeyCode.VcNumPadAdd },
            { "numenter", KeyCode.VcNumPadEnter },
            { "num.", KeyCode.VcNumPadSeparator },
            { "num1", KeyCode.VcNumPad1 },
            { "num2", KeyCode.VcNumPad2 },
            { "num3", KeyCode.VcNumPad3 },
            { "num4", KeyCode.VcNumPad4 },
            { "num5", KeyCode.VcNumPad5 },
            { "num6", KeyCode.VcNumPad6 },
            { "num7", KeyCode.VcNumPad7 },
            { "num8", KeyCode.VcNumPad8 },
            { "num9", KeyCode.VcNumPad9 },
            { "num0", KeyCode.VcNumPad0 },
            // {"numend", KeyCode.VcNumPadEnd},
            // {"numdown", KeyCode.VcNumPadDown},
            // {"numpgdn", KeyCode.VcNumPadPageDown},
            // {"numleft", KeyCode.VcNumPadLeft},
            { "numclear", KeyCode.VcNumPadClear },
            // {"numright", KeyCode.VcNumPadRight},
            // {"numhome", KeyCode.VcNumPadHome},
            // {"numup", KeyCode.VcNumPadUp},
            // {"numpgup", KeyCode.VcNumPadPageUp},
            // {"numins", KeyCode.VcNumPadInsert},
            // {"numdel", KeyCode.VcNumPadDelete},

            { "shift", KeyCode.VcLeftShift },
            { "rshift", KeyCode.VcRightShift },
            { "ctrl", KeyCode.VcLeftControl },
            { "rctrl", KeyCode.VcRightControl },
            { "alt", KeyCode.VcLeftAlt },
            { "ralt", KeyCode.VcRightAlt },
            { "lwin", KeyCode.VcLeftMeta },
            { "rwin", KeyCode.VcRightMeta },
            { "apps", KeyCode.VcContextMenu },
            { "power", KeyCode.VcPower },
            { "sleep", KeyCode.VcSleep },
            // { "wake", KeyCode.VcWake },
            { "mediaplay", KeyCode.VcMediaPlay },
            { "mediastop", KeyCode.VcMediaStop },
            { "mediaprev", KeyCode.VcMediaPrevious },
            { "medianext", KeyCode.VcMediaNext },
            { "mediaselect", KeyCode.VcMediaSelect },
            { "mediaeject", KeyCode.VcMediaEject },
            { "mute", KeyCode.VcVolumeMute },
            { "vol+", KeyCode.VcVolumeUp },
            { "vol-", KeyCode.VcVolumeDown },
            { "appmail", KeyCode.VcAppMail },
            { "appcalculator", KeyCode.VcAppCalculator },
            // {"appmusic", KeyCode.VcAppMusic},
            // {"apppictures", KeyCode.VcAppPictures},

            { "search", KeyCode.VcBrowserSearch },
            { "webhome", KeyCode.VcBrowserHome },
            { "back", KeyCode.VcBrowserBack },
            { "forward", KeyCode.VcBrowserForward },
            { "stop", KeyCode.VcBrowserStop },
            { "refresh", KeyCode.VcBrowserRefresh },
            { "favorites", KeyCode.VcBrowserFavorites },
            { "katakana", KeyCode.VcKatakana },
            { "_", KeyCode.VcUnderscore },
            // {"furigana", KeyCode.VcFurigana},
            { "kanji", KeyCode.VcKanji },
            { "hiragana", KeyCode.VcHiragana },
            { "yen", KeyCode.VcYen },
            // {"numpadcomma", KeyCode.VcNumPadComma},

            // {"sunhelp", KeyCode.VcSunHelp},

            // {"sunstop", KeyCode.VcSunStop},
            // {"sunprops", KeyCode.VcSunProps},
            // {"sunfront", KeyCode.VcSunFront},
            // {"sunopen", KeyCode.VcSunOpen},
            // {"sunfind", KeyCode.VcSunFind},
            // {"sunagain", KeyCode.VcSunAgain},
            // {"sunundo", KeyCode.VcSunUndo},
            // {"suncopy", KeyCode.VcSunCopy},
            // {"suninsert", KeyCode.VcSunInsert},
            // {"suncut", KeyCode.VcSunCut},

            { "undefined", KeyCode.VcUndefined },
            // {"charundefined", KeyCode.CharUndefined},
        };

    private static readonly Dictionary<string, MouseButton> MouseButtons =
        new()
        {
            { "lmb", MouseButton.Button1 },
            { "rmb", MouseButton.Button2 },
            { "mmb", MouseButton.Button3 },
            { "mb4", MouseButton.Button4 },
            { "mb5", MouseButton.Button5 }
        };
}
