namespace YMouseButtonControl.Core.ViewModels.AppViewModel.Queries.StartupInstaller.CanBeInstalled;

public static class CanBeInstalledWindows
{
    public sealed class Handler : ICanBeInstalledHandler
    {
        public bool Execute() => true;
    }
}
