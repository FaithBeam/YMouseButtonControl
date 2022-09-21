using System.Collections.Generic;
using YMouseButtonControl.Core.Models;

namespace YMouseButtonControl.Core.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public List<Profile> Profiles { get; }

    public MainWindowViewModel()
    {
        Profiles = new List<Profile>
        {
            new()
            {
                Name = "Default",
                Checked = true
            }
        };
    }
}