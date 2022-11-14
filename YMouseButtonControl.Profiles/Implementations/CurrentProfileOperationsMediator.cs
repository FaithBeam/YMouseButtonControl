using System;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Implementations;

public class CurrentProfileOperationsMediator : ReactiveObject, ICurrentProfileOperationsMediator
{
    private Profile _currentProfile;
    
    public Profile CurrentProfile
    {
        get => _currentProfile;
        set => this.RaiseAndSetIfChanged(ref _currentProfile, value);
    }
    
    public event EventHandler<SelectedProfileChangedEventArgs> CurrentProfileChanged;
    public event EventHandler<SelectedProfileEditedEventArgs> CurrentProfileButtonMappingEdited;

    public void UpdateMouse(IButtonMapping value, MouseButton button)
    {
        var args = new SelectedProfileEditedEventArgs(value, button);
        switch (button)
        {
            case MouseButton.MouseButton1:
                CurrentProfile.MouseButton1 = value;
                CurrentProfile.MouseButton1LastIndex = value.Index;
                break;
            case MouseButton.MouseButton2:
                CurrentProfile.MouseButton2 = value;
                CurrentProfile.MouseButton2LastIndex = value.Index;
                break;
            case MouseButton.MouseButton3:
                CurrentProfile.MouseButton3 = value;
                CurrentProfile.MouseButton3LastIndex = value.Index;
                break;
            case MouseButton.MouseButton4:
                CurrentProfile.MouseButton4 = value;
                CurrentProfile.MouseButton4LastIndex = value.Index;
                break;
            case MouseButton.MouseButton5:
                CurrentProfile.MouseButton5 = value;
                CurrentProfile.MouseButton5LastIndex = value.Index;
                break;
            case MouseButton.MouseWheelUp:
                CurrentProfile.MouseWheelUp = value;
                CurrentProfile.MouseWheelUpLastIndex = value.Index;
                break;
            case MouseButton.MouseWheelDown:
                CurrentProfile.MouseWheelDown = value;
                CurrentProfile.MouseWheelDownLastIndex = value.Index;
                break;
            case MouseButton.MouseWheelLeft:
                CurrentProfile.MouseWheelLeft = value;
                CurrentProfile.MouseWheelLeftLastIndex = value.Index;
                break;
            case MouseButton.MouseWheelRight:
                CurrentProfile.MouseWheelRight = value;
                CurrentProfile.MouseWheelRightLastIndex = value.Index;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }
        OnCurrentProfileButtonMappingEdited(args);
    }
    
    private void OnCurrentProfileChanged(SelectedProfileChangedEventArgs e)
    {
        CurrentProfileChanged?.Invoke(this, e);
    }

    private void OnCurrentProfileButtonMappingEdited(SelectedProfileEditedEventArgs e)
    {
        CurrentProfileButtonMappingEdited?.Invoke(this, e);
    }
}