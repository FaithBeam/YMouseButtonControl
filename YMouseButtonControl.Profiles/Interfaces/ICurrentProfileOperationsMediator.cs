using System;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Interfaces;

public interface ICurrentProfileOperationsMediator
{
    Profile CurrentProfile { get; set; }
    event EventHandler<SelectedProfileChangedEventArgs> CurrentProfileChanged;
    void UpdateMouseButton4(IButtonMapping value);
    void UpdateMouseButton5(IButtonMapping value);
}