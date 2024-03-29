using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

namespace YMouseButtonControl.Services.Windows.Implementations;

// This class handles the payload injection into the target process
public class Payload : IDisposable
{
    private readonly uint _targetpid;
    private readonly string _dllPath =
        @"C:\Users\Tech\RiderProjects\YMouseButtonControl\x64\Debug\YMouseButtonControl.Services.Windows.ProcessPayload.dll";
    private nint _dllAddr;
    private nint _functionCallbackAddr;
    private nint _functionUpdateDisabledKeysAddr;
    private nint _hHook;
    private uint _threadId;
    private uint WM_QUIT = 0x0012;
    private Profile _profile;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private delegate void UpdateDisabledKeys(string key, uint disabled);

    public Payload(uint pId, Profile profile)
    {
        _targetpid = pId;
        _profile = profile;
        Setup();
    }

    public void Dispose()
    {
        if (_hHook != IntPtr.Zero)
        {
            if (!WinApi.PostThreadMessage(_threadId, WM_QUIT, 0, 0))
            {
                throw new Exception($"ERROR POSTING WM_CLOSE TO {_threadId}");
            }

            if (!WinApi.UnhookWindowsHookEx(_hHook))
            {
                throw new Exception($"ERROR UNHOOKING WINDOWS HOOK: {_hHook}");
            }
            _hHook = 0;
        }
        if (_dllAddr != IntPtr.Zero)
        {
            if (!WinApi.FreeLibrary(_dllAddr))
            {
                throw new Exception($"ERROR FREEING LIBRARY: {_dllAddr}");
            }
        }
    }

    // This method loads the dll and gets the process addresses
    private void Setup()
    {
        _dllAddr = WinApi.LoadLibrary(_dllPath);
        if (_dllAddr == IntPtr.Zero)
        {
            throw new Exception($"ERROR LOADING LIBRARY: {_dllPath}");
        }

        _functionCallbackAddr = WinApi.GetProcAddress(_dllAddr, "start_callback");
        if (_functionCallbackAddr == IntPtr.Zero)
        {
            throw new Exception($"ERROR FINDING ADDRESS: {_dllAddr}");
        }

        _functionUpdateDisabledKeysAddr = WinApi.GetProcAddress(_dllAddr, "update_disabled_keys");
        if (_functionUpdateDisabledKeysAddr == IntPtr.Zero)
        {
            throw new Exception($"ERROR FINDING ADDRESS FOR UPDATE_DISABLED_KEYS");
        }

        var updateDisabledKeys = Marshal.GetDelegateForFunctionPointer<UpdateDisabledKeys>(
            _functionUpdateDisabledKeysAddr
        );

        Trace.WriteLine(Convert.ToUInt32(_profile.MouseButton4.MouseButtonDisabled));

        updateDisabledKeys("lmb", Convert.ToUInt32(_profile.MouseButton1.MouseButtonDisabled));
        updateDisabledKeys("rmb", Convert.ToUInt32(_profile.MouseButton2.MouseButtonDisabled));
        updateDisabledKeys("mmb", Convert.ToUInt32(_profile.MouseButton3.MouseButtonDisabled));
        updateDisabledKeys("mb4", Convert.ToUInt32(_profile.MouseButton4.MouseButtonDisabled));
        updateDisabledKeys("mb5", Convert.ToUInt32(_profile.MouseButton5.MouseButtonDisabled));
    }

    public void Run()
    {
        Trace.WriteLine("RUNNING");
        var t1 = new Thread(() =>
        {
            _threadId = WinApi.GetCurrentThreadId();
            Trace.WriteLine(_targetpid);
            _hHook = WinApi.SetWindowsHookEx(
                HookType.WH_MOUSE,
                _functionCallbackAddr,
                _dllAddr,
                _targetpid
            );
            if (_hHook == IntPtr.Zero)
            {
                var lastError = Marshal.GetLastWin32Error();
                throw new Exception($"ERROR SETTING WINDOWS HOOK: {lastError}");
            }

            int bRet;
            Trace.WriteLine("ABOUT TO GET MESSAGES");
            while ((bRet = WinApi.GetMessage(out MSG msg, 0, 0, 0)) != 0)
            {
                if (bRet == -1)
                {
                    return;
                }

                WinApi.TranslateMessage(ref msg);

                WinApi.DispatchMessage(ref msg);
            }
        });
        t1.Start();
    }
}
