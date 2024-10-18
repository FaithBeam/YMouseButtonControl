using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Context;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Queries;
using static Dapper.SqlMapper;

namespace YMouseButtonControl.Core.Repositories;

public class SettingRepository(YMouseButtonControlDbContext ctx, SettingQueries queries)
    : IRepository<Setting, BaseSettingVm>
{
    private readonly YMouseButtonControlDbContext _ctx = ctx;
    private const string TblName = "Settings";

    public int Add(BaseSettingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return conn.Execute(queries.Add(), vm);
    }

    public async Task<int> AddAsync(BaseSettingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.ExecuteAsync(queries.Add(), vm);
    }

    public BaseSettingVm? GetById(int id)
    {
        using var conn = _ctx.CreateConnection();
        return SettingMapper.Map(conn.QueryFirstOrDefault<Setting>(queries.GetById(TblName), id));
    }

    public async Task<BaseSettingVm?> GetByIdAsync(int id)
    {
        using var conn = _ctx.CreateConnection();
        return SettingMapper.Map(
            await conn.QueryFirstOrDefaultAsync<Setting>(queries.GetById(TblName), id)
        );
    }

    public IEnumerable<BaseSettingVm> GetAll()
    {
        using var conn = _ctx.CreateConnection();
        using var reader = conn.ExecuteReader(queries.GetAll(TblName));
        var settings = new List<Setting>();
        var settingBoolParser = reader.GetRowParser<SettingBool>();
        var settingIntParser = reader.GetRowParser<SettingInt>();
        var settingStringParser = reader.GetRowParser<SettingString>();
        while (reader.Read())
        {
            var discriminator = (SettingType)
                reader.GetInt32(reader.GetOrdinal(nameof(SettingType)));
            switch (discriminator)
            {
                case SettingType.SettingBool:
                    settings.Add(settingBoolParser(reader));
                    break;
                case SettingType.SettingString:
                    settings.Add(settingStringParser(reader));
                    break;
                case SettingType.SettingInt:
                    settings.Add(settingIntParser(reader));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return settings.Select(SettingMapper.Map);
    }

    public BaseSettingVm? GetByName(string name)
    {
        using var conn = _ctx.CreateConnection();
        using var reader = conn.ExecuteReader(queries.GetByName(TblName), new { Name = name });
        var settingBoolParser = reader.GetRowParser<SettingBool>();
        var settingIntParser = reader.GetRowParser<SettingInt>();
        var settingStringParser = reader.GetRowParser<SettingString>();
        reader.Read();
        var discriminator = (SettingType)reader.GetInt32(reader.GetOrdinal(nameof(SettingType)));
        return discriminator switch
        {
            SettingType.SettingBool => SettingMapper.Map(settingBoolParser(reader)),
            SettingType.SettingString => SettingMapper.Map(settingStringParser(reader)),
            SettingType.SettingInt => SettingMapper.Map(settingIntParser(reader)),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    public async Task<IEnumerable<BaseSettingVm>> GetAllAsync()
    {
        using var conn = _ctx.CreateConnection();
        return (await conn.QueryAsync<Setting>(queries.GetAll(TblName))).Select(SettingMapper.Map);
    }

    public int Update(BaseSettingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        var ent = SettingMapper.Map(vm);
        return ent switch
        {
            SettingBool t => conn.Execute(
                queries.Update(),
                new
                {
                    ent.Id,
                    ent.Name,
                    t.BoolValue,
                    StringValue = (string?)null,
                    IntValue = (int?)null,
                }
            ),
            SettingString t => conn.Execute(
                queries.Update(),
                new
                {
                    ent.Id,
                    ent.Name,
                    BoolValue = (bool?)null,
                    t.StringValue,
                    IntValue = (int?)null,
                }
            ),
            SettingInt t => conn.Execute(
                queries.Update(),
                new
                {
                    ent.Id,
                    ent.Name,
                    BoolValue = (bool?)null,
                    StringValue = (string?)null,
                    t.IntValue,
                }
            ),
            _ => throw new NotImplementedException(),
        };
    }

    public async Task<int> UpdateAsync(BaseSettingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.ExecuteAsync(queries.Update(), vm);
    }

    public int Delete(BaseSettingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return conn.Execute(queries.DeleteById(TblName), vm.Id);
    }

    public Task<int> DeleteAsync(BaseSettingVm vm)
    {
        using var conn = _ctx.CreateConnection();
        return conn.ExecuteAsync(queries.DeleteById(TblName), vm.Id);
    }
}
