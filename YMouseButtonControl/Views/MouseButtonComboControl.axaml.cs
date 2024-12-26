using Avalonia.ReactiveUI;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.MouseCombo;

namespace YMouseButtonControl.Views;

public partial class MouseButtonComboControl : ReactiveUserControl<IMouseComboViewModel>
{
    public MouseButtonComboControl()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is null)
            {
                return;
            }
        });
    }
}
