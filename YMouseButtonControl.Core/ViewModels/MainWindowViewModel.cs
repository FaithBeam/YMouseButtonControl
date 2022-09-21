using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.Models;

namespace YMouseButtonControl.Core.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Profile _currentProfile;
    public List<Profile> Profiles { get; }

    public Profile CurrentProfile
    {
        get => _currentProfile;
        set => this.RaiseAndSetIfChanged(ref _currentProfile, value);
    }

    public MainWindowViewModel()
    {
        Profiles = new List<Profile>
        {
            new()
            {
                Name = "Default",
                Checked = true,
                Description = "N/A",
                Process = "N/A",
                MatchType = "N/A",
                ParentClass = "N/A",
                WindowCaption = "N/A",
                WindowClass = "N/A"
            },
            new()
            {
                Name = "Profile 2",
                Checked = true,
                Description = "N/A",
                Process = "OG",
                MatchType = "N/A",
                ParentClass = "N/A",
                WindowCaption = "N/A",
                WindowClass = "N/A"
            }
        };
        CurrentProfile = Profiles.First();
    }
}