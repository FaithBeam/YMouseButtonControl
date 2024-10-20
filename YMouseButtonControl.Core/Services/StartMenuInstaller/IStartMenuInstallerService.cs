namespace YMouseButtonControl.Core.Services.StartMenuInstaller;

public interface IStartMenuInstallerService
{
    bool InstallStatus();
    void Install();
    void Uninstall();
}