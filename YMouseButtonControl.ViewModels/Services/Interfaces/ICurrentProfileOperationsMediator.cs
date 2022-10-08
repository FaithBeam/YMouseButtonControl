using System;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.ViewModels.Services.Interfaces;

public interface ICurrentProfileOperationsMediator
{
    public Profile CurrentProfile { get; set; }

    public event EventHandler<SelectedProfileChangedEventArgs> SelectedProfileChanged;
}