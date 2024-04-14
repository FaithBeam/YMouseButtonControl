using System.Threading.Tasks;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;

public interface IShowSimulatedKeystrokesDialogService
{
    Interaction<
        SimulatedKeystrokesDialogViewModel,
        SimulatedKeystrokesDialogModel?
    > ShowSimulatedKeystrokesPickerInteraction { get; }
    Task<SimulatedKeystrokes?> ShowSimulatedKeystrokesDialog(string buttonName);
    Task<SimulatedKeystrokes?> ShowSimulatedKeystrokesDialog(
        string buttonName,
        IButtonMapping mapping
    );
}
