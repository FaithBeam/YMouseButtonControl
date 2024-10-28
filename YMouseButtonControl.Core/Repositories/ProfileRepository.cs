using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YMouseButtonControl.Core.Mappings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Context;
using YMouseButtonControl.DataAccess.Models;
using YMouseButtonControl.DataAccess.Queries;

namespace YMouseButtonControl.Core.Repositories;

public class ProfileRepository(
    IConnectionProvider connectionProvider,
    ProfileQueries queries,
    ButtonMappingQueries btnMappingQueries
) : IRepository<Profile, ProfileVm>
{
    private const string TblName = "Profiles";

    public int Add(ProfileVm vm)
    {
        using var conn = connectionProvider.CreateConnection();
        return conn.Query<int>(queries.Add(), vm).Single();
    }

    public async Task<int> AddAsync(ProfileVm vm)
    {
        using var conn = connectionProvider.CreateConnection();
        return await conn.ExecuteAsync(queries.Add(), vm);
    }

    public ProfileVm? GetById(int id)
    {
        using var conn = connectionProvider.CreateConnection();
        var profile = conn.QueryFirstOrDefault<Profile>(queries.GetById(TblName), new { Id = id });
        if (profile == null)
        {
            return null;
        }
        profile.ButtonMappings = GetButtonMappingsForProfileId(id);
        return ProfileMapper.Map(profile);
    }

    public async Task<ProfileVm?> GetByIdAsync(int id)
    {
        using var conn = connectionProvider.CreateConnection();
        return ProfileMapper.Map(
            await conn.QueryFirstOrDefaultAsync<Profile>(queries.GetById(TblName), id)
        );
    }

    public IEnumerable<ProfileVm> GetAll()
    {
        using var conn = connectionProvider.CreateConnection();
        return conn.Query<Profile>(queries.GetAll(TblName))
            .Select(p =>
            {
                p.ButtonMappings = GetButtonMappingsForProfileId(p.Id);
                return ProfileMapper.Map(p);
            });
    }

    public async Task<IEnumerable<ProfileVm>> GetAllAsync()
    {
        using var conn = connectionProvider.CreateConnection();
        return (await conn.QueryAsync<Profile>(queries.GetAll(TblName))).Select(ProfileMapper.Map);
    }

    public int Update(ProfileVm vm)
    {
        using var conn = connectionProvider.CreateConnection();
        return conn.Execute(queries.Update(), vm);
    }

    public async Task<int> UpdateAsync(ProfileVm vm)
    {
        using var conn = connectionProvider.CreateConnection();
        return await conn.ExecuteAsync(queries.Update(), vm);
    }

    public int Delete(ProfileVm vm)
    {
        using var conn = connectionProvider.CreateConnection();
        return conn.Execute(queries.DeleteById(TblName), vm);
    }

    public Task<int> DeleteAsync(ProfileVm vm)
    {
        using var conn = connectionProvider.CreateConnection();
        return conn.ExecuteAsync(queries.DeleteById(TblName), vm);
    }

    private List<ButtonMapping> GetButtonMappingsForProfileId(int id)
    {
        using var conn = connectionProvider.CreateConnection();
        using var reader = conn.ExecuteReader(btnMappingQueries.GetByProfileId(), new { Id = id });
        var nothingParser = reader.GetRowParser<NothingMapping>();
        var disabledParser = reader.GetRowParser<DisabledMapping>();
        var simulatedKeystrokeParser = reader.GetRowParser<SimulatedKeystroke>();
        var rightClickParser = reader.GetRowParser<RightClick>();

        var buttonMappings = new List<ButtonMapping>();

        while (reader.Read())
        {
            var discriminator = (ButtonMappingType)
                reader.GetInt32(reader.GetOrdinal("ButtonMappingType"));
            switch (discriminator)
            {
                case ButtonMappingType.Disabled:
                    buttonMappings.Add(disabledParser(reader));
                    break;
                case ButtonMappingType.Nothing:
                    buttonMappings.Add(nothingParser(reader));
                    break;
                case ButtonMappingType.SimulatedKeystroke:
                    buttonMappings.Add(simulatedKeystrokeParser(reader));
                    break;
                case ButtonMappingType.RightClick:
                    buttonMappings.Add(rightClickParser(reader));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return buttonMappings;
    }

    public ProfileVm? GetByName(string name)
    {
        using var conn = connectionProvider.CreateConnection();
        return ProfileMapper.Map(
            conn.QuerySingleOrDefault<Profile>(queries.GetByName(TblName), name)
        );
    }
}
