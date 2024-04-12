using Avalonia;
using Avalonia.ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Implementations;

namespace YMouseButtonControl.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }
}
