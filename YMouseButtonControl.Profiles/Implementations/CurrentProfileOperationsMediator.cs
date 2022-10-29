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
    
    public void UpdateMouseButton1(IButtonMapping value)
    {
        CurrentProfile.MouseButton1 = value;
        CurrentProfile.MouseButton1LastIndex = value.Index;
        CurrentProfile.MouseButton1.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }
    
    public void UpdateMouseButton2(IButtonMapping value)
    {
        CurrentProfile.MouseButton2 = value;
        CurrentProfile.MouseButton2LastIndex = value.Index;
        CurrentProfile.MouseButton2.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }
    
    public void UpdateMouseButton3(IButtonMapping value)
    {
        CurrentProfile.MouseButton3 = value;
        CurrentProfile.MouseButton3LastIndex = value.Index;
        CurrentProfile.MouseButton3.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }
    
    public void UpdateMouseButton4(IButtonMapping value)
    {
        CurrentProfile.MouseButton4 = value;
        CurrentProfile.MouseButton4LastIndex = value.Index;
        CurrentProfile.MouseButton4.CanRaiseDialog = false;
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
    
    public void UpdateMouseWheelUp(IButtonMapping value)
    {
        CurrentProfile.MouseWheelUp = value;
        CurrentProfile.MouseWheelUpLastIndex = value.Index;
        CurrentProfile.MouseWheelUp.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }
    
    public void UpdateMouseWheelDown(IButtonMapping value)
    {
        CurrentProfile.MouseWheelDown = value;
        CurrentProfile.MouseWheelDownLastIndex = value.Index;
        CurrentProfile.MouseWheelDown.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }
    
    public void UpdateMouseWheelLeft(IButtonMapping value)
    {
        CurrentProfile.MouseWheelLeft = value;
        CurrentProfile.MouseWheelLeftLastIndex = value.Index;
        CurrentProfile.MouseWheelLeft.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }

    public void UpdateMouseWheelRight(IButtonMapping value)
    {
        CurrentProfile.MouseWheelRight = value;
        CurrentProfile.MouseWheelRightLastIndex = value.Index;
        CurrentProfile.MouseWheelRight.CanRaiseDialog = false;
        var args = new SelectedProfileChangedEventArgs(_currentProfile);
        OnCurrentProfileChanged(args);
    }
    
    private void OnCurrentProfileChanged(SelectedProfileChangedEventArgs e)
    {
        var handler = CurrentProfileChanged;
        handler?.Invoke(this, e);
    }
}