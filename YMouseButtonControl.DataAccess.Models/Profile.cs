namespace YMouseButtonControl.DataAccess.Models
{
    public class Profile
    {
        public bool Checked { get; set; }

        public string Name { get; set; }

        public IButtonMapping MouseButton4 { get; set; }

        public int MouseButton4LastIndex { get; set; } = 0;

        public string Description { get; set; }

        public string WindowCaption { get; set; }

        public string Process { get; set; }

        public string WindowClass { get; set; }

        public string ParentClass { get; set; }

        public string MatchType { get; set; }
    }
}