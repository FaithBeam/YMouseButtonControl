using System;
using Newtonsoft.Json;
using ReactiveUI;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.Core.DataAccess.Models.Implementations;

[JsonObject(MemberSerialization.OptOut)]
public class Profile : ReactiveObject, IEquatable<Profile>
{
    private IButtonMapping _mb1 = new NothingMapping();
    private IButtonMapping _mb2 = new NothingMapping();
    private IButtonMapping _mb3 = new NothingMapping();
    private IButtonMapping _mb4 = new NothingMapping();
    private IButtonMapping _mb5 = new NothingMapping();
    private IButtonMapping _mwu = new NothingMapping();
    private IButtonMapping _mwd = new NothingMapping();
    private IButtonMapping _mwl = new NothingMapping();
    private IButtonMapping _mwr = new NothingMapping();

    private int _displayPriority;

    private bool _isChecked = true;

    private string _description = "N/A";
    private string _windowCaption = "N/A";
    private string _process = "N/A";
    private string _windowClass = "N/A";
    private string _parentClass = "N/A";
    private string _matchType = "N/A";

    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// The display priority of a profile. Lower values should appear first in a list. Higher values should appear later in a list.
    /// </summary>
    public int DisplayPriority
    {
        get => _displayPriority;
        set => this.RaiseAndSetIfChanged(ref _displayPriority, value);
    }

    public bool IsDefault { get; set; }

    public bool Checked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }

    public string Name { get; set; } = string.Empty;

    public IButtonMapping MouseButton1
    {
        get => _mb1;
        set => this.RaiseAndSetIfChanged(ref _mb1, value);
    }

    public IButtonMapping MouseButton2
    {
        get => _mb2;
        set => this.RaiseAndSetIfChanged(ref _mb2, value);
    }

    public IButtonMapping MouseButton3
    {
        get => _mb3;
        set => this.RaiseAndSetIfChanged(ref _mb3, value);
    }

    public IButtonMapping MouseButton4
    {
        get => _mb4;
        set => this.RaiseAndSetIfChanged(ref _mb4, value);
    }

    public IButtonMapping MouseButton5
    {
        get => _mb5;
        set => this.RaiseAndSetIfChanged(ref _mb5, value);
    }

    public IButtonMapping MouseWheelUp
    {
        get => _mwu;
        set => this.RaiseAndSetIfChanged(ref _mwu, value);
    }

    public IButtonMapping MouseWheelDown
    {
        get => _mwd;
        set => this.RaiseAndSetIfChanged(ref _mwd, value);
    }

    public IButtonMapping MouseWheelLeft
    {
        get => _mwl;
        set => this.RaiseAndSetIfChanged(ref _mwl, value);
    }

    public IButtonMapping MouseWheelRight
    {
        get => _mwr;
        set => this.RaiseAndSetIfChanged(ref _mwr, value);
    }

    public string Description
    {
        get
        {
            if (Name == "Default")
            {
                return "Default";
            }
            return string.IsNullOrWhiteSpace(_description) ? _process : _description;
        }
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public string WindowCaption
    {
        get => _windowCaption;
        set => this.RaiseAndSetIfChanged(ref _windowCaption, value);
    }

    public string Process
    {
        get => _process;
        set => this.RaiseAndSetIfChanged(ref _process, value);
    }

    public string WindowClass
    {
        get => _windowClass;
        set => this.RaiseAndSetIfChanged(ref _windowClass, value);
    }

    public string ParentClass
    {
        get => _parentClass;
        set => this.RaiseAndSetIfChanged(ref _parentClass, value);
    }

    public string MatchType
    {
        get => _matchType;
        set => this.RaiseAndSetIfChanged(ref _matchType, value);
    }

    public bool Equals(Profile? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Checked == other.Checked
            && Name == other.Name
            && MouseButton1.Equals(other.MouseButton1)
            && MouseButton2.Equals(other.MouseButton2)
            && MouseButton3.Equals(other.MouseButton3)
            && MouseButton4.Equals(other.MouseButton4)
            && MouseButton5.Equals(other.MouseButton5)
            && MouseWheelUp.Equals(other.MouseWheelUp)
            && MouseWheelDown.Equals(other.MouseWheelDown)
            && MouseWheelLeft.Equals(other.MouseWheelLeft)
            && MouseWheelRight.Equals(other.MouseWheelRight)
            && Description == other.Description
            && WindowCaption == other.WindowCaption
            && Process == other.Process
            && WindowClass == other.WindowClass
            && ParentClass == other.ParentClass
            && MatchType == other.MatchType;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((Profile)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Checked.GetHashCode();
            hashCode = (hashCode * 397) ^ Name.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseButton1.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseButton2.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseButton3.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseButton4.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseButton5.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseWheelUp.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseWheelDown.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseWheelLeft.GetHashCode();
            hashCode = (hashCode * 397) ^ MouseWheelRight.GetHashCode();
            hashCode = (hashCode * 397) ^ Description.GetHashCode();
            hashCode = (hashCode * 397) ^ WindowCaption.GetHashCode();
            hashCode = (hashCode * 397) ^ Process.GetHashCode();
            hashCode = (hashCode * 397) ^ WindowClass.GetHashCode();
            hashCode = (hashCode * 397) ^ ParentClass.GetHashCode();
            hashCode = (hashCode * 397) ^ MatchType.GetHashCode();
            return hashCode;
        }
    }
}
