using System.Collections.ObjectModel;
using NSubstitute;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.ViewModels.Implementations;

namespace YMouseButtonControl.ViewModels.Tests.Implementations;

public class LayerViewModelTests
{
    private static object[] _mbIndexData =
    {
        new object[] { "Mb1Index", "MouseButton1Combo" },
        new object[] { "Mb2Index", "MouseButton2Combo" },
        new object[] { "Mb3Index", "MouseButton3Combo" },
        new object[] { "Mb4Index", "MouseButton4Combo" },
        new object[] { "Mb5Index", "MouseButton5Combo" },
        new object[] { "MwuIndex", "MouseWheelUpCombo" },
        new object[] { "MwdIndex", "MouseWheelDownCombo" },
        new object[] { "MwlIndex", "MouseWheelLeftCombo" },
        new object[] { "MwrIndex", "MouseWheelRightCombo" },
    };

    [Test]
    [TestCaseSource(nameof(_mbIndexData))]
    public void TestChangeMouseButtonIndex_ToDisabled_DoesNotShowSimulatedKeystrokesDialog(string mbIdxName, string mbComboName)
    {
        var (lvm, sskdsM, _, _) = GetLayerViewModel();
        var lvmType = lvm.GetType();
        var p = lvmType.GetProperty(mbIdxName);

        p.SetValue(lvm, 1);
        var mbIdxVal = (int)lvmType.GetProperty(mbIdxName).GetValue(lvm, null);
        var mbComboVal = (ObservableCollection<IButtonMapping>)lvmType.GetProperty(mbComboName).GetValue(lvm, null);

        Assert.Multiple(() =>
        {
            Assert.That(mbIdxVal, Is.EqualTo(1));
            Assert.That(mbComboVal[1], Is.InstanceOf<DisabledMapping>());
            sskdsM.DidNotReceiveWithAnyArgs().ShowSimulatedKeystrokesDialog();
        });
    }

    [Test]
    [TestCaseSource(nameof(_mbIndexData))]
    public void TestChangeMouseButtonIndex_ToSimulatedKeystrokes_RaisesShowSimulatedKeystrokesDialog(string mbIdxName, string mbComboName)
    {
        var (lvm, sskdsM, _, _) = GetLayerViewModel();
        var lvmType = lvm.GetType();
        var p = lvmType.GetProperty(mbIdxName);

        p.SetValue(lvm, 2);
        var mbIdxVal = (int)lvmType.GetProperty(mbIdxName).GetValue(lvm, null);
        var mbComboVal = (ObservableCollection<IButtonMapping>)lvmType.GetProperty(mbComboName).GetValue(lvm, null);

        Assert.Multiple(() =>
        {
            Assert.That(mbIdxVal, Is.EqualTo(2));
            Assert.That(mbComboVal[2], Is.InstanceOf<SimulatedKeystrokes>());
            sskdsM.ReceivedWithAnyArgs(1).ShowSimulatedKeystrokesDialog(default);
        });
    }

    private (LayerViewModel sut, IShowSimulatedKeystrokesDialogService sskdsM, IMouseListener mlM, IProfilesService psM)
        GetLayerViewModel(Profile? profile = null)
    {
        var psM = Substitute.For<IProfilesService>();
        psM.CurrentProfile.Returns(profile ?? new Profile());
        var mlM = Substitute.For<IMouseListener>();
        var sskdsM = Substitute.For<IShowSimulatedKeystrokesDialogService>();
        var sut = new LayerViewModel(psM, mlM, sskdsM);
        return (sut, sskdsM, mlM, psM);
    }


    //
    // [Test]
    // public void TestChangeMouseButtonIndex()
    // {
    //     _lvm.Mb1Index = 2;
    //     Assert.IsTrue(_lvm.Mb1Index == 2);
    //     Assert.IsTrue(_lvm.MouseButton1Combo[_lvm.Mb1Index] is SimulatedKeystrokes);
    //     dialog.Verify(x => x.ShowSimulatedKeystrokesDialog(It.IsAny<ISequencedMapping>()), Times.Once);
    // }
}