using System;
using YMouseButtonControl.DataAccess.Models.Enums;
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
    public event EventHandler<SelectedProfileEditedEventArgs> CurrentProfileEdited;
    
    public void UpdateMouseButton1(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseButton1 = value;
        CurrentProfile.MouseButton1LastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseButton1);
        OnCurrentProfileEdited(args);
    }
    
    public void UpdateMouseButton2(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseButton2 = value;
        CurrentProfile.MouseButton2LastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseButton2);
        OnCurrentProfileEdited(args);
    }
    
    public void UpdateMouseButton3(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseButton3 = value;
        CurrentProfile.MouseButton3LastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseButton3);
        OnCurrentProfileEdited(args);
    }
    
    public void UpdateMouseButton4(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseButton4 = value;
        CurrentProfile.MouseButton4LastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseButton4);
        OnCurrentProfileEdited(args);
    }

    public void UpdateMouseButton5(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseButton5 = value;
        CurrentProfile.MouseButton5LastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseButton5);
        OnCurrentProfileEdited(args);
    }
    
    public void UpdateMouseWheelUp(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseWheelUp = value;
        CurrentProfile.MouseWheelUpLastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseWheelUp);
        OnCurrentProfileEdited(args);
    }
    
    public void UpdateMouseWheelDown(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseWheelDown = value;
        CurrentProfile.MouseWheelDownLastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseWheelDown);
        OnCurrentProfileEdited(args);
    }
    
    public void UpdateMouseWheelLeft(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseWheelLeft = value;
        CurrentProfile.MouseWheelLeftLastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseWheelLeft);
        OnCurrentProfileEdited(args);
    }

    public void UpdateMouseWheelRight(IButtonMapping value)
    {
        if (value is null)
        {
            return;
        }
        CurrentProfile.MouseWheelRight = value;
        CurrentProfile.MouseWheelRightLastIndex = value.Index;
        var args = new SelectedProfileEditedEventArgs(value, MouseButton.MouseWheelRight);
        OnCurrentProfileEdited(args);
    }
    
    private void OnCurrentProfileChanged(SelectedProfileChangedEventArgs e)
    {
        CurrentProfileChanged?.Invoke(this, e);
    }

    private void OnCurrentProfileEdited(SelectedProfileEditedEventArgs e)
    {
        CurrentProfileEdited?.Invoke(this, e);
    }
}