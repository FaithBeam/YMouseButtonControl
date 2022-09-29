using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.ViewModels.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class LayerViewModel : ViewModelBase, ILayerViewModel
{
    private int _mb4LastIndex;
    
    public int MouseButton4LastIndex
    {
        get => _mb4LastIndex;
        set => this.RaiseAndSetIfChanged(ref _mb4LastIndex, value);
    }
    
    public AvaloniaList<string> MouseButton4Combo { get; set; } = new(Factories.ButtonMappingFactory.ButtonMappings);
}