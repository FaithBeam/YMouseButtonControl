﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.Implementations.Dialogs;

public class SimulatedKeystrokesDialogViewModel : DialogBase
{
    private static readonly Dictionary<string, string> CombinedKeyDict = ModifierKeys
        .Concat(StandardKeys)
        .Concat(DirectionKeys)
        .Concat(FunctionKeys)
        .Concat(NumericKeypadKeys)
        .Concat(MediaKeys)
        .Concat(BrowserKeys)
        .Concat(MouseButtons)
        .ToDictionary(x => x.Key, x => x.Value);

    private string _customKeys;
    private string? _currentKey;
    private string _description;
    private readonly string _title;
    private int _caretIndex;
    private bool _blockOriginalMouseInput = true;
    private short _x;
    private short _y;
    private ISimulatedKeystrokesType? _currentSimulatedKeystrokesType;
    private readonly IMouseListener _mouseListener;

    public SimulatedKeystrokesDialogViewModel(
        IMouseListener mouseListener,
        string buttonName,
        IButtonMapping? currentMapping = null
    )
    {
        _title = $"SimulatedKeystrokes - {buttonName}";
        _mouseListener = mouseListener;
        _mouseListener.OnMouseMovedEventHandler += MouseListenerOnOnMouseMovedEventHandler;
        _description = currentMapping?.PriorityDescription ?? string.Empty;
        _customKeys = currentMapping?.Keys ?? string.Empty;
        SimulatedKeystrokesType =
            currentMapping?.SimulatedKeystrokesType ?? new MouseButtonPressedActionType();
        _caretIndex = 0;

        var canExecuteOkCmd = this.WhenAnyValue(
            property1: x => x.CustomKeys,
            property2: x => x.SimulatedKeystrokesType,
            selector: (keys, skt) =>
                !string.IsNullOrWhiteSpace(keys)
                && skt is not null
                && (
                    keys != currentMapping?.Keys
                    || !Equals(skt, currentMapping.SimulatedKeystrokesType)
                )
        );
        OkCommand = ReactiveCommand.Create(
            () =>
                new SimulatedKeystrokesDialogModel
                {
                    CustomKeys = CustomKeys,
                    SimulatedKeystrokesType = SimulatedKeystrokesType,
                    Description = Description,
                    BlockOriginalMouseInput = BlockOriginalMouseInput
                },
            canExecuteOkCmd
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
    }

    private void MouseListenerOnOnMouseMovedEventHandler(
        object? sender,
        NewMouseHookMoveEventArgs e
    )
    {
        X = e.X;
        Y = e.Y;
        ComputedXy = $"{X},{Y}";
    }

    public ISimulatedKeystrokesType? SimulatedKeystrokesType
    {
        get => _currentSimulatedKeystrokesType;
        set => this.RaiseAndSetIfChanged(ref _currentSimulatedKeystrokesType, value);
    }

    private short X
    {
        get => _x;
        set => this.RaiseAndSetIfChanged(ref _x, value);
    }

    private short Y
    {
        get => _y;
        set => this.RaiseAndSetIfChanged(ref _y, value);
    }

    public string? ComputedXy
    {
        get => _computedXy;
        set => this.RaiseAndSetIfChanged(ref _computedXy, value);
    }

    public string Title => _title;

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

    public bool BlockOriginalMouseInput
    {
        get => _blockOriginalMouseInput;
        set => this.RaiseAndSetIfChanged(ref _blockOriginalMouseInput, value);
    }

    public AvaloniaList<ISimulatedKeystrokesType> SimulatedKeystrokesTypes { get; set; } =
        new(GetSimulatedKeystrokesTypes());

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

    public static Dictionary<string, string> MouseButtons =>
        new()
        {
            { "Left Button", "{LMB}" },
            { "Right Button", "{RMB}" },
            { "Middle Button", "{MMB}" },
            { "4th Button", "{MB4}" },
            { "5th Button", "{MB5}" },
            { "Wheel Up", "{MWUP}" },
            { "Wheel Down", "{MWDN}" },
            { "Tilt Left", "{TILTL}" },
            { "Tilt Right", "{TILTR}" },
        };

    public string? CurrentKey
    {
        get => _currentKey;
        set => this.RaiseAndSetIfChanged(ref _currentKey, value);
    }

    public int CaretIndex
    {
        get => _caretIndex;
        set => this.RaiseAndSetIfChanged(ref _caretIndex, value);
    }

    private static readonly List<Func<ISimulatedKeystrokesType>> SimulatedKeystrokeTypesList =
    [
        () => new MouseButtonPressedActionType(),
        () => new MouseButtonReleasedActionType(),
        () => new DuringMouseActionType(),
        () => new InAnotherThreadPressedActionType(),
        () => new InAnotherThreadReleasedActionType(),
        () => new RepeatedlyWhileButtonDownActionType(),
        () => new StickyRepeatActionType(),
        () => new StickyHoldActionType(),
        () => new AsMousePressedAndReleasedActionType()
    ];

    private string? _computedXy;

    private static IEnumerable<ISimulatedKeystrokesType> GetSimulatedKeystrokesTypes() =>
        SimulatedKeystrokeTypesList.Select(x => x());
}
