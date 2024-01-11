using System;
using System.Collections.Generic;
using System.Linq;
using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.DataAccess.Models.Models;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;
using YMouseButtonControl.KeyboardAndMouse.Services;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulateKeyService : ISimulateKeyService
{
    private readonly IEventSimulator _sim;

    public SimulateKeyService(IEventSimulator sim)
    {
        _sim = sim;
    }

    public SimulateKeyboardResult SimulateKeyPress(KeyCode key) => new()
        { Result = _sim.SimulateKeyPress(key).ToString() };

    public SimulateKeyboardResult SimulateKeyRelease(KeyCode key) => new()
        { Result = _sim.SimulateKeyRelease(key).ToString() };

    public SimulateKeyboardResult SimulateMouseMovement(short x, short y) => new()
        { Result = _sim.SimulateMouseMovement(x, y).ToString() };
    
    public SimulateKeyboardResult SimulateMouseMovementRelativeToCursor(short x, short y) => new()
        { Result = _sim.SimulateMouseMovementRelative(x, y).ToString() };
    
    public UioHookResult SimulateMousePress(MouseButton mb)
    {
        return _sim.SimulateMousePress(mb);
    }
    
    public UioHookResult SimulateMousePress(short x, short y, MouseButton mb)
    {
        return _sim.SimulateMousePress(x, y, mb);
    }

    public UioHookResult SimulateMouseRelease(MouseButton mb)
    {
        return _sim.SimulateMouseRelease(mb);
    }
    
    public UioHookResult SimulateMouseRelease(short x, short y, MouseButton mb)
    {
        return _sim.SimulateMouseRelease(x, y, mb);
    }

    /// <summary>
    /// Press then release
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public SimulateKeyboardResult SimulateKeyTap(string key)
    {
        var keyCode = KeyCodeMappings.KeyCodes[key];
        _sim.SimulateKeyPress(keyCode);
        _sim.SimulateKeyRelease(keyCode);
        return new SimulateKeyboardResult { Result = "Success" };
    }

    public void PressKeys(IEnumerable<IParsedEvent> events)
    {
        foreach (var e in events)
        {
            switch (e)
            {
                case ParsedKey key:
                    SimulateKeyPress(key.KeyCode);
                    break;
                case ParsedMouseButton button:
                    SimulateMousePress(button.MouseButton);
                    break;
                case ParsedMouseMovement movement:
                    RouteMouseMovement(movement);
                    break;
            }
        }
    }

    /// <summary>
    /// Keys will be released in the reversed order they come. For example, sending abc to this method will release cba
    /// in that order.
    /// </summary>
    public void ReleaseKeys(IEnumerable<IParsedEvent> events)
    {
        var reversed = events.Reverse();
        foreach (var e in reversed)
        {
            switch (e)
            {
                case ParsedKey key:
                    SimulateKeyRelease(key.KeyCode);
                    break;
                case ParsedMouseButton button:
                    SimulateMouseRelease(button.MouseButton);
                    break;
            }
        }
    }

    public void TapKeys(IEnumerable<IParsedEvent> events)
    {
        var stack = new Stack<IParsedEvent>();
        foreach (var e in events)
        {
            if (e is ParsedMouseMovement movement)
            {
                RouteMouseMovement(movement);
                continue;
            }
            
            // Pop the entire stack if the last key pressed is a normal key
            if (stack.TryPeek(out var peekPk) && !peekPk.IsModifier)
            {
                while (stack.TryPop(out var poppedEvent))
                {
                    switch (poppedEvent)
                    {
                        case ParsedKey key:
                            SimulateKeyRelease(key.KeyCode);
                            break;
                        case ParsedMouseButton button:
                            SimulateMouseRelease(button.MouseButton);
                            break;
                    }
                }
            }

            // Push the event to the stack and press simulate the press
            stack.Push(e);
            switch (e)
            {
                case ParsedKey key:
                    SimulateKeyPress(key.KeyCode);
                    break;
                case ParsedMouseButton button:
                    SimulateMousePress(button.MouseButton);
                    break;
            }
        }

        // Release any remaining keys in the stack
        while (stack.TryPop(out var poppedEvent))
        {
            switch (poppedEvent)
            {
                case ParsedKey key:
                    SimulateKeyRelease(key.KeyCode);
                    break;
                case ParsedMouseButton button:
                    SimulateMouseRelease(button.MouseButton);
                    break;
            }
        }
    }

    public void RouteMouseMovement(ParsedMouseMovement movement)
    {
        switch (movement.MovementRelativeTo)
        {
            case MovementRelativeTo.PrimaryMonitor:
                SimulateMouseMovement(movement.X, movement.Y);
                break;
            case MovementRelativeTo.Cursor:
                SimulateMouseMovementRelativeToCursor(movement.X, movement.Y);
                break;
            case MovementRelativeTo.ActiveWindow:
                break;
            case MovementRelativeTo.PrimaryProfileWindow:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}