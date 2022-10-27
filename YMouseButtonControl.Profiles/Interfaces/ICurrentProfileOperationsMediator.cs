using System;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Interfaces;

public interface ICurrentProfileOperationsMediator
{
    Profile CurrentProfile { get; set; }
    void SetMouseButton5LastIndex(int index);
    void UpdateMouseButton5(IButtonMapping value);
    event EventHandler<SelectedProfileChangedEventArgs> CurrentProfileChanged;
}