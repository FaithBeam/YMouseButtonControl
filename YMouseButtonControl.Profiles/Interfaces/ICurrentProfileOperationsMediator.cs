using System;
using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Interfaces;

public interface ICurrentProfileOperationsMediator
{
    Profile CurrentProfile { get; set; }
    event EventHandler<SelectedProfileChangedEventArgs> CurrentProfileChanged;
    event EventHandler<SelectedProfileEditedEventArgs> CurrentProfileButtonMappingEdited;
    void UpdateMouse(IButtonMapping value, MouseButton button);
}