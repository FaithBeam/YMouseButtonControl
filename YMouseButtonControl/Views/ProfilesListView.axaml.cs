using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Avalonia.Implementations;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces.Dialogs;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class ProfilesListView : ReactiveUserControl<ProfilesListViewModel>
{
    public ProfilesListView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is null) return;
            d(ViewModel.ShowProcessSelectorInteraction.RegisterHandler(async x =>
                await ShowProcessSelectorDialogAsync(x)));
            d(ViewModel.ShowExportFileDialog.RegisterHandler(ShowExportFileDialog));
            d(ViewModel.ShowImportFileDialog.RegisterHandler(ShowImportFileDialog));
        });
    }

    private async Task ShowImportFileDialog(IInteractionContext<Unit, string> interactionContext)
    {
        var result = await new Window().StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType(".json") { Patterns = new[] { "*.json" } } }
        });
        if (result.Any())
        {
            var f = result[0];
            var localPath = f.TryGetLocalPath();
            if (string.IsNullOrWhiteSpace(localPath))
            {
                interactionContext.SetOutput(localPath!);
                return;
            }
        }
        
        interactionContext.SetOutput(string.Empty);
    }

    private async Task ShowExportFileDialog(IInteractionContext<string, string> interactionContext)
    {
        var file = await new Window().StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save As",
            DefaultExtension = ".json",
            SuggestedFileName = $"{interactionContext.Input}.json",
            FileTypeChoices = new[] { new FilePickerFileType("json") { Patterns = new[] { "*.json" } } }
        });
        if (file is not null)
        {
            var localPath = file.TryGetLocalPath();
            if (string.IsNullOrWhiteSpace(localPath))
            {
                interactionContext.SetOutput(localPath!);
                return;
            }
        }
        interactionContext.SetOutput(string.Empty);
    }

    private async Task ShowProcessSelectorDialogAsync(
        IInteractionContext<IProcessSelectorDialogViewModel, Profile?> interaction
    )
    {
        interaction.Input.RefreshButtonCommand.Execute(null);
        var dialog = new ProcessSelectorDialog
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<Profile?>(MainWindowProvider.GetMainWindow());
        interaction.SetOutput(result);
    }
}