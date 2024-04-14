using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.Core.ViewModels.Interfaces.Dialogs;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.Services;

public class ShowSimulatedKeystrokesDialogService : IShowSimulatedKeystrokesDialogService
{
    public Interaction<
        SimulatedKeystrokesDialogViewModel,
        SimulatedKeystrokesDialogModel?
    > ShowSimulatedKeystrokesPickerInteraction { get; } = new();

    public async Task<SimulatedKeystrokes?> ShowSimulatedKeystrokesDialog()
    {
        return await ShowSimulatedKeystrokesDialog(null);
    }

    public async Task<SimulatedKeystrokes?> ShowSimulatedKeystrokesDialog(IButtonMapping? mapping)
    {
        var result = await ShowSimulatedKeystrokesPickerInteraction.Handle(
            new SimulatedKeystrokesDialogViewModel(mapping)
        );
        if (result is null)
        {
            return null;
        }

        return new SimulatedKeystrokes
        {
            Keys = result.CustomKeys,
            PriorityDescription = result.Description,
            SimulatedKeystrokesType = result.SimulatedKeystrokesType,
            BlockOriginalMouseInput = result.BlockOriginalMouseInput
        };
    }
}
