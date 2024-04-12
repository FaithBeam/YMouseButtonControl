using YMouseButtonControl.Core.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.Core.ViewModels.Models;

public class SimulatedKeystrokesDialogModel
{
    public string? CustomKeys { get; set; }
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }
    public string? Description { get; set; }
}
