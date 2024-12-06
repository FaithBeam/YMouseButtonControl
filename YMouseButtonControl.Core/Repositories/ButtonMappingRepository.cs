using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Queries;
using YMouseButtonControl.Infrastructure.Context;

namespace YMouseButtonControl.Core.Repositories;

public class ButtonMappingRepository(YMouseButtonControlDbContext ctx, ButtonMappingQueries queries)
    : IRepository<ButtonMapping, BaseButtonMappingVm>
{
    private readonly YMouseButtonControlDbContext _ctx = ctx;
    private const string TblName = "ButtonMappings";

    public int Add(BaseButtonMappingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        var ent = ButtonMappingMapper.Map(vm);
        ent.ButtonMappingType = ent switch
        {
            DisabledMapping => ButtonMappingType.Disabled,
            NothingMapping => ButtonMappingType.Nothing,
            SimulatedKeystroke => ButtonMappingType.SimulatedKeystroke,
            RightClick => ButtonMappingType.RightClick,
            _ => throw new System.NotImplementedException(),
        };
        return conn.Execute(queries.Add(), ent);
    }

    public async Task<int> AddAsync(BaseButtonMappingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.ExecuteAsync(queries.Add(), vm);
    }

    public BaseButtonMappingVm? GetById(int id)
    {
        using var conn = _ctx.CreateConnection();
        return ButtonMappingMapper.Map(
            conn.QueryFirstOrDefault<ButtonMapping>(queries.GetById(TblName), id)
        );
    }

    public async Task<BaseButtonMappingVm?> GetByIdAsync(int id)
    {
        using var conn = _ctx.CreateConnection();
        return ButtonMappingMapper.Map(
            await conn.QueryFirstOrDefaultAsync<ButtonMapping>(queries.GetById(TblName), id)
        );
    }

    public IEnumerable<BaseButtonMappingVm> GetAll()
    {
        using var conn = _ctx.CreateConnection();
        return conn.Query<ButtonMapping>(queries.GetAll(TblName)).Select(ButtonMappingMapper.Map);
    }

    public async Task<IEnumerable<BaseButtonMappingVm>> GetAllAsync()
    {
        using var conn = _ctx.CreateConnection();
        return (await conn.QueryAsync<ButtonMapping>(queries.GetAll(TblName))).Select(
            ButtonMappingMapper.Map
        );
    }

    public int Update(BaseButtonMappingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return conn.Execute(queries.Update(), vm);
    }

    public async Task<int> UpdateAsync(BaseButtonMappingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.ExecuteAsync(queries.Update(), vm);
    }

    public int Delete(BaseButtonMappingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return conn.Execute(queries.DeleteById(TblName), vm.Id);
    }

    public Task<int> DeleteAsync(BaseButtonMappingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return conn.ExecuteAsync(queries.DeleteById(TblName), vm.Id);
    }

    public BaseButtonMappingVm? GetByName(string name)
    {
        throw new System.NotImplementedException();
    }
}
