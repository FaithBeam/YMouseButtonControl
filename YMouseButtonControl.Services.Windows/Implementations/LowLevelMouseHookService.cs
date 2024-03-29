using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;

namespace YMouseButtonControl.Services.Windows.Implementations;

[SupportedOSPlatform("windows5.1.2600")]
public class LowLevelMouseHookService : IDisposable, ILowLevelMouseHookService
{
    private uint _threadId;
    private UnhookWindowsHookExSafeHandle? _hHook;
    private readonly IProfilesService _profilesService;
    private readonly ICurrentWindowService _currentWindowService;
    private string _foregroundWindow = string.Empty;
    private Thread? _thread;
    private readonly object _lockObj = new();

    private readonly ConcurrentDictionary<uint, bool> _buttonDisabledDict =
        new(
            new Dictionary<uint, bool>
            {
                { PInvoke.WM_LBUTTONDOWN, false },
                { PInvoke.WM_LBUTTONUP, false },
                { PInvoke.WM_RBUTTONDOWN, false },
                { PInvoke.WM_RBUTTONUP, false },
                { PInvoke.WM_MBUTTONDOWN, false },
                { PInvoke.WM_MBUTTONUP, false },
                { PInvoke.XBUTTON1, false },
                { PInvoke.XBUTTON2, false }
            }
        );

    public LowLevelMouseHookService(
        IProfilesService profilesService,
        ICurrentWindowService currentWindowService
    )
    {
        _profilesService = profilesService;
        _currentWindowService = currentWindowService;
        this.WhenAnyValue(x => x._currentWindowService.ForegroundWindow)
            .DistinctUntilChanged()
            .Subscribe(OnForegroundWindowChanged);
        _profilesService
            .Profiles.ToObservableChangeSet(x => x)
            .ToCollection()
            .Subscribe(OnProfilesChanged);
        UpdateDisabledButtons();
    }

    public void Run()
    {
        _thread = new Thread(() =>
        {
            _threadId = PInvoke.GetCurrentThreadId();

            _hHook = PInvoke.SetWindowsHookEx(
                WINDOWS_HOOK_ID.WH_MOUSE_LL,
                LowLevelMouseCallback,
                null,
                0
            );

            if (_hHook is null || _hHook.IsInvalid)
            {
                throw new Exception("ERROR SETTING WINDOWS HOOK");
            }

            int bRet;
            while ((bRet = PInvoke.GetMessage(out _, HWND.Null, 0, 0)) != 0)
            {
                if (bRet == -1)
                {
                    return;
                }
            }
        });
        _thread.Start();
    }

    public void Dispose()
    {
        if (_hHook is null || _hHook.IsClosed || _hHook.IsInvalid)
        {
            return;
        }

        _hHook.Close();

        if (PInvoke.PostThreadMessage(_threadId, PInvoke.WM_QUIT, 0, 0))
        {
            throw new Exception($"ERROR POSTING WM_QUIT TO {_threadId}");
        }

        _thread?.Join();
    }

    private void OnForegroundWindowChanged(string foregroundWindow)
    {
        lock (_lockObj)
        {
            _foregroundWindow = foregroundWindow;
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
                _buttonDisabledDict[PInvoke.WM_LBUTTONDOWN] = true;
                _buttonDisabledDict[PInvoke.WM_LBUTTONUP] = true;
            }
            else
            {
                _buttonDisabledDict[PInvoke.WM_LBUTTONDOWN] = false;
                _buttonDisabledDict[PInvoke.WM_LBUTTONUP] = false;
            }

            if (p.MouseButton2.MouseButtonDisabled)
            {
                _buttonDisabledDict[PInvoke.WM_RBUTTONDOWN] = true;
                _buttonDisabledDict[PInvoke.WM_RBUTTONUP] = true;
            }
            else
            {
                _buttonDisabledDict[PInvoke.WM_RBUTTONDOWN] = false;
                _buttonDisabledDict[PInvoke.WM_RBUTTONUP] = false;
            }

            if (p.MouseButton3.MouseButtonDisabled)
            {
                _buttonDisabledDict[PInvoke.WM_MBUTTONDOWN] = true;
                _buttonDisabledDict[PInvoke.WM_MBUTTONUP] = true;
            }
            else
            {
                _buttonDisabledDict[PInvoke.WM_MBUTTONDOWN] = false;
                _buttonDisabledDict[PInvoke.WM_MBUTTONUP] = false;
            }

            if (p.MouseButton4.MouseButtonDisabled)
            {
                _buttonDisabledDict[PInvoke.XBUTTON1] = true;
            }
            else
            {
                _buttonDisabledDict[PInvoke.XBUTTON1] = false;
            }

            if (p.MouseButton5.MouseButtonDisabled)
            {
                _buttonDisabledDict[PInvoke.XBUTTON2] = true;
            }
            else
            {
                _buttonDisabledDict[PInvoke.XBUTTON2] = false;
            }
        }
    }

    private LRESULT HandleButton(uint button, int nCode, WPARAM wParam, LPARAM lParam) =>
        _buttonDisabledDict[button]
            ? new LRESULT(-1)
            : PInvoke.CallNextHookEx(null, nCode, wParam, lParam);

    private LRESULT HandleNormalButton(int nCode, WPARAM wParam, LPARAM lParam) =>
        HandleButton((uint)wParam, nCode, wParam, lParam);

    private LRESULT HandleXButton(int nCode, WPARAM wParam, LPARAM lParam)
    {
        var xmbStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
        var xmb = HiWord(xmbStruct.mouseData);
        return HandleButton(xmb, nCode, wParam, lParam);
    }

    private LRESULT LowLevelMouseCallback(int code, WPARAM wParam, LPARAM lParam)
    {
        if (code < 0)
        {
            return PInvoke.CallNextHookEx(null, code, wParam, lParam);
        }

        switch ((uint)wParam)
        {
            case PInvoke.WM_LBUTTONDOWN:
            case PInvoke.WM_LBUTTONUP:
            case PInvoke.WM_RBUTTONDOWN:
            case PInvoke.WM_RBUTTONUP:
            case PInvoke.WM_MBUTTONDOWN:
            case PInvoke.WM_MBUTTONUP:
                return HandleNormalButton(code, wParam, lParam);
            case PInvoke.WM_XBUTTONDOWN:
            case PInvoke.WM_XBUTTONUP:
                return HandleXButton(code, wParam, lParam);
        }

        return PInvoke.CallNextHookEx(null, code, wParam, lParam);
    }

    private static uint HiWord(uint num) => num >> 16;
}
