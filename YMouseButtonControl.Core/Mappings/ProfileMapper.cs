using System.Collections.Generic;
using System.Linq;
using Riok.Mapperly.Abstractions;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class ProfileMapper
{
    public static partial ProfileVm Map(Profile? profile);

    public static partial Profile Map(ProfileVm vm);

    public static partial void Map(ProfileVm src, Profile dst);

    private static List<BaseButtonMappingVm> MapButtonMapping(
        ICollection<ButtonMapping> buttonMappings
    ) => buttonMappings.Select(ButtonMappingMapper.Map).ToList();

    private static ICollection<ButtonMapping> MapButtonMappingVms(
        List<BaseButtonMappingVm> buttonMappings
    ) => buttonMappings.Select(ButtonMappingMapper.Map).ToList();
}
