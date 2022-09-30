using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YMouseButtonControl.Views;

public partial class ProfilesListView : UserControl
{
    public ProfilesListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}