using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.IsInstalled;

public static class IsInstalledOsx
{
    public sealed class Handler : IIsInstalledHandler
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

        public bool Execute() =>
            File.Exists(_plistPath)
            && File.ReadAllText(_plistPath).Contains($"<string>{GetCurExePath()}</string>");

        private static string GetCurExePath() =>
            Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
    }
}
