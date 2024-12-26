using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.App.Commands.StartupInstaller.Install;

public static class InstallLinux
{
    public sealed class Handler : IInstallHandler
    {
        private const string DesktopFile = """
            [Desktop Entry]
            Type=Application
            Exec={0}
            Path={1}
            Hidden=false
            NoDisplay=false
            X-GNOME-Autostart-enabled=true
            Name=YMouseButtonControl
            Comment=YMouseButtonControl
            """;
        private readonly string _configDir = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData
        );
        private readonly string _autostartDir;
        private readonly string _desktopFilePath;

        public Handler()
        {
            _autostartDir = Path.Combine(_configDir, "autostart");
            _desktopFilePath = Path.Combine(_autostartDir, "YMouseButtonControl.desktop");
        }

        public void Execute() =>
            File.WriteAllText(
                _desktopFilePath,
                string.Format(DesktopFile, GetCurExePath(), GetCurExeParentPath())
            );

        private static string GetCurExeParentPath() =>
            Path.GetDirectoryName(GetCurExePath())
            ?? throw new Exception("Error retrieving parent of process path");

        private static string GetCurExePath() =>
            Environment.ProcessPath ?? throw new Exception("Error retrieving process path");
    }
}
