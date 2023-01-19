using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Avalonia.Collections;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class LowLevelMouseHookService : IDisposable, ILowLevelMouseHookService
{
    private uint _threadId;
    private IntPtr _hHook;
    private static uint XBUTTON1 = 1;
    private static uint XBUTTON2 = 2;
    private uint WM_QUIT = 0x0012;
    private readonly IProfilesService _profilesService;
    private readonly ICurrentWindowService _currentWindowService;
    private string _foregroundWindow = string.Empty;
    private Thread _thread;
    private readonly object _lockObj = new();

    private readonly ConcurrentDictionary<uint, bool> _buttonDisabledDict = new(
        new Dictionary<uint, bool>
    {
        { (uint)WM.LBUTTONDOWN, false },
        { (uint)WM.LBUTTONUP, false },
        { (uint)WM.RBUTTONDOWN, false },
        { (uint)WM.RBUTTONUP, false },
        { (uint)WM.MBUTTONDOWN, false },
        { (uint)WM.MBUTTONUP, false },
        { XBUTTON1, false },
        { XBUTTON2, false }
    });

    public LowLevelMouseHookService(IProfilesService profilesService, ICurrentWindowService currentWindowService)
    {
        _profilesService = profilesService;
        _currentWindowService = currentWindowService;
        _currentWindowService.OnActiveWindowChangedEventHandler += OnForegroundWindowChanged;
        _profilesService.Profiles
            .ToObservableChangeSet(x => x)
            .ToCollection()
            .Subscribe(OnProfilesChanged);
        UpdateDisabledButtons();
    }

    private void OnForegroundWindowChanged(object sender, ActiveWindowChangedEventArgs e)
    {
        lock (_lockObj)
        {
            _foregroundWindow = e.ActiveWindow;
        }

        // Reset mouse buttons back to non blocking state when foreground window changes
        foreach (var k in _buttonDisabledDict.Keys)
        {
            _buttonDisabledDict[k] = false;
        }
        
        UpdateDisabledButtons();
    }
    
    private void OnProfilesChanged(IReadOnlyCollection<Profile> profiles)
    {
        Trace.WriteLine("UPDATE DISABLED BUTTONS");
        UpdateDisabledButtons();
    }

    private void UpdateDisabledButtons()
    {
        foreach (var p in _profilesService.Profiles)
        {
            if (!_foregroundWindow.Contains(p.Process))
            {
                continue;
            }
            
            if (p.MouseButton1.MouseButtonDisabled)
            {
                _buttonDisabledDict[(uint)WM.LBUTTONDOWN] = true;
                _buttonDisabledDict[(uint)WM.LBUTTONUP] = true;
            }
            else
            {
                _buttonDisabledDict[(uint)WM.LBUTTONDOWN] = false;
                _buttonDisabledDict[(uint)WM.LBUTTONUP] = false;
            }
            
            if (p.MouseButton2.MouseButtonDisabled)
            {
                _buttonDisabledDict[(uint)WM.RBUTTONDOWN] = true;
                _buttonDisabledDict[(uint)WM.RBUTTONUP] = true;
            }
            else
            {
                _buttonDisabledDict[(uint)WM.RBUTTONDOWN] = false;
                _buttonDisabledDict[(uint)WM.RBUTTONUP] = false;
            }
            
            if (p.MouseButton3.MouseButtonDisabled)
            {
                _buttonDisabledDict[(uint)WM.MBUTTONDOWN] = true;
                _buttonDisabledDict[(uint)WM.MBUTTONUP] = true;
            }
            else
            {
                _buttonDisabledDict[(uint)WM.MBUTTONDOWN] = false;
                _buttonDisabledDict[(uint)WM.MBUTTONUP] = false;
            }
            
            if (p.MouseButton4.MouseButtonDisabled)
            {
                _buttonDisabledDict[XBUTTON1] = true;
            }
            else
            {
                _buttonDisabledDict[XBUTTON1] = false;
            }
            
            if (p.MouseButton5.MouseButtonDisabled)
            {
                _buttonDisabledDict[XBUTTON2] = true;
            }
            else
            {
                _buttonDisabledDict[XBUTTON2] = false;
            }
        }
    }

    public void Run()
    {
        _thread = new Thread(() =>
        {
            _threadId = WinApi.GetCurrentThreadId();

            _hHook = WinApi.SetWindowsHookEx(HookType.WH_MOUSE_LL, LowLevelMouseCallback, IntPtr.Zero, 0);
            if (_hHook == IntPtr.Zero)
            {
                throw new Exception("ERROR SETTING WINDOWS HOOK");
            }

            int bRet;
            while ((bRet = WinApi.GetMessage(out MSG msg, 0,0,0)) != 0)
            {
                if (bRet == -1)
                {
                    return;
                }
            }
        });
        _thread.Start();
    }

    private nint HandleButton(uint button, int nCode, nint wParam, nint lParam)
    {
        if (_buttonDisabledDict[button])
        {
            return -1;
        }

        return WinApi.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    private nint HandleNormalButton(int nCode, nint wParam, nint lParam)
    {
        return HandleButton((uint)wParam, nCode, wParam, lParam);
    }

    private nint HandleXButton(int nCode, nint wParam, nint lParam)
    {
        var xmbStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
        var xmb = HiWord(xmbStruct.mouseData);
        return HandleButton((uint)xmb, nCode, wParam, lParam);
    }

    private IntPtr LowLevelMouseCallback(int code, nint wParam, nint lParam)
    {
        if (code < 0)
        {
            return WinApi.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        switch ((WM)wParam)
        {
            case WM.LBUTTONDOWN:
            case WM.LBUTTONUP:
            case WM.RBUTTONDOWN:
            case WM.RBUTTONUP:
            case WM.MBUTTONDOWN:
            case WM.MBUTTONUP:
                return HandleNormalButton(code, wParam, lParam);
            case WM.XBUTTONDOWN:
            case WM.XBUTTONUP:
                return HandleXButton(code, wParam, lParam);
        }

        return WinApi.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
    }

    private int HiWord(int num)
    {
        return num >> 16;
    }

    public void Dispose()
    {
        if (_hHook == IntPtr.Zero)
        {
            return;
        }

        if (!WinApi.UnhookWindowsHookEx(_hHook))
        {
            throw new Exception("ERROR UNHOOKING WINDOWS HOOK");
        }

        if (!WinApi.PostThreadMessage(_threadId, WM_QUIT, 0, 0))
        {
            throw new Exception($"ERROR POSTING WM_QUIT TO {_threadId}");
        }

        _currentWindowService.OnActiveWindowChangedEventHandler -= OnForegroundWindowChanged;
        
        _hHook = IntPtr.Zero;

        _thread.Join();
    }
}