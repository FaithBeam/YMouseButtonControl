using Riok.Mapperly.Abstractions;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.Mappings;

[Mapper]
public static partial class ThemeMapper
{
    public static partial ThemeVm Map(Theme? theme);

    public static partial Theme Map(ThemeVm? vm);
}
