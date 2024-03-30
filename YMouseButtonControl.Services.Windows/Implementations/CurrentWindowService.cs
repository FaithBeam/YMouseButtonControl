using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using ReactiveUI;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Threading;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.Services.Windows.Implementations;

[SupportedOSPlatform("windows5.1.2600")]
public class CurrentWindowService : ReactiveObject, IDisposable, ICurrentWindowService
{
    private uint _threadId;
    private UnhookWinEventSafeHandle? _hEvent;
    private Thread? _thread;
    private string _foregroundWindow = string.Empty;

    public string ForegroundWindow
    {
        get => _foregroundWindow;
        private set => this.RaiseAndSetIfChanged(ref _foregroundWindow, value);
    }

    public void Dispose()
    {
        if (_hEvent is null)
        {
            throw new Exception("Win event null");
        }

        if (!PInvoke.PostThreadMessage(_threadId, PInvoke.WM_QUIT, 0, 0))
        {
            throw new Exception($"ERROR POSTING WM_QUIT TO {_threadId}");
        }

        _hEvent.Dispose();

        _thread?.Join();
    }

    public unsafe void Run()
    {
        _thread = new Thread(() =>
        {
            _threadId = PInvoke.GetCurrentThreadId();

            _hEvent = PInvoke.SetWinEventHook(
                PInvoke.EVENT_SYSTEM_FOREGROUND,
                PInvoke.EVENT_SYSTEM_MINIMIZEEND,
                null,
                (_, eventType, hWnd, _, _, _, _) =>
                {
                    switch (eventType)
                    {
                        case PInvoke.EVENT_SYSTEM_FOREGROUND:
                            uint pId;
                            var res = PInvoke.GetWindowThreadProcessId(hWnd, &pId);
                            if (res == 0)
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }

                            var hProc = PInvoke.OpenProcess_SafeHandle(
                                PROCESS_ACCESS_RIGHTS.PROCESS_QUERY_LIMITED_INFORMATION,
                                false,
                                pId
                            );
                            if (hProc is null || hProc.IsInvalid)
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }
                            fixed (char* pText = new char[1024])
                            {
                                var lenCopied = PInvoke.GetModuleFileNameEx(
                                    hProc,
                                    null,
                                    pText,
                                    1024
                                );
                                if (lenCopied == 0)
                                {
                                    throw new Win32Exception(Marshal.GetLastWin32Error());
                                }

                                ForegroundWindow = new string(pText);
                            }

                            break;
                    }
                },
                0,
                0,
                PInvoke.WINEVENT_OUTOFCONTEXT | PInvoke.WINEVENT_SKIPOWNPROCESS
            );

            if (_hEvent.IsInvalid)
            {
                throw new Win32Exception("Unable to set win event hook");
            }

            int bRet;
            while ((bRet = PInvoke.GetMessage(out var msg, HWND.Null, 0, 0)) != 0)
            {
                if (bRet == -1)
                {
                    return;
                }

                PInvoke.TranslateMessage(msg);

                PInvoke.DispatchMessage(msg);
            }
        });
        _thread.Start();
    }
}
