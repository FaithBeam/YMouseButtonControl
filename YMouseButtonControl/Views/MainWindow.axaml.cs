using System.Threading.Tasks;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Splat;
using YMouseButtonControl.DependencyInjection;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Implementations.Dialogs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.Views.Dialogs;

namespace YMouseButtonControl.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = Locator.Current.GetRequiredService<IMainWindowViewModel>();
#if DEBUG
        this.AttachDevTools();
#endif
    }
}
