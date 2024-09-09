using System.Reactive;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Interfaces;

namespace YMouseButtonControl.Core.ViewModels.MainWindow;

public interface IMainWindowViewModel
{
    IProfilesInformationViewModel ProfilesInformationViewModel { get; }
    IProfilesListViewModel ProfilesListViewModel { get; }
    ILayerViewModel LayerViewModel { get; }
    ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    ReactiveCommand<Unit, Unit> CloseCommand { get; }
    ReactiveCommand<Unit, Unit> SettingsCommand { get; }
    Interaction<Unit, Unit> ShowSettingsDialogInteraction { get; }
    string? ProfileName { get; set; }
}
