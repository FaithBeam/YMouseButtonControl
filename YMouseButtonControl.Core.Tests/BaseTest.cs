using AutoFixture;
using AutoFixture.Kernel;
using YMouseButtonControl.Core.DataAccess.Configuration;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.Core.Profiles.Implementations;
using YMouseButtonControl.DataAccess.LiteDb;

namespace YMouseButtonControl.Core.Tests;

public abstract class BaseTest
{
    private const string ConnStr = "tmp.db";

    [SetUp]
    public void Setup()
    {
        if (File.Exists(ConnStr))
        {
            File.Delete(ConnStr);
        }
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(ConnStr))
        {
            File.Delete(ConnStr);
        }
    }

    public static ProfilesService GetProfilesService()
    {
        return GetProfilesService(Array.Empty<Profile>());
    }

    /// <summary>
    /// This method adds seed data to the database before it is read
    /// </summary>
    /// <param name="seedData"></param>
    /// <returns></returns>
    public static ProfilesService GetProfilesService(IEnumerable<Profile>? seedData)
    {
        var dbConfig = new DatabaseConfiguration { ConnectionString = $"Filename={ConnStr}" };
        var uowF = new LiteDbUnitOfWorkFactory(dbConfig);

        if (seedData is null)
            return new ProfilesService(uowF);

        var uow = uowF.Create();
        var repo = uow.GetRepository<Profile>();
        repo.ApplyAction(seedData);
        uow.Dispose();
        return new ProfilesService(uowF);
    }

    public static List<Profile> GetSeedData(int count = 10)
    {
        var fixture = new Fixture();
        fixture.Customize<Profile>(c => c.With(p => p.IsDefault, false));
        fixture.Customizations.Add(
            new TypeRelay(
                typeof(DataAccess.Models.Interfaces.ISimulatedKeystrokesType),
                typeof(StickyHoldActionType)
            )
        );
        fixture.Customizations.Add(
            new TypeRelay(
                typeof(DataAccess.Models.Interfaces.IButtonMapping),
                typeof(NothingMapping)
            )
        );
        return fixture.CreateMany<Profile>(count).ToList()
            ?? throw new Exception("Error creating test profiles");
    }
}
