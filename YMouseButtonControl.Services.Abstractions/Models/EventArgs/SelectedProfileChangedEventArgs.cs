using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Services.Abstractions.Models.EventArgs;

public class SelectedProfileChangedEventArgs : System.EventArgs
{
    public Profile NewProfile { get; }
    
    public SelectedProfileChangedEventArgs(Profile newProfile)
    {
        NewProfile = newProfile;
    }
}