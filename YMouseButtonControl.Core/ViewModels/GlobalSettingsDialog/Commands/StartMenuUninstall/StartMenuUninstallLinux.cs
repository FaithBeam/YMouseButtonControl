using System;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.StartMenuUninstall;

public class StartMenuUninstallLinux : IStartMenuUninstall
{
    private readonly string _localShare = Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData
    );

    private readonly string _desktopFilePath;

    public StartMenuUninstallLinux()
    {
        var applicationsDir = Path.Combine(_localShare, "applications");
        _desktopFilePath = Path.Combine(applicationsDir, "YMouseButtonControl.desktop");
    }

    public void Uninstall() => File.Delete(_desktopFilePath);
}
