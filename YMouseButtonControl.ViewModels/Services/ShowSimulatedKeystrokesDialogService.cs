using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
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
        var result = await ShowSimulatedKeystrokesPickerInteraction.Handle(new SimulatedKeystrokesDialogViewModel());
        if (result is null)
        {
            return null;
        }

        return new SimulatedKeystrokes()
        {
            Keys = result.CustomKeys,
            SimulatedKeystrokesType = result.SimulatedKeystrokesType,
        };
    }
}