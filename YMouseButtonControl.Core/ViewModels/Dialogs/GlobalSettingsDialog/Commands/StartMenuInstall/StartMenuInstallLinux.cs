using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.GlobalSettingsDialog.Commands.StartMenuInstall;

public static class StartMenuInstallLinux
{
    public sealed class Handler : IStartMenuInstallHandler
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

        private readonly string _localShare = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData
        );

        private readonly string _desktopFilePath;

        public Handler()
        {
            var applicationsDir = Path.Combine(_localShare, "applications");
            _desktopFilePath = Path.Combine(applicationsDir, "YMouseButtonControl.desktop");
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
