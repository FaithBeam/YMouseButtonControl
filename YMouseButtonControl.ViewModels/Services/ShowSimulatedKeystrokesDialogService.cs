using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Models;

namespace YMouseButtonControl.ViewModels.Services;

public class ShowSimulatedKeystrokesDialogService : IShowSimulatedKeystrokesDialogService
{
    public ShowSimulatedKeystrokesDialogService()
    {
        ShowSimulatedKeystrokesPickerInteraction = new Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel>();
    }

    public Interaction<SimulatedKeystrokesDialogViewModel, SimulatedKeystrokesDialogModel> ShowSimulatedKeystrokesPickerInteraction { get; }
    public async Task<SimulatedKeystrokes> ShowSimulatedKeystrokesDialog()
    {
        return await ShowSimulatedKeystrokesDialog(null);
    }

    public async Task<SimulatedKeystrokes> ShowSimulatedKeystrokesDialog(IButtonMapping mapping)
    {
        var result = await ShowSimulatedKeystrokesPickerInteraction.Handle(new SimulatedKeystrokesDialogViewModel(mapping));
        if (result is null)
        {
            return null;
        }

        return new SimulatedKeystrokes()
        {
            Keys = result.CustomKeys,
            PriorityDescription = result.Description,
            SimulatedKeystrokesType = result.SimulatedKeystrokesType,
        };
    }
}