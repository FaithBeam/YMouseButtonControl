using System.Collections.Generic;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.ViewModels.Models;

public class SimulatedKeystrokesDialogModel
{
    public IEnumerable<IParsedEvent> Events { get; set; }
    public ISimulatedKeystrokesType SimulatedKeystrokesType { get; set; }
    public string Description { get; set; }
}