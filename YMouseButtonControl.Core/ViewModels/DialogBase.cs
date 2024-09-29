using ReactiveUI;
using YMouseButtonControl.Core.Services.Theme;

namespace YMouseButtonControl.Core.ViewModels;

public class DialogBase(IThemeService themeService) : ReactiveObject
{
    public IThemeService ThemeService { get; } = themeService;
}
