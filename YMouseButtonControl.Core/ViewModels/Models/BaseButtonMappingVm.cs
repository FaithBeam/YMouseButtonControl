﻿using System;
using Newtonsoft.Json;
using ReactiveUI;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.Models;

[JsonObject(MemberSerialization.OptOut)]
public abstract class BaseButtonMappingVm : ReactiveObject, IEquatable<BaseButtonMappingVm>
{
    private string? _keys;
    private int _index;
    private bool _enabled;
    private string? _description;
    private string? _priorityDescription;
    private int? _autoAutoRepeatDelay;
    private bool? _autoRepeatRandomizeDelayEnabled;
    private bool _state;
    private bool? _blockOriginalMouseInput;
    private BaseSimulatedKeystrokeTypeVm? _simulatedKeystrokeTypeVm;
    private bool _selected;
    private bool _hasSettingsPopped;

    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public int ProfileId { get; set; }

    public MouseButton MouseButton { get; set; }

    public int Index
    {
        get => _index;
        set => this.RaiseAndSetIfChanged(ref _index, value);
    }

    /// <summary>
    /// Used to return to the previously selected button mapping when a user changes profile
    /// </summary>
    public bool Selected
    {
        get => _selected;
        set => this.RaiseAndSetIfChanged(ref _selected, value);
    }

    /// <summary>
    /// If the simulated keystrokes settings window has popped for this button mapping
    /// </summary>
    public bool HasSettingsPopped
    {
        get => _hasSettingsPopped;
        set => this.RaiseAndSetIfChanged(ref _hasSettingsPopped, value);
    }

    public bool Enabled
    {
        get => _enabled;
        set => this.RaiseAndSetIfChanged(ref _enabled, value);
    }
    public string? Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    public string? PriorityDescription
    {
        get => _priorityDescription;
        set => this.RaiseAndSetIfChanged(ref _priorityDescription, value);
    }

    public int? AutoRepeatDelay
    {
        get => _autoAutoRepeatDelay;
        set => this.RaiseAndSetIfChanged(ref _autoAutoRepeatDelay, value);
    }

    public bool? AutoRepeatRandomizeDelayEnabled
    {
        get => _autoRepeatRandomizeDelayEnabled;
        set => this.RaiseAndSetIfChanged(ref _autoRepeatRandomizeDelayEnabled, value);
    }

    public string? Keys
    {
        get => _keys;
        set => this.RaiseAndSetIfChanged(ref _keys, value);
    }
    public bool State
    {
        get => _state;
        set => this.RaiseAndSetIfChanged(ref _state, value);
    }
    public bool CanRaiseDialog { get; set; }
    public BaseSimulatedKeystrokeTypeVm? SimulatedKeystrokeType
    {
        get => _simulatedKeystrokeTypeVm;
        set => this.RaiseAndSetIfChanged(ref _simulatedKeystrokeTypeVm, value);
    }
    public bool? BlockOriginalMouseInput
    {
        get => _blockOriginalMouseInput;
        set => this.RaiseAndSetIfChanged(ref _blockOriginalMouseInput, value);
    }

    protected BaseButtonMappingVm CreateClone(BaseButtonMappingVm clone)
    {
        clone.Id = Id;
        clone.ProfileId = ProfileId;
        clone.MouseButton = MouseButton;
        clone.Index = Index;
        clone.Selected = Selected;
        clone.HasSettingsPopped = HasSettingsPopped;
        clone.Enabled = Enabled;
        clone.Description = Description;
        clone.PriorityDescription = PriorityDescription;
        clone.AutoRepeatDelay = AutoRepeatDelay;
        clone.AutoRepeatRandomizeDelayEnabled = AutoRepeatRandomizeDelayEnabled;
        clone.Keys = Keys;
        clone.State = State;
        clone.CanRaiseDialog = CanRaiseDialog;
        clone.SimulatedKeystrokeType = SimulatedKeystrokeType?.Clone();
        clone.BlockOriginalMouseInput = BlockOriginalMouseInput;

        return clone;
    }

    public abstract BaseButtonMappingVm Clone();

    public override string? ToString()
    {
        return Description;
    }

    public bool Equals(BaseButtonMappingVm? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id == other.Id
            && Index == other.Index
            && Enabled == other.Enabled
            && Description == other.Description
            && PriorityDescription == other.PriorityDescription
            && Keys == other.Keys
            && State == other.State
            && CanRaiseDialog == other.CanRaiseDialog
            && AutoRepeatDelay == other.AutoRepeatDelay
            && AutoRepeatRandomizeDelayEnabled == other.AutoRepeatRandomizeDelayEnabled
            && Equals(SimulatedKeystrokeType, other.SimulatedKeystrokeType)
            && BlockOriginalMouseInput == other.BlockOriginalMouseInput
            && Selected == other.Selected;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((BaseButtonMappingVm)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Index);
        hashCode.Add(Enabled);
        hashCode.Add(Description);
        hashCode.Add(PriorityDescription);
        hashCode.Add(AutoRepeatDelay);
        hashCode.Add(AutoRepeatRandomizeDelayEnabled);
        hashCode.Add(Keys);
        hashCode.Add(State);
        hashCode.Add(SimulatedKeystrokeType);
        hashCode.Add(BlockOriginalMouseInput);
        return hashCode.ToHashCode();
    }
}

public class DisabledMappingVm : BaseButtonMappingVm
{
    public DisabledMappingVm()
    {
        Index = 1;
        Description = "Disabled";
    }

    public override BaseButtonMappingVm Clone() => CreateClone(new DisabledMappingVm());
}

public class NothingMappingVm : BaseButtonMappingVm
{
    public NothingMappingVm()
    {
        Index = 0;
        Description = "** No Change (Don't Intercept) **";
    }

    public override BaseButtonMappingVm Clone() => CreateClone(new NothingMappingVm());
}

public class SimulatedKeystrokeVm : BaseButtonMappingVm
{
    public SimulatedKeystrokeVm()
    {
        Index = 2;
        Description = "Simulated Keys (undefined)";
        CanRaiseDialog = true;
        BlockOriginalMouseInput = true;
    }

    public override BaseButtonMappingVm Clone() => CreateClone(new SimulatedKeystrokeVm());

    public override string? ToString()
    {
        var myStr = SimulatedKeystrokeType is not null
            ? $"Simulated Keys: ({SimulatedKeystrokeType.ShortDescription})"
            : Description;

        if (!string.IsNullOrWhiteSpace(PriorityDescription))
        {
            myStr += $"[{PriorityDescription}]";
        }

        if (!string.IsNullOrWhiteSpace(Keys))
        {
            myStr += $"[{Keys}]";
        }

        return myStr;
    }
}

public class RightClickVm : BaseButtonMappingVm
{
    public RightClickVm()
    {
        Index = 3;
        Description = "Right Click";
    }

    public override BaseButtonMappingVm Clone() => CreateClone(new RightClickVm());
}
