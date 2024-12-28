using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog;
using YMouseButtonControl.Core.ViewModels.ProfilesList;

namespace YMouseButtonControl.Views.Dialogs;

public partial class ProcessSelectorDialog : ReactiveWindow<ProcessSelectorDialogViewModel>
{
    public ProcessSelectorDialog()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is null)
            {
                return;
            }
            d(ViewModel!.OkCommand.Subscribe(Close));
            d(ViewModel.ShowSpecificWindowInteraction.RegisterHandler(ShowFindWindowDialog));
        });
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async Task ShowFindWindowDialog(IInteractionContext<FindWindowDialogVm, Unit> ctx)
    {
        var dialog = new FindWindowDialog { DataContext = ctx.Input };
        var result = await dialog.ShowDialog<Unit>(MainWindowProvider.GetMainWindow());
        ctx.SetOutput(result);
    }
}
