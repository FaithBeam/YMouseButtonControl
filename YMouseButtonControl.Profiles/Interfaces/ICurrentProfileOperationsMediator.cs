using System;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Profiles.Interfaces;

public interface ICurrentProfileOperationsMediator
{
    Profile CurrentProfile { get; set; }
    event EventHandler<SelectedProfileChangedEventArgs> CurrentProfileChanged;
    void UpdateMouseButton1(IButtonMapping value);
    void UpdateMouseButton2(IButtonMapping value);
    void UpdateMouseButton3(IButtonMapping value);
    void UpdateMouseButton4(IButtonMapping value);
    void UpdateMouseButton5(IButtonMapping value);
    void UpdateMouseWheelUp(IButtonMapping value);
    void UpdateMouseWheelDown(IButtonMapping value);
    void UpdateMouseWheelLeft(IButtonMapping value);
    void UpdateMouseWheelRight(IButtonMapping value);
}