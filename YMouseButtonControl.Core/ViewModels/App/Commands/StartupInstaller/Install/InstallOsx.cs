using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.App.Commands.StartupInstaller.Install;

public static class InstallOsx
{
    public sealed class Handler : IInstallHandler
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

        public Handler()
        {
            _usrLaunchAgentsDir = Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Library",
                "LaunchAgents"
            );
            _plistPath = Path.Join(_usrLaunchAgentsDir, "com.github.ymousebuttoncontrol.plist");
        }

        public void Execute() =>
            File.WriteAllText(_plistPath, string.Format(PlistData, GetCurExePath()));

        private static string GetCurExePath() =>
            Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
    }
}
