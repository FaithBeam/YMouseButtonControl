using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using ReactiveUI;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class CurrentWindowService : ReactiveObject, IDisposable, ICurrentWindowService
{
    private uint _threadId;
    private IntPtr _hEvent;
    private uint WM_QUIT = 0x0012;
    private Thread _thread;
    private string _foregroundWindow = string.Empty;

    public string ForegroundWindow
    {
        get => _foregroundWindow;
        private set => this.RaiseAndSetIfChanged(ref _foregroundWindow, value);
    }

    public void Dispose()
    {
        if (_hEvent == IntPtr.Zero)
        {
            return;
        }

        if (!WinApi.PostThreadMessage(_threadId, WM_QUIT, 0, 0))
        {
            throw new Exception($"ERROR POSTING WM_QUIT TO {_threadId}");
        }

        _hEvent = IntPtr.Zero;

        _thread.Join();
    }

    public void Run()
    {
        var winEvenCallbackDelegate = new WinApi.WinEventDelegate(WinEvenProcCallback);

        _thread = new Thread(() =>
        {
            _threadId = WinApi.GetCurrentThreadId();

            _hEvent = WinApi.SetWinEventHook(
                WinEvents.EVENT_SYSTEM_FOREGROUND,
                WinEvents.EVENT_SYSTEM_MINIMIZEEND,
                IntPtr.Zero,
                winEvenCallbackDelegate,
                0,
                0,
                WinEventFlags.WINEVENT_OUTOFCONTEXT | WinEventFlags.WINEVENT_SKIPOWNPROCESS
            );
            if (_hEvent == IntPtr.Zero)
            {
                Console.WriteLine("ERROR SETTING WINDOWS EVENT HOOK");
            }

            int bRet;
            while ((bRet = WinApi.GetMessage(out var msg, 0, 0, 0)) != 0)
            {
                if (bRet == -1)
                {
                    return;
                }

                WinApi.TranslateMessage(ref msg);

                WinApi.DispatchMessage(ref msg);
            }

            // Unsuccessful when attempting unhook. Is this necessary since we post WM_QUIT to this thread? Commented for now.
            //if (!WinApi.UnhookWinEvent(_hEvent))
            //{
            //    throw new Exception("ERROR UNHOOKING WIN EVENT");
            //}
        });
        _thread.Start();
    }

    private void WinEvenProcCallback(
        IntPtr hWinEventHook,
        uint eventType,
        IntPtr hwnd,
        int idObject,
        int idChild,
        uint dwEventThread,
        uint dwmsEventTime
    )
    {
        uint pId;
        switch ((WinEvents)eventType)
        {
            case WinEvents.EVENT_SYSTEM_FOREGROUND:
                var result = WinApi.GetWindowThreadProcessId(hwnd, out pId);
                var hProc = WinApi.OpenProcess(
                    (uint)(
                        ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead
                    ),
                    false,
                    pId
                );
                var sb = new StringBuilder(1024);
                result = WinApi.GetModuleFileNameEx(hProc, IntPtr.Zero, sb, sb.Capacity);
                ForegroundWindow = sb.ToString();
                break;
            case WinEvents.EVENT_SYSTEM_MINIMIZEEND:
                result = WinApi.GetWindowThreadProcessId(hwnd, out pId);
                hProc = WinApi.OpenProcess(
                    (uint)(
                        ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead
                    ),
                    false,
                    pId
                );
                sb = new StringBuilder(1024);
                result = WinApi.GetModuleFileNameEx(hProc, IntPtr.Zero, sb, sb.Capacity);
                //activeProc = sb.ToString();
                break;
            default:
                break;
        }
    }
}
