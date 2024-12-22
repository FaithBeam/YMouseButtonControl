using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.ViewModels.GlobalSettingsDialog.Commands.UpdateStartsMinimized;

public interface IUpdateStartsMinimized
{
    Task ExecuteAsync(UpdateStartsMinimized.Command c);
}

public sealed class UpdateStartsMinimized(YMouseButtonControlDbContext db) : IUpdateStartsMinimized
{
    public sealed record Command(bool Value);

    public async Task ExecuteAsync(Command c)
    {
        var ent = await db.SettingBools.FirstAsync(x => x.Name == "StartMinimized");
        ent.BoolValue = c.Value;
        await db.SaveChangesAsync();
    }
}
