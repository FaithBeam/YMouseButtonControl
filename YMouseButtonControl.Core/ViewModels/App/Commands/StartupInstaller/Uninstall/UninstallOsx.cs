using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.App.Commands.StartupInstaller.Uninstall;

public static class UninstallOsx
{
    public sealed class Handler : IUninstallHandler
    {
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

        public void Execute() => File.Delete(_plistPath);
    }
}
