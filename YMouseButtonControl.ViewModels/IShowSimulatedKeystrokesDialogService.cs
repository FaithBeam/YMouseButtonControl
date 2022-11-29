using System.Threading.Tasks;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Models;

namespace YMouseButtonControl.ViewModels;

public interface IShowSimulatedKeystrokesDialogService
{
    Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel> ShowSimulatedKeystrokesPickerInteraction { get; }
    Task<SimulatedKeystrokes> ShowSimulatedKeystrokesDialog();
}