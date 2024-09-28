using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.Services.Profiles;

namespace YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;

public interface IApply
{
    void ApplyProfiles();
}

public class Apply(IUnitOfWork unitOfWork, IProfilesService profilesService) : IApply
{
    public void ApplyProfiles()
    {
        foreach (var dbVm in unitOfWork.ProfileRepo.Get())
        {
            unitOfWork.ProfileRepo.Remove(dbVm.Id);
        }
        unitOfWork.Save();
        foreach (var vm in profilesService.Profiles)
        {
            unitOfWork.ProfileRepo.Update(vm.Id, vm);
        }
        unitOfWork.Save();
    }
}
