﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.DataAccess.Context;

public class YMouseButtonControlDbContext(DbContextOptions<YMouseButtonControlDbContext> options)
    : DbContext(options)
{
    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingBool> SettingBools { get; set; }
    public DbSet<SettingString> SettingStrings { get; set; }
    public DbSet<SettingInt> SettingInts { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ButtonMapping> ButtonMappings { get; set; }
    public DbSet<DisabledMapping> DisabledMappings { get; set; }
    public DbSet<NothingMapping> NothingMappings { get; set; }
    public DbSet<SimulatedKeystroke> SimulatedKeystrokes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>();
        modelBuilder.Entity<SettingBool>();
        modelBuilder.Entity<SettingString>();
        modelBuilder.Entity<SettingInt>();

        modelBuilder.Entity<Profile>();
        modelBuilder.Entity<ButtonMapping>();
        modelBuilder.Entity<DisabledMapping>();
        modelBuilder.Entity<NothingMapping>();
        modelBuilder.Entity<RightClick>();
        modelBuilder.Entity<SimulatedKeystroke>();

        modelBuilder
            .Entity<NothingMapping>()
            .HasData(
                new NothingMapping
                {
                    Id = 1,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mb1,
                },
                new NothingMapping
                {
                    Id = 2,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mb2,
                },
                new NothingMapping
                {
                    Id = 3,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mb3,
                },
                new NothingMapping
                {
                    Id = 4,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mb4,
                },
                new NothingMapping
                {
                    Id = 5,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mb5,
                },
                new NothingMapping
                {
                    Id = 6,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mwu,
                },
                new NothingMapping
                {
                    Id = 7,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mwd,
                },
                new NothingMapping
                {
                    Id = 8,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mwl,
                },
                new NothingMapping
                {
                    Id = 9,
                    ProfileId = 1,
                    MouseButton = MouseButton.Mwr,
                }
            );
        modelBuilder
            .Entity<Profile>()
            .HasData(
                new Profile
                {
                    Id = 1,
                    Checked = true,
                    Name = "Default",
                    IsDefault = true,
                    Description = "Default description",
                    Process = "*",
                    MatchType = "N/A",
                    ParentClass = "N/A",
                    WindowCaption = "N/A",
                    WindowClass = "N/A",
                }
            );
        modelBuilder
            .Entity<SettingBool>()
            .HasData(
                new SettingBool
                {
                    Id = 1,
                    Name = "StartMinimized",
                    Value = false,
                }
            );
        modelBuilder
            .Entity<SettingInt>()
            .HasData(
                new SettingInt
                {
                    Id = 2,
                    Name = "Theme",
                    Value = (int)ThemeEnum.Default,
                }
            );
    }
}

public class YMouseButtonControlDbContextFactory
    : IDesignTimeDbContextFactory<YMouseButtonControlDbContext>
{
    public YMouseButtonControlDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<YMouseButtonControlDbContext>();
        optionsBuilder.UseSqlite(
            "Data Source=YMouseButtonControlContext-71b0de61-4c45-4680-b3d3-c603b1260ede.db",
            sqliteOptionsAction: so => so.MigrationsAssembly("YMouseButtonControl.DataAccess")
        );

        return new YMouseButtonControlDbContext(optionsBuilder.Options);
    }
}
