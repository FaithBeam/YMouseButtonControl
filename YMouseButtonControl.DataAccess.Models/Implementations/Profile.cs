using Newtonsoft.Json;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations
{
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

        private string _description = "N/A";
        private string _windowCaption = "N/A";
        private string _process = "N/A";
        private string _windowClass = "N/A";
        private string _parentClass = "N/A";
        private string _matchType = "N/A";

        [JsonIgnore] public int Id { get; set; }
        public bool Checked { get; set; }

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
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _mb1.Equals(other._mb1) && _mb2.Equals(other._mb2) && _mb3.Equals(other._mb3) &&
                   _mb4.Equals(other._mb4) && _mb5.Equals(other._mb5) && _mwu.Equals(other._mwu) &&
                   _mwd.Equals(other._mwd) && _mwl.Equals(other._mwl) && _mwr.Equals(other._mwr) &&
                   _description == other._description && _windowCaption == other._windowCaption &&
                   _process == other._process && _windowClass == other._windowClass &&
                   _parentClass == other._parentClass && _matchType == other._matchType && Id == other.Id &&
                   Checked == other.Checked && Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Profile)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(_mb1);
            hashCode.Add(_mb2);
            hashCode.Add(_mb3);
            hashCode.Add(_mb4);
            hashCode.Add(_mb5);
            hashCode.Add(_mwu);
            hashCode.Add(_mwd);
            hashCode.Add(_mwl);
            hashCode.Add(_mwr);
            hashCode.Add(_description);
            hashCode.Add(_windowCaption);
            hashCode.Add(_process);
            hashCode.Add(_windowClass);
            hashCode.Add(_parentClass);
            hashCode.Add(_matchType);
            hashCode.Add(Id);
            hashCode.Add(Checked);
            hashCode.Add(Name);
            return hashCode.ToHashCode();
        }
    }
}