using Avalonia.Collections;
using Avalonia.Media;
using ReactiveUI;
using YMouseButtonControl.KeyboardAndMouse;
using YMouseButtonControl.Services.Abstractions.Enums;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class LayerViewModel : ViewModelBase, ILayerViewModel
{
    private readonly IProfileOperationsMediator _profileOperationsMediator;
    private readonly IMouseListener _mouseListener;
    private IBrush _mouseButton1BackgroundColor = Brushes.White;

    public LayerViewModel(IProfileOperationsMediator profileOperationsMediator, IMouseListener mouseListener)
    {
        _profileOperationsMediator = profileOperationsMediator;
        _profileOperationsMediator.SelectedProfileChanged += OnSelectedProfileChanged;
        _mouseListener = mouseListener;
        _mouseListener.OnMousePressedEventHandler += OnMouseClicked;
        _mouseListener.OnMouseReleasedEventHandler += OnMouseReleased;
    }

    public int MouseButton4LastIndex => _profileOperationsMediator.CurrentProfile.MouseButton4LastIndex;

    public IBrush MouseButton1BackgroundColor
    {
        get => _mouseButton1BackgroundColor;
        set => this.RaiseAndSetIfChanged(ref _mouseButton1BackgroundColor, value);
    }

    public AvaloniaList<string> MouseButton4Combo { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);

    private void OnSelectedProfileChanged(object sender, SelectedProfileChangedEventArgs e)
    {
        MouseButton4Combo = new AvaloniaList<string>(Factories.ButtonMappingFactory.ButtonMappings)
        {
            [_profileOperationsMediator.CurrentProfile.MouseButton4.Index] =
                _profileOperationsMediator.CurrentProfile.MouseButton4.ToString()
        };
        this.RaisePropertyChanged(nameof(MouseButton4Combo));
        this.RaisePropertyChanged(nameof(MouseButton4LastIndex));
    }

    private void OnMouseReleased(object sender, NewMouseHookEventArgs e)
    {
        if (e.Button == NewMouseButton.Button1)
        {
            MouseButton1BackgroundColor = Brushes.White;
        }
    }

    private void OnMouseClicked(object sender, NewMouseHookEventArgs e)
    {
        if (e.Button == NewMouseButton.Button1)
        {
            MouseButton1BackgroundColor = Brushes.Yellow;
        }
    }
}