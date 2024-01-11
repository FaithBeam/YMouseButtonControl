using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Services.Abstractions.Models;

namespace YMouseButtonControl.ViewModels.Interfaces.Dialogs;

public interface IProcessSelectorDialogViewModel
{
    ICommand RefreshButtonCommand { get; }
    ReactiveCommand<Unit, Profile> OkCommand { get; }
    ObservableCollection<ProcessModel> Processes { get; }
    ProcessModel SelectedProcessModel { get; set; }
}