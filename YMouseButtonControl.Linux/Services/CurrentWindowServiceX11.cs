using System.Diagnostics;
using YMouseButtonControl.Core.Services.Processes;

namespace YMouseButtonControl.Linux.Services;

public class CurrentWindowServiceX11 : ICurrentWindowService
{
    public string ForegroundWindow => GetForegroundWindow();

    private string GetForegroundWindow()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = "-c \"xdotool getwindowfocus getwindowpid\"",
            RedirectStandardOutput = true,
        };
        using var xdoProc = new Process();
        xdoProc.StartInfo = startInfo;
        xdoProc.Start();
        var pid = xdoProc.StandardOutput.ReadToEnd().TrimEnd();
        xdoProc.WaitForExit();

        if (string.IsNullOrWhiteSpace(pid))
        {
            return "";
        }

        startInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"ls -l /proc/{pid}/exe\"",
            RedirectStandardOutput = true,
        };
        using var proc = new Process();
        proc.StartInfo = startInfo;
        proc.Start();
        var path = proc.StandardOutput.ReadToEnd().Split("-> ")[1].Trim();
        proc.WaitForExit();
        return path;
    }
}
