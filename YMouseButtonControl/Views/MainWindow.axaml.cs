using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.Views;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class MainWindow : ReactiveWindow<IMainWindowViewModel>, IMainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is not null)
            {
                d(
                    ViewModel.ShowSettingsDialogInteraction.RegisterHandler(
                        ShowGlobalSettingsDialog
                    )
                );
            }
        });

#if DEBUG
        this.AttachDevTools();
#endif
    }

    private async Task ShowGlobalSettingsDialog(
        IInteractionContext<IGlobalSettingsDialogViewModel, Unit> context
    )
    {
        var dialog = new GlobalSettingsDialog { DataContext = context.Input };
        await dialog.ShowDialog<IGlobalSettingsDialogViewModel?>(
            MainWindowProvider.GetMainWindow()
        );
        context.SetOutput(Unit.Default);
    }
}
