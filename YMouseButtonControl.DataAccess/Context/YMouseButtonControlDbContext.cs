using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Infrastructure.Context;

public class YMouseButtonControlDbContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<SettingBool> SettingBools { get; set; } = null!;
    public DbSet<SettingString> SettingStrings { get; set; } = null!;
    public DbSet<SettingInt> SettingInts { get; set; } = null!;
    public DbSet<Theme> Themes { get; set; } = null!;
    public DbSet<ButtonMapping> ButtonMappings { get; set; } = null!;
    public DbSet<DisabledMapping> DisabledMappings { get; set; } = null!;
    public DbSet<NothingMapping> NothingMappings { get; set; } = null!;
    public DbSet<SimulatedKeystroke> SimulatedKeystrokeMappings { get; set; } = null!;
    public DbSet<RightClick> RightClickMappings { get; set; } = null!;

    public YMouseButtonControlDbContext(DbContextOptions<YMouseButtonControlDbContext> opts)
        : base(opts)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Profile>()
            .HasData(
                new Profile
                {
                    Id = 1,
                    IsDefault = true,
                    Checked = true,
                    DisplayPriority = 0,
                    Name = "Default",
                    Description = "Default description",
                    WindowCaption = "N/A",
                    Process = "*",
                    MatchType = "N/A",
                    ParentClass = "N/A",
                    WindowClass = "N/A",
                }
            );

        modelBuilder
            .Entity<NothingMapping>()
            .HasData(
                [
                    new NothingMapping
                    {
                        Id = 1,
                        MouseButton = MouseButton.Mb1,
                        ProfileId = 1,
                    },
                    new NothingMapping
                    {
                        Id = 2,
                        MouseButton = MouseButton.Mb2,
                        ProfileId = 1,
                    },
                    new NothingMapping
                    {
                        Id = 3,
                        MouseButton = MouseButton.Mb3,
                        ProfileId = 1,
                    },
                    new NothingMapping
                    {
                        Id = 4,
                        MouseButton = MouseButton.Mb4,
                        ProfileId = 1,
                    },
                    new NothingMapping
                    {
                        Id = 5,
                        MouseButton = MouseButton.Mb5,
                        ProfileId = 1,
                    },
                    new NothingMapping
                    {
                        Id = 6,
                        MouseButton = MouseButton.Mwu,
                        ProfileId = 1,
                    },
                    new NothingMapping
                    {
                        Id = 7,
                        MouseButton = MouseButton.Mwd,
                        ProfileId = 1,
                    },
                    new NothingMapping
                    {
                        Id = 8,
                        MouseButton = MouseButton.Mwl,
                        ProfileId = 1,
                    },
                    new NothingMapping
                    {
                        Id = 9,
                        MouseButton = MouseButton.Mwr,
                        ProfileId = 1,
                    },
                ]
            );

        modelBuilder
            .Entity<Theme>()
            .HasData(
                new Theme
                {
                    Id = 1,
                    Name = "Default",
                    Background = "SystemAltHighColor",
                    Highlight = "SystemAccentColor",
                },
                new Theme
                {
                    Id = 2,
                    Name = "Light",
                    Background = "White",
                    Highlight = "Yellow",
                },
                new Theme
                {
                    Id = 3,
                    Name = "Dark",
                    Background = "Black",
                    Highlight = "#3700b3",
                }
            );

        modelBuilder
            .Entity<SettingBool>()
            .HasData(
                new SettingBool
                {
                    Id = 1,
                    Name = "StartMinimized",
                    BoolValue = false,
                }
            );
        modelBuilder
            .Entity<SettingInt>()
            .HasData(
                new SettingInt
                {
                    Id = 2,
                    Name = "Theme",
                    IntValue = 3,
                }
            );
    }
}
