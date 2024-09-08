namespace YMouseButtonControl.Core.Services;

public interface IStartupInstallerService
{
    public bool InstallStatus();
    public void Install();
    public void Uninstall();
}
