using Moq.AutoMock;

namespace YMouseButtonControl.ViewModels.Tests.Implementations;

[TestClass]
public class LayerViewModelTests
{
    private AutoMocker _autoMocker;

    public LayerViewModelTests()
    {
        _autoMocker = new AutoMocker();
    }

    [TestMethod]
    public void TestGetMappingAsync()
    {
        _autoMocker.Use<>();
    }
}