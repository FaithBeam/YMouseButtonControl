using Riok.Mapperly.Abstractions;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.Mappings;

[Mapper]
public static partial class SettingMapper
{
    [MapDerivedType<SettingString, SettingStringVm>]
    [MapDerivedType<SettingBool, SettingBoolVm>]
    [MapDerivedType<SettingInt, SettingIntVm>]
    private static partial BaseSettingVm Map(Setting setting);

    [MapDerivedType<SettingStringVm, SettingString>]
    [MapDerivedType<SettingBoolVm, SettingBool>]
    [MapDerivedType<SettingIntVm, SettingInt>]
    private static partial Setting Map(BaseSettingVm baseSettingVm);
}
