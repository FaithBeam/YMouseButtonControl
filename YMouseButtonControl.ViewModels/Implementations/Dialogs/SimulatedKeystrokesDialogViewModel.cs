using System.Reactive;
using System.Reactive.Linq;
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
    private int _simulatedKeystrokesIndex;
    private readonly ObservableAsPropertyHelper<ISimulatedKeystrokesType> _currentSimulatedKeystrokesType;
    
    public SimulatedKeystrokesDialogViewModel()
    {
        OkCommand = ReactiveCommand.Create(() => new SimulatedKeystrokesDialogModel
        {
            CustomKeys = CustomKeys,
            SimulatedKeystrokesType = CurrentSimulatedKeystrokesType
        });
        _currentSimulatedKeystrokesType = this
            .WhenAnyValue(x => x.SimulatedKeystrokesIndex)
            .DistinctUntilChanged()
            .Select(x => SimulatedKeystrokesTypes[x])
            .ToProperty(this, x => x.CurrentSimulatedKeystrokesType);
        _simulatedKeystrokesIndex = 0;
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

    public int SimulatedKeystrokesIndex
    {
        get => _simulatedKeystrokesIndex;
        set => this.RaiseAndSetIfChanged(ref _simulatedKeystrokesIndex, value);
    }

    private ISimulatedKeystrokesType CurrentSimulatedKeystrokesType => _currentSimulatedKeystrokesType.Value;

    public ReactiveCommand<Unit, SimulatedKeystrokesDialogModel> OkCommand { get; }
}