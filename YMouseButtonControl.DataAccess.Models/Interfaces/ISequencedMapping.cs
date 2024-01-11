using YMouseButtonControl.DataAccess.Models.Models;

namespace YMouseButtonControl.DataAccess.Models.Interfaces;

public interface ISequencedMapping : IButtonMapping
{
    public IEnumerable<IParsedEvent> Sequence { get; set; }
    public bool State { get; set; }
    public ISimulatedKeystrokesType? SimulatedKeystrokesType { get; set; }
}