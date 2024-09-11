using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace YMouseButtonControl.Views.Dialogs;

public partial class GlobalSettingsDialog : Window
{
    public GlobalSettingsDialog()
    {
        InitializeComponent();
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
