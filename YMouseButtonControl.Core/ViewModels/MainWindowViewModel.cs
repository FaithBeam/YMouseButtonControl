using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.Core.Models;
using YMouseButtonControl.Core.Models.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.Core.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Profile _curProfile;
    private IButtonMapping _curMb4;
    public AvaloniaList<string> MouseButton4Combo { get; set; }
    public AvaloniaList<Profile> Profiles { get; }

    public Profile CurrentProfile
    {
        get => _curProfile;
        set
        {
            this.RaiseAndSetIfChanged(ref _curProfile, value);   
            UpdateCurrents();
        }
    }

    public IButtonMapping CurrentMouseButton4
    {
        get => _curMb4;
        set
        {
            this.RaiseAndSetIfChanged(ref _curMb4, value);
        }
    }

    public MainWindowViewModel()
    {
        MouseButton4Combo = new AvaloniaList<string>()
        {
            "** No Change (Don't Intercept) **"
        };
        Profiles = new AvaloniaList<Profile>
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
                WindowClass = "N/A",
                MouseButton4 = new SimulatedKeystrokes()
                {
                    CanRaiseDialog = true,
                    Keys = "w",
                    SimulatedKeystrokesType = new StickyHoldActionType()
                }
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

    private void UpdateCurrents()
    {
        CurrentMouseButton4 = CurrentProfile.MouseButton4;
        MouseButton4Combo[0] = CurrentMouseButton4.ToString();
    }
}