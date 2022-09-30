using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YMouseButtonControl.Views;

public partial class ProfileInformationView : UserControl
{
    public ProfileInformationView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}