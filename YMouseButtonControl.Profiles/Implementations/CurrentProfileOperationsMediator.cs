using System;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Implementations;

public class CurrentProfileOperationsMediator : ICurrentProfileOperationsMediator
{
    private Profile _currentProfile;
    
    public Profile CurrentProfile
    {
        get => _currentProfile;
        set
        {
            if (_currentProfile == value)
            {
                return;
            }

            _currentProfile = value;

            if (value is null)
            {
                return;
            }
            var args = new SelectedProfileChangedEventArgs(_currentProfile);
            OnCurrentProfileChanged(args);
        }
    }
    
    public event EventHandler<SelectedProfileChangedEventArgs> CurrentProfileChanged;

    public void SetMouseButton5LastIndex(int index)
    {
        CurrentProfile.MouseButton5LastIndex = index;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }

    public void UpdateMouseButton5(IButtonMapping value)
    {
        CurrentProfile.MouseButton5 = value;
        CurrentProfile.MouseButton5LastIndex = value.Index;
        CurrentProfile.MouseButton5.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }

    private void OnCurrentProfileChanged(SelectedProfileChangedEventArgs e)
    {
        var handler = CurrentProfileChanged;
        handler?.Invoke(this, e);
    }
}