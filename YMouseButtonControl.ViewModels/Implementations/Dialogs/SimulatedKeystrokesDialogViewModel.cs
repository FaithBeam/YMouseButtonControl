using System.Reactive;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Factories;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.ViewModels.Models;

namespace YMouseButtonControl.ViewModels.Implementations.Dialogs;

public class SimulatedKeystrokesDialogViewModel : DialogBase
{
    private string _customKeys;
    private string _description;
    
    public SimulatedKeystrokesDialogViewModel()
    {
        OkCommand = ReactiveCommand.Create(() => new SimulatedKeystrokesDialogModel
        {
            CustomKeys = CustomKeys,
            SimulatedKeystrokesType = CurrentSimulatedKeystrokesType
        });
    }

    public string CustomKeys
    {
        get => _customKeys;
        set => this.RaiseAndSetIfChanged(ref _customKeys, value);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public AvaloniaList<ISimulatedKeystrokesType> SimulatedKeystrokesTypes { get; set; } =
        new(SimulatedKeystrokesMappingFactory.GetSimulatedKeystrokesTypes());

    public ISimulatedKeystrokesType CurrentSimulatedKeystrokesType { get; set; }

    public ReactiveCommand<Unit, SimulatedKeystrokesDialogModel> OkCommand { get; }
}