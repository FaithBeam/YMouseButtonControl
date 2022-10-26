using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.ViewModels.Models;

public class SimulatedKeystrokesDialogModel
{
    public string CustomKeys { get; set; }
    public ISimulatedKeystrokesType SimulatedKeystrokesType { get; set; }
    public string Description { get; set; }
}