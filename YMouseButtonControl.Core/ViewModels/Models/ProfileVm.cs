using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using Newtonsoft.Json;
using ReactiveUI;
using YMouseButtonControl.Domain.Models;

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

    private readonly SourceCache<BaseButtonMappingVm, int> _btnSc;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _btnsMappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mb1Mappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mb2Mappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mb3Mappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mb4Mappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mb5Mappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mwuMappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mwdMappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mwlMappings;
    private readonly ReadOnlyObservableCollection<BaseButtonMappingVm> _mwrMappings;

    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mb1;
    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mb2;
    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mb3;
    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mb4;
    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mb5;
    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mwu;
    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mwd;
    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mwl;
    private readonly ObservableAsPropertyHelper<BaseButtonMappingVm> _mwr;

    public ProfileVm(List<BaseButtonMappingVm> buttonMappings)
    {
        _btnSc = new SourceCache<BaseButtonMappingVm, int>(x => x.Id);
        _btnSc.Edit(x => x.AddOrUpdate(buttonMappings));
        _btnSc.Connect().AutoRefresh().Bind(out _btnsMappings).Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb1)
            .AutoRefresh()
            .Bind(out _mb1Mappings)
            .Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb2)
            .AutoRefresh()
            .Bind(out _mb2Mappings)
            .Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb3)
            .AutoRefresh()
            .Bind(out _mb3Mappings)
            .Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb4)
            .AutoRefresh()
            .Bind(out _mb4Mappings)
            .Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb5)
            .AutoRefresh()
            .Bind(out _mb5Mappings)
            .Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mwu)
            .AutoRefresh()
            .Bind(out _mwuMappings)
            .Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mwd)
            .AutoRefresh()
            .Bind(out _mwdMappings)
            .Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mwl)
            .AutoRefresh()
            .Bind(out _mwlMappings)
            .Subscribe();
        _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mwr)
            .AutoRefresh()
            .Bind(out _mwrMappings)
            .Subscribe();

        _mb1 = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb1)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseButton1);
        _mb2 = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb2)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseButton2);
        _mb3 = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb3)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseButton3);
        _mb4 = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb4)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseButton4);
        _mb5 = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mb5)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseButton5);
        _mwu = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mwu)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseWheelUp);
        _mwd = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mwd)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseWheelDown);
        _mwl = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mwl)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseWheelLeft);
        _mwr = _btnSc
            .Connect()
            .Filter(x => x.MouseButton == MouseButton.Mwr)
            .WhenPropertyChanged(x => x.Selected)
            .Where(x => x.Value)
            .Select(x => x.Sender)
            .ToProperty(this, x => x.MouseWheelRight);
    }

    [JsonIgnore]
    public SourceCache<BaseButtonMappingVm, int> BtnSc => _btnSc;

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

    public ReadOnlyObservableCollection<BaseButtonMappingVm> ButtonMappings => _btnsMappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> Mb1Mappings => _mb1Mappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> Mb2Mappings => _mb2Mappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> Mb3Mappings => _mb3Mappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> Mb4Mappings => _mb4Mappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> Mb5Mappings => _mb5Mappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> MwuMappings => _mwuMappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> MwdMappings => _mwdMappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> MwlMappings => _mwlMappings;

    [JsonIgnore]
    public ReadOnlyObservableCollection<BaseButtonMappingVm> MwrMappings => _mwrMappings;

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton1 => _mb1.Value;

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton2 => _mb2.Value;

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton3 => _mb3.Value;

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton4 => _mb4.Value;

    [JsonIgnore]
    public BaseButtonMappingVm MouseButton5 => _mb5.Value;

    [JsonIgnore]
    public BaseButtonMappingVm MouseWheelUp => _mwu.Value;

    [JsonIgnore]
    public BaseButtonMappingVm MouseWheelDown => _mwd.Value;

    [JsonIgnore]
    public BaseButtonMappingVm MouseWheelLeft => _mwl.Value;

    [JsonIgnore]
    public BaseButtonMappingVm MouseWheelRight => _mwr.Value;

    public int DisplayPriority
    {
        get => _displayPriority;
        set => this.RaiseAndSetIfChanged(ref _displayPriority, value);
    }

    public ProfileVm Clone() =>
        new(ButtonMappings.Select(x => x.Clone()).ToList())
        {
            Id = Id,
            Checked = Checked,
            DisplayPriority = DisplayPriority,
            IsDefault = IsDefault,
            Name = Name,
            Description = Description,
            MatchType = MatchType,
            ParentClass = ParentClass,
            Process = Process,
            WindowClass = WindowClass,
            WindowCaption = WindowCaption,
        };

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
