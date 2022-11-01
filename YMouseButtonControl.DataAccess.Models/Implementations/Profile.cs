using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations
{
    public class Profile : IEquatable<Profile>
    {
        public int Id { get; set; }
        public bool Checked { get; set; }

        public string Name { get; set; } = string.Empty;

        public IButtonMapping MouseButton1 { get; set; } = new NothingMapping();

        public IButtonMapping MouseButton2 { get; set; } = new NothingMapping();

        public IButtonMapping MouseButton3 { get; set; } = new NothingMapping();

        public IButtonMapping MouseButton4 { get; set; } = new NothingMapping();

        public IButtonMapping MouseButton5 { get; set; } = new NothingMapping();

        public IButtonMapping MouseWheelUp { get; set; } = new NothingMapping();

        public IButtonMapping MouseWheelDown { get; set; } = new NothingMapping();

        public IButtonMapping MouseWheelLeft { get; set; } = new NothingMapping();

        public IButtonMapping MouseWheelRight { get; set; } = new NothingMapping();

        public int MouseButton1LastIndex { get; set; } = 0;

        public int MouseButton2LastIndex { get; set; } = 0;
        
        public int MouseButton3LastIndex { get; set; } = 0;

        public int MouseButton4LastIndex { get; set; } = 0;
        
        public int MouseButton5LastIndex { get; set; } = 0;
        
        public int MouseWheelUpLastIndex { get; set; } = 0;
        
        public int MouseWheelDownLastIndex { get; set; } = 0;
        
        public int MouseWheelLeftLastIndex { get; set; } = 0;
        
        public int MouseWheelRightLastIndex { get; set; } = 0;

        public string Description { get; set; } = "N/A";

        public string WindowCaption { get; set; } = "N/A";

        public string Process { get; set; } = "N/A";

        public string WindowClass { get; set; } = "N/A";

        public string ParentClass { get; set; } = "N/A";

        public string MatchType { get; set; } = "N/A";

        public bool Equals(Profile? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Checked == other.Checked && Name == other.Name && MouseButton1.Equals(other.MouseButton1) && MouseButton2.Equals(other.MouseButton2) && MouseButton3.Equals(other.MouseButton3) && MouseButton4.Equals(other.MouseButton4) && MouseButton5.Equals(other.MouseButton5) && MouseWheelUp.Equals(other.MouseWheelUp) && MouseWheelDown.Equals(other.MouseWheelDown) && MouseWheelLeft.Equals(other.MouseWheelLeft) && MouseWheelRight.Equals(other.MouseWheelRight) && MouseButton1LastIndex == other.MouseButton1LastIndex && MouseButton2LastIndex == other.MouseButton2LastIndex && MouseButton3LastIndex == other.MouseButton3LastIndex && MouseButton4LastIndex == other.MouseButton4LastIndex && MouseButton5LastIndex == other.MouseButton5LastIndex && MouseWheelUpLastIndex == other.MouseWheelUpLastIndex && MouseWheelDownLastIndex == other.MouseWheelDownLastIndex && MouseWheelLeftLastIndex == other.MouseWheelLeftLastIndex && MouseWheelRightLastIndex == other.MouseWheelRightLastIndex && Description == other.Description && WindowCaption == other.WindowCaption && Process == other.Process && WindowClass == other.WindowClass && ParentClass == other.ParentClass && MatchType == other.MatchType;
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
                hashCode = (hashCode * 397) ^ MouseButton1LastIndex;
                hashCode = (hashCode * 397) ^ MouseButton2LastIndex;
                hashCode = (hashCode * 397) ^ MouseButton3LastIndex;
                hashCode = (hashCode * 397) ^ MouseButton4LastIndex;
                hashCode = (hashCode * 397) ^ MouseButton5LastIndex;
                hashCode = (hashCode * 397) ^ MouseWheelUpLastIndex;
                hashCode = (hashCode * 397) ^ MouseWheelDownLastIndex;
                hashCode = (hashCode * 397) ^ MouseWheelLeftLastIndex;
                hashCode = (hashCode * 397) ^ MouseWheelRightLastIndex;
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
}