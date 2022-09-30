using System;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.ViewModels.Services.Interfaces;

public interface IProfileOperationsMediator
{
    public Profile CurrentProfile { get; set; }

    public event EventHandler<SelectedProfileChangedEventArgs> SelectedProfileChanged;
}