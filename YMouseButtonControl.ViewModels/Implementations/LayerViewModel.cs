using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Implementations;

public class LayerViewModel : ViewModelBase, ILayerViewModel
{
    private readonly IProfileOperationsMediator _profileOperationsMediator;

    public LayerViewModel(IProfileOperationsMediator profileOperationsMediator)
    {
        _profileOperationsMediator = profileOperationsMediator;
        _profileOperationsMediator.SelectedProfileChanged += OnSelectedProfileChanged;
    }

    public int MouseButton4LastIndex => _profileOperationsMediator.CurrentProfile.MouseButton4LastIndex;

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
}