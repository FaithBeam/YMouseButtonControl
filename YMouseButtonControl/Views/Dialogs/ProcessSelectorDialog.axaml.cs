using System.Threading.Tasks;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;

namespace YMouseButtonControl.Views.Dialogs;

public partial class ProcessSelectorDialog : ReactiveWindow<ProcessSelectorDialogViewModel>
{
    public ProcessSelectorDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}