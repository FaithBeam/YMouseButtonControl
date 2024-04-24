using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.UnitOfWork;
using YMouseButtonControl.Core.Profiles.Interfaces;

namespace YMouseButtonControl.Core.ViewModels.MainWindow.Features.Apply;

public interface IApply
{
    void ApplyProfiles();
}

public class Apply(IUnitOfWorkFactory unitOfWorkFactory, IProfilesService profilesService) : IApply
{
    public void ApplyProfiles()
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        var repository = unitOfWork.GetRepository<Profile>();
        repository.ApplyAction(profilesService.Profiles);
    }
}
