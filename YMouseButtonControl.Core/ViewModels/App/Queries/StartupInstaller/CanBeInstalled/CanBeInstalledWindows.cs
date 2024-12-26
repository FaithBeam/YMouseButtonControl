namespace YMouseButtonControl.Core.ViewModels.App.Queries.StartupInstaller.CanBeInstalled;

public static class CanBeInstalledWindows
{
    public sealed class Handler : ICanBeInstalledHandler
    {
        public bool Execute() => true;
    }
}
