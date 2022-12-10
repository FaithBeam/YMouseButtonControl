using System;
using System.Collections.Generic;
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
    private string _currentKey;
    private string _description;
    private int _simulatedKeystrokesIndex;
    private int _caretIndex;
    private readonly ObservableAsPropertyHelper<ISimulatedKeystrokesType> _currentSimulatedKeystrokesType;
    
    public SimulatedKeystrokesDialogViewModel() : this(null) {}

    public SimulatedKeystrokesDialogViewModel(IButtonMapping currentMapping)
    {
        OkCommand = ReactiveCommand.Create(() => new SimulatedKeystrokesDialogModel
        {
            CustomKeys = CustomKeys,
            SimulatedKeystrokesType = CurrentSimulatedKeystrokesType,
            Description = Description
        });
        SplitButtonCommand = ReactiveCommand.Create<string>(insertString =>
        {
            if (insertString != "{}")
            {
                insertString = ModifierKeys[insertString];
            }
            CustomKeys = CustomKeys.Insert(CaretIndex, insertString);
            CaretIndex += insertString.Length;
        });
        _currentSimulatedKeystrokesType = this
            .WhenAnyValue(x => x.SimulatedKeystrokesIndex)
            .DistinctUntilChanged()
            .Select(x => SimulatedKeystrokesTypes[x])
            .ToProperty(this, x => x.CurrentSimulatedKeystrokesType);
        _description = currentMapping?.PriorityDescription ?? string.Empty;
        _customKeys = currentMapping?.Keys ?? string.Empty;
        _simulatedKeystrokesIndex = currentMapping?.SimulatedKeystrokesType?.Index ?? 0;
        _caretIndex = 0;
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
    
    public ReactiveCommand<string, Unit> SplitButtonCommand { get; }

    public Dictionary<string, string> ModifierKeys => new()
    {
        {"Control", "{CTRL}"},
        {"Right Control", "{RCTRL}"},
        {"Alt", "{ALT}"},
        {"Right Alt (Alt Gr)", "{RALT}"},
        {"Shift", "{SHIFT}"},
        {"Right Shift", "{RSHIFT}"},
        {"Windows Key", "{LWIN}"},
        {"Right Windows Key", "{RWIN}"},
        {"Apps (Context Menu key)", "{APPS}"},
    };

    public string CurrentKey
    {
        get => _currentKey;
        set => this.RaiseAndSetIfChanged(ref _currentKey, value);
    }

    public int CaretIndex
    {
        get => _caretIndex;
        set => this.RaiseAndSetIfChanged(ref _caretIndex, value);
    }
}