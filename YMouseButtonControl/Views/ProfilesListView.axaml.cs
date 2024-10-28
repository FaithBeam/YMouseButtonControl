using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Core.ViewModels.ProfilesList;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class ProfilesListView : ReactiveUserControl<ProfilesListViewModel>
{
    private readonly IMainWindowProvider _mainWindowProvider;

    public ProfilesListView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is null)
                return;
            d(
                ViewModel.ShowProcessSelectorInteraction.RegisterHandler(async x =>
                    await ShowProcessSelectorDialogAsync(x)
                )
            );
            d(ViewModel.ShowExportFileDialog.RegisterHandler(ShowExportFileDialog));
            d(ViewModel.ShowImportFileDialog.RegisterHandler(ShowImportFileDialog));
        });
        _mainWindowProvider = Program.Container!.GetRequiredService<IMainWindowProvider>();
    }

    private static async Task ShowImportFileDialog(
        IInteractionContext<Unit, string> interactionContext
    )
    {
        var result = await new Window().StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Open",
                AllowMultiple = false,
                FileTypeFilter = [new FilePickerFileType(".json") { Patterns = ["*.json"] }],
            }
        );
        if (result.Any())
        {
            var f = result[0];
            interactionContext.SetOutput(
                f.TryGetLocalPath() ?? throw new Exception("Error retrieving chosen path")
            );
            return;
        }

        interactionContext.SetOutput(string.Empty);
    }

    private static async Task ShowExportFileDialog(
        IInteractionContext<string, string> interactionContext
    )
    {
        var file = await new Window().StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions
            {
                Title = "Save As",
                DefaultExtension = ".json",
                SuggestedFileName = $"{interactionContext.Input}.json",
                FileTypeChoices = [new FilePickerFileType("json") { Patterns = ["*.json"] }],
            }
        );
        if (file is not null)
        {
            interactionContext.SetOutput(
                file.TryGetLocalPath() ?? throw new Exception("Error retrieving chosen path")
            );
            return;
        }

        interactionContext.SetOutput(string.Empty);
    }

    private async Task ShowProcessSelectorDialogAsync(
        IInteractionContext<IProcessSelectorDialogViewModel, ProfileVm?> interaction
    )
    {
        interaction.Input.RefreshButtonCommand.Execute(null);
        var dialog = new ProcessSelectorDialog { DataContext = interaction.Input };
        var mw = _mainWindowProvider.GetMainWindow();
        var result = await dialog.ShowDialog<ProfileVm?>(_mainWindowProvider.GetMainWindow());
        interaction.SetOutput(result);
    }
}
