using Riok.Mapperly.Abstractions;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.Mappings;

[Mapper]
public static partial class SettingMapper
{
    [MapDerivedType<SettingString, SettingStringVm>]
    [MapDerivedType<SettingBool, SettingBoolVm>]
    [MapDerivedType<SettingInt, SettingIntVm>]
    public static partial BaseSettingVm Map(Setting? setting);

    [MapDerivedType<SettingStringVm, SettingString>]
    [MapDerivedType<SettingBoolVm, SettingBool>]
    [MapDerivedType<SettingIntVm, SettingInt>]
    public static partial Setting Map(BaseSettingVm baseSettingVm);

    [MapDerivedType<SettingStringVm, SettingString>]
    [MapDerivedType<SettingBoolVm, SettingBool>]
    [MapDerivedType<SettingIntVm, SettingInt>]
    public static partial void Map(BaseSettingVm src, Setting dst);
}
