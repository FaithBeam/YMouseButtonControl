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
    Task<SimulatedKeystrokes?> ShowSimulatedKeystrokesDialog();
    Task<SimulatedKeystrokes?> ShowSimulatedKeystrokesDialog(IButtonMapping mapping);
}
