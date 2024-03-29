using System.Collections.Generic;
using System.Linq;
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
    private static readonly Dictionary<string, string> CombinedKeyDict = ModifierKeys
        .Concat(StandardKeys)
        .Concat(DirectionKeys)
        .Concat(FunctionKeys)
        .Concat(NumericKeypadKeys)
        .Concat(MediaKeys)
        .Concat(BrowserKeys)
        .ToDictionary(x => x.Key, x => x.Value);

    private string _customKeys;
    private string _currentKey;
    private string _description;
    private int _simulatedKeystrokesIndex;
    private int _caretIndex;
    private readonly ObservableAsPropertyHelper<ISimulatedKeystrokesType> _currentSimulatedKeystrokesType;

    public SimulatedKeystrokesDialogViewModel()
        : this(null) { }

    public SimulatedKeystrokesDialogViewModel(IButtonMapping currentMapping)
    {
        OkCommand = ReactiveCommand.Create(
            () =>
                new SimulatedKeystrokesDialogModel
                {
                    CustomKeys = CustomKeys,
                    SimulatedKeystrokesType = CurrentSimulatedKeystrokesType,
                    Description = Description
                }
        );

        SplitButtonCommand = ReactiveCommand.Create<string>(insertString =>
        {
            if (insertString != "{}")
            {
                insertString = CombinedKeyDict[insertString];
            }

            CustomKeys = CustomKeys.Insert(CaretIndex, insertString);
            CaretIndex += insertString.Length;
        });

        _currentSimulatedKeystrokesType = this.WhenAnyValue(x => x.SimulatedKeystrokesIndex)
            .DistinctUntilChanged()
            .Select(x => SimulatedKeystrokesTypes[x])
            .ToProperty(this, x => x.CurrentSimulatedKeystrokesType);

        _description = currentMapping?.PriorityDescription ?? string.Empty;
        _customKeys = currentMapping?.Keys ?? string.Empty;
        SimulatedKeystrokesIndex = currentMapping?.SimulatedKeystrokesType?.Index ?? 0;
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
        new(SimulatedKeystrokeTypesMappingFactory.GetSimulatedKeystrokesTypes());

    public int SimulatedKeystrokesIndex
    {
        get => _simulatedKeystrokesIndex;
        set => this.RaiseAndSetIfChanged(ref _simulatedKeystrokesIndex, value);
    }

    private ISimulatedKeystrokesType CurrentSimulatedKeystrokesType =>
        _currentSimulatedKeystrokesType.Value;

    public ReactiveCommand<Unit, SimulatedKeystrokesDialogModel> OkCommand { get; }

    public ReactiveCommand<string, Unit> SplitButtonCommand { get; }

    public static Dictionary<string, string> ModifierKeys =>
        new()
        {
            { "Control", "{CTRL}" },
            { "Right Control", "{RCTRL}" },
            { "Alt", "{ALT}" },
            { "Right Alt (Alt Gr)", "{RALT}" },
            { "Shift", "{SHIFT}" },
            { "Right Shift", "{RSHIFT}" },
            { "Windows Key", "{LWIN}" },
            { "Right Windows Key", "{RWIN}" },
            { "Apps (Context Menu key)", "{APPS}" },
        };

    public static Dictionary<string, string> StandardKeys =>
        new()
        {
            { "Escape", "{ESC}" },
            { "Space", "{SPACE}" },
            { "Return", "{RETURN}" },
            { "Tab", "{TAB}" },
            { "Backspace", "{BACKSPACE}" },
            { "Delete", "{DEL}" },
            { "Insert", "{INS}" },
            { "Home", "{HOME}" },
            { "End", "{END}" },
            { "Page Up", "{PGUP}" },
            { "Page Down", "{PGDN}" },
            { "PrtScn", "{PRTSCN}" },
            { "Pause", "{PAUSE}" },
            { "CAPS Lock Toggle", "{CAPS}" },
            { "Scroll Lock Toggle", "{SCROLLLOCK}" },
        };

    public static Dictionary<string, string> DirectionKeys =>
        new()
        {
            { "Up", "{UP}" },
            { "Down", "{DOWN}" },
            { "Left", "{LEFT}" },
            { "Right", "{RIGHT}" },
        };

    public static Dictionary<string, string> FunctionKeys =>
        new()
        {
            { "F1", "{F1}" },
            { "F2", "{F2}" },
            { "F3", "{F3}" },
            { "F4", "{F4}" },
            { "F5", "{F5}" },
            { "F6", "{F6}" },
            { "F7", "{F7}" },
            { "F8", "{F8}" },
            { "F9", "{F9}" },
            { "F10", "{F10}" },
            { "F11", "{F11}" },
            { "F12", "{F12}" },
            { "F13", "{F13}" },
            { "F14", "{F14}" },
            { "F15", "{F15}" },
            { "F16", "{F16}" },
            { "F17", "{F17}" },
            { "F18", "{F18}" },
            { "F19", "{F19}" },
            { "F20", "{F20}" },
            { "F21", "{F21}" },
            { "F22", "{F22}" },
            { "F23", "{F23}" },
            { "F24", "{F24}" }
        };

    public static Dictionary<string, string> NumericKeypadKeys =>
        new()
        {
            { "Num Lock Toggle", "{NUMLOCK}" },
            { "Divide", "{NUM/}" },
            { "Multiply", "{NUM*}" },
            { "Subtract", "{NUM-}" },
            { "Add", "{NUM+}" },
            { "Decimal", "{NUM.}" },
            { "Equals", "{NUM=}" },
            { "Enter", "{NUMENTER}" },
            { "Num 0", "{NUM0}" },
            { "Num 1", "{NUM1}" },
            { "Num 2", "{NUM2}" },
            { "Num 3", "{NUM3}" },
            { "Num 4", "{NUM4}" },
            { "Num 5", "{NUM5}" },
            { "Num 6", "{NUM6}" },
            { "Num 7", "{NUM7}" },
            { "Num 8", "{NUM8}" },
            { "Num 9", "{NUM9}" },
            { "Num End", "{NUMEND}" },
            { "Num Up", "{NUMUP}" },
            { "Num Down", "{NUMDOWN}" },
            { "Num Left", "{NUMLEFT}" },
            { "Num Right", "{NUMRIGHT}" },
            { "Num Page Up", "{NUMPGUP}" },
            { "Num Page Down", "{NUMPGDN}" },
            { "Num Clear", "{NUMCLEAR}" },
            { "Num Home", "{NUMHOME}" },
            { "Num Insert", "{NUMINS}" },
            { "Num Delete", "{NUMDEL}" },
        };

    public static Dictionary<string, string> MediaKeys =>
        new()
        {
            { "Volume Up", "{VOL+}" },
            { "Volume Down", "{VOL-}" },
            { "Mute", "{MUTE}" },
            { "Play/Pause", "{MEDIAPLAY}" },
            { "Stop", "{MEDIASTOP}" },
            { "Next Track", "{MEDIANEXT}" },
            { "Previous Track", "{MEDIAPREV}" },
            { "Select Track", "{MEDIASELECT}" },
            { "Eject Media", "{MEDIAEJECT}" }
        };

    public static Dictionary<string, string> BrowserKeys =>
        new()
        {
            { "Web Home", "{WEBHOME}" },
            { "Web Back", "{BACK}" },
            { "Web Forward", "{FORWARD}" },
            { "Web Favorites", "{FAVORITES}" },
            { "Web Refresh", "{REFRESH}" },
            { "Web Search", "{SEARCH}" },
            { "Web Stop", "{STOP}" },
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
