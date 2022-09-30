namespace YMouseButtonControl.ViewModels.Interfaces;

public interface IProfilesInformationViewModel
{
    string Description { get; }
    string WindowCaption { get; }
    string Process { get; }
    string WindowClass { get; }
    string ParentClass { get; }
    string MatchType { get; }
}