using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations
{
    public class Profile
    {
        public bool Checked { get; set; }

        public string Name { get; set; }

        public IButtonMapping MouseButton1 { get; set; } = new NothingMapping();

        public int MouseButton1LastIndex { get; set; }
        
        public IButtonMapping MouseButton2 { get; set; } = new NothingMapping();

        public int MouseButton2LastIndex { get; set; }
        
        public IButtonMapping MouseButton3 { get; set; } = new NothingMapping();

        public int MouseButton3LastIndex { get; set; }

        public IButtonMapping MouseButton4 { get; set; } = new NothingMapping();

        public int MouseButton4LastIndex { get; set; }
        
        public IButtonMapping MouseButton5 { get; set; } = new NothingMapping();

        public int MouseButton5LastIndex { get; set; }
        
        public IButtonMapping WheelUp { get; set; } = new NothingMapping();

        public int WheelUpLastIndex { get; set; }
        
        public IButtonMapping WheelDown { get; set; } = new NothingMapping();

        public int WheelDownLastIndex { get; set; }
        
        public IButtonMapping WheelLeft { get; set; } = new NothingMapping();

        public int WheelLeftLastIndex { get; set; }
        
        public IButtonMapping WheelRight { get; set; } = new NothingMapping();

        public int WheelRightLastIndex { get; set; }

        public string Description { get; set; }

        public string WindowCaption { get; set; }

        public string Process { get; set; }

        public string WindowClass { get; set; }

        public string ParentClass { get; set; }

        public string MatchType { get; set; }
    }
}