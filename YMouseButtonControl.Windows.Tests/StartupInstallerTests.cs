using System.Runtime.Versioning;
using YMouseButtonControl.Services.Windows.Services;

namespace YMouseButtonControl.Windows.Tests;

[SupportedOSPlatform("windows")]
[Explicit]
public class StartupInstallerTests
{
    [Test]
    public void I_can_get_startup_install_status()
    {
        var sut = new StartupInstallerService();

        Assert.DoesNotThrow(() =>
        {
            sut.InstallStatus();
        });
    }

    [Test]
    public void I_can_install()
    {
        var sut = new StartupInstallerService();

        Assert.DoesNotThrow(() =>
        {
            sut.Install();

            Assert.That(sut.InstallStatus(), Is.True);
        });
    }

    [Test]
    public void I_can_uninstall()
    {
        var sut = new StartupInstallerService();

        Assert.DoesNotThrow(() =>
        {
            sut.Uninstall();

            Assert.That(sut.InstallStatus(), Is.False);
        });
    }
}
