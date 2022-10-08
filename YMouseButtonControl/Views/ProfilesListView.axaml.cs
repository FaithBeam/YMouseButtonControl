using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Avalonia.Implementations;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class ProfilesListView : ReactiveUserControl<ProfilesListViewModel>
{
    public ProfilesListView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is not null)
            {
                d(ViewModel.ShowProcessSelectorInteraction.RegisterHandler(ShowProcessSelectorDialogAsync));
            }
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private async Task ShowProcessSelectorDialogAsync(
        InteractionContext<ProcessSelectorDialogViewModel, Profile?> interaction
    )
    {
        var dialog = new ProcessSelectorDialog
        {
            DataContext = interaction.Input
        };

        var result = await dialog.ShowDialog<Profile?>(MainWindowProvider.GetMainWindow());
        interaction.SetOutput(result);
    }
}