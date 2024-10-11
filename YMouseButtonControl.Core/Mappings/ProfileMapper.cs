using System.Linq;
using Riok.Mapperly.Abstractions;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.Mappings;

[Mapper]
public static partial class ProfileMapper
{
    public static partial ProfileVm Map(Profile? profile);

    public static partial Profile Map(ProfileVm vm);

    public static partial IQueryable<ProfileVm> Map(IQueryable<Profile> queryable);

    public static partial void Map(ProfileVm src, Profile dst);
}
