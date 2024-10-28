using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.MainWindow;
using YMouseButtonControl.Core.Views;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class MainWindow : ReactiveWindow<IMainWindowViewModel>, IMainWindow
{
    private readonly IMainWindowProvider _mainWindowProvider;

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

        _mainWindowProvider = Program.Container!.GetRequiredService<IMainWindowProvider>();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    public MainWindow(IMainWindowProvider mainWindowProvider)
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

        _mainWindowProvider = mainWindowProvider;

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
            _mainWindowProvider.GetMainWindow()
        );
        context.SetOutput(Unit.Default);
    }
}
