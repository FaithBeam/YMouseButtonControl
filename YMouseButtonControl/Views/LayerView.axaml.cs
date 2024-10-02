using Avalonia.ReactiveUI;
using YMouseButtonControl.Core.ViewModels.LayerViewModel;

namespace YMouseButtonControl.Views;

public partial class LayerView : ReactiveUserControl<LayerViewModel>
{
    public LayerView()
    {
        InitializeComponent();
    }
}
