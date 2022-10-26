using System;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Services.Implementations;

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

            var args = new SelectedProfileChangedEventArgs(_currentProfile);
            OnProfileChanged(args);
        }
    }

    public void SetMouseButton5LastIndex(int index)
    {
        CurrentProfile.MouseButton5LastIndex = index;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnProfileChanged(args);
    }

    public void UpdateMouseButton5(IButtonMapping value)
    {
        CurrentProfile.MouseButton5 = value;
        CurrentProfile.MouseButton5LastIndex = value.Index;
        CurrentProfile.MouseButton5.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnProfileChanged(args);
    }

    public event EventHandler<SelectedProfileChangedEventArgs> SelectedProfileChanged;

    private void OnProfileChanged(SelectedProfileChangedEventArgs e)
    {
        var handler = SelectedProfileChanged;
        handler?.Invoke(this, e);
    }
}