using System;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Services.Implementations;

public class CurrentCurrentProfileOperationsMediator: ICurrentProfileOperationsMediator
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

    public event EventHandler<SelectedProfileChangedEventArgs> SelectedProfileChanged;
    
    private void OnProfileChanged(SelectedProfileChangedEventArgs e)
    {
        var handler = SelectedProfileChanged;
        handler?.Invoke(this, e);
    }
}