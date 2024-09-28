namespace YMouseButtonControl.Core.Services.StartupInstaller;

public interface IStartupInstallerService
{
    public bool ButtonEnabled();
    public bool InstallStatus();
    public void Install();
    public void Uninstall();
}
