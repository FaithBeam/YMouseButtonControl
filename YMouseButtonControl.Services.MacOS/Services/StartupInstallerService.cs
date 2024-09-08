using System;
using System.IO;
using System.Reflection;
using YMouseButtonControl.Core.Services;

namespace YMouseButtonControl.Services.MacOS.Services;

public class StartupInstallerService : IStartupInstallerService
{
    private const string PlistData = """
        <?xml version="1.0" encoding="UTF-8"?>
        <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
        <plist version="1.0">
        <dict>
            <key>Label</key>
            <string>com.github.ymousebuttoncontrol</string>
            
            <key>ProgramArguments</key>
            <array>
                <string>{0}</string>
            </array>
            
            <key>RunAtLoad</key>
            <true/>
            
            <key>KeepAlive</key>
            <false/>
        </dict>
        </plist>
        """;

    private readonly string _usrLaunchAgentsDir;
    private readonly string _plistPath;

    public StartupInstallerService()
    {
        _usrLaunchAgentsDir = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Library",
            "LaunchAgents"
        );
        _plistPath = Path.Join(_usrLaunchAgentsDir, "com.github.ymousebuttoncontrol.plist");
    }

    public bool ButtonEnabled() => Directory.Exists(_usrLaunchAgentsDir);

    public bool InstallStatus() =>
        File.Exists(_plistPath)
        && File.ReadAllText(_plistPath).Contains($"<string>{GetCurExePath()}</string>");

    public void Install() =>
        File.WriteAllText(_plistPath, string.Format(PlistData, GetCurExePath()));

    public void Uninstall() => File.Delete(_plistPath);

    private static string GetCurExePath() =>
        Path.Join(
            Path.GetDirectoryName(AppContext.BaseDirectory)
                ?? throw new Exception("Error retrieving path of executing assembly"),
            "YMouseButtonControl"
        );
}
