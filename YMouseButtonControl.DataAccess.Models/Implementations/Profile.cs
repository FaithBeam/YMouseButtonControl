using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.DataAccess.Models.Implementations
{
    public class Profile
    {
        public bool Checked { get; set; }

        public string Name { get; set; }

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

        public string Description { get; set; }

        public string WindowCaption { get; set; }

        public string Process { get; set; }

        public string WindowClass { get; set; }

        public string ParentClass { get; set; }

        public string MatchType { get; set; }
    }
}