using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using Newtonsoft.Json;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.ViewModels.Models;

[JsonObject(MemberSerialization.OptOut)]
public class ProfileVm : ReactiveObject, IEquatable<ProfileVm>
{
    private string? _description;
    private bool _checked;
    private string _name = string.Empty;
    private string _windowCaption = "N/A";
    private string _process = "N/A";
    private string _windowClass = "N/A";
    private string _parentClass = "N/A";
    private string _matchType = "N/A";
    private int _displayPriority;

    private readonly SourceCache<BaseButtonMappingVm, MouseButton> _btnSc;

    public ProfileVm(List<BaseButtonMappingVm> buttonMappings)
    {
        _btnSc = new SourceCache<BaseButtonMappingVm, MouseButton>(x => x.MouseButton);
        _btnSc.Edit(x => x.AddOrUpdate(buttonMappings));
    }

    [JsonIgnore]
    public int Id { get; set; }
    public bool IsDefault { get; set; }
    public bool Checked
    {
        get => _checked;
        set => this.RaiseAndSetIfChanged(ref _checked, value);
    }
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public required string Description
    {
        get
        {
            if (Name == "Default")
            {
                return "Default";
            }

            return string.IsNullOrWhiteSpace(_description) ? Process : _description;
        }
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public string WindowCaption
    {
        get => _windowCaption;
        set => this.RaiseAndSetIfChanged(ref _windowCaption, value);
    }

    public required string Process
    {
        get => _process;
        set => this.RaiseAndSetIfChanged(ref _process, value);
    }
    public required string WindowClass
    {
        get => _windowClass;
        set => this.RaiseAndSetIfChanged(ref _windowClass, value);
    }
    public required string ParentClass
    {
        get => _parentClass;
        set => this.RaiseAndSetIfChanged(ref _parentClass, value);
    }
    public required string MatchType
    {
        get => _matchType;
        set => this.RaiseAndSetIfChanged(ref _matchType, value);
    }

    public List<BaseButtonMappingVm> ButtonMappings => _btnSc.Items.ToList();

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton1
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mb1);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton2
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mb2);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton3
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mb3);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton4
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mb4);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton5
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mb5);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    [JsonIgnore]
    public BaseButtonMappingVm MouseWheelUp
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mwu);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    [JsonIgnore]
    public BaseButtonMappingVm MouseWheelDown
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mwd);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    [JsonIgnore]
    public BaseButtonMappingVm MouseWheelLeft
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mwl);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    [JsonIgnore]
    public BaseButtonMappingVm MouseWheelRight
    {
        get => _btnSc.Items.First(x => x.MouseButton == MouseButton.Mwr);
        set
        {
            _btnSc.Edit(updater => updater.AddOrUpdate(value));
            this.RaisePropertyChanged();
        }
    }

    public int DisplayPriority
    {
        get => _displayPriority;
        set => this.RaiseAndSetIfChanged(ref _displayPriority, value);
    }

    public bool Equals(ProfileVm? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Description == other.Description
            && Checked == other.Checked
            && Name == other.Name
            && WindowCaption == other.WindowCaption
            && Process == other.Process
            && WindowClass == other.WindowClass
            && ParentClass == other.ParentClass
            && MatchType == other.MatchType
            && IsDefault == other.IsDefault
            && ButtonMappings.SequenceEqual(other.ButtonMappings)
            && DisplayPriority == other.DisplayPriority;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ProfileVm)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(_description);
        hashCode.Add(_checked);
        hashCode.Add(_name);
        hashCode.Add(_windowCaption);
        hashCode.Add(_process);
        hashCode.Add(_windowClass);
        hashCode.Add(_parentClass);
        hashCode.Add(_matchType);
        hashCode.Add(_btnSc);
        hashCode.Add(Id);
        hashCode.Add(IsDefault);
        hashCode.Add(ButtonMappings);
        hashCode.Add(DisplayPriority);
        return hashCode.ToHashCode();
    }
}
