using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Collections;
using ReactiveUI;
using YMouseButtonControl.Core.Config;
using YMouseButtonControl.Core.Factories;
using YMouseButtonControl.Core.Models;
using YMouseButtonControl.Core.Models.SimulatedKeystrokesTypes;

namespace YMouseButtonControl.Core.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    #region Fields

    private Profile _curProfile;
    private int _mb4LastIndex;

    #endregion

    #region Constructor

    public MainWindowViewModel(AvaloniaList<Profile> profiles)
    {
        Profiles = profiles;
        CurrentProfile = Profiles.First();
        ApplyCommand = ReactiveCommand.CreateFromTask(OnClickedApply);
    }

    #endregion

    #region Properties
    
    public ICommand ApplyCommand { get; }

    public int MouseButton4LastIndex
    {
        get => _mb4LastIndex;
        set => this.RaiseAndSetIfChanged(ref _mb4LastIndex, value);
    }

    public Profile CurrentProfile
    {
        get => _curProfile;
        set
        {
            if (_curProfile is not null)
            {
                _curProfile.MouseButton4LastIndex = MouseButton4LastIndex;
            }
            this.RaiseAndSetIfChanged(ref _curProfile, value);
            ResetMouseCombos();
            MouseButton4LastIndex = _curProfile.MouseButton4LastIndex;
        }
    }
    public AvaloniaList<string> MouseButton4Combo { get; set; } = new(ButtonMappingFactory.ButtonMappings);

    public AvaloniaList<Profile> Profiles { get; }

    #endregion

    #region Methods

    private async Task OnClickedApply()
    {
        await Task.Run(() => SaveProfiles.Save(Profiles));
    }

    private void ResetMouseCombos()
    {
        ResetCombo(MouseButton4Combo, CurrentProfile.MouseButton4);
    }

    private void ResetCombo(IAvaloniaList<string> comboList, IButtonMapping mapping)
    {
        for (var i = 0; i < ButtonMappingFactory.ButtonMappings.Count; i++)
        {
            if (mapping is not null && i == mapping.Index)
            {
                comboList[i] = mapping.ToString();
            }
            else
            {
                comboList[i] = ButtonMappingFactory.ButtonMappings[i];
            }
        }
    }
    
    #endregion

    
}