using System.Transactions;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;

public interface IApply
{
    void ApplyProfiles();
}

public class Apply(
    IRepository<Profile, ProfileVm> profileRepository,
    IRepository<ButtonMapping, BaseButtonMappingVm> buttonMappingRepository,
    IProfilesService profilesService
) : IApply
{
    public void ApplyProfiles()
    {
        using var trn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        foreach (var dbVm in profileRepository.GetAll())
        {
            profileRepository.Delete(dbVm);
        }
        foreach (var vm in profilesService.Profiles)
        {
            var profileId = profileRepository.Add(vm);
            foreach (var bm in vm.ButtonMappings)
            {
                if (bm.ProfileId <= 0)
                {
                    bm.ProfileId = profileId;
                }
                buttonMappingRepository.Add(bm);
            }
        }
        trn.Complete();
    }
}
