using System.Linq;
using System.Transactions;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;

public interface IApply
{
    void ApplyProfiles();
}

public class Apply(
    IRepository<Profile, ProfileVm> profileRepository,
    //IRepository<ButtonMapping, BaseButtonMappingVm> buttonMappingRepository,
    IProfilesService profilesService
) : IApply
{
    public void ApplyProfiles()
    {
        var dbProfiles = profileRepository.GetAll();

        // delete profiles
        dbProfiles
            .Where(x => !profilesService.Profiles.Any(y => y.Id == x.Id))
            .ToList()
            .ForEach(x => profileRepository.Delete(x));

        // update profiles
        profilesService
            .Profiles.Where(x => dbProfiles.Any(y => y.Id == x.Id))
            .ToList()
            .ForEach(x => profileRepository.Update(x));

        // add profiles
        profilesService
            .Profiles.Where(x => !dbProfiles.Any(y => y.Id == x.Id))
            .ToList()
            .ForEach(x => profileRepository.Add(x));
    }
}
