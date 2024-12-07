﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YMouseButtonControl.Infrastructure.Context;

#nullable disable

namespace YMouseButtonControl.Infrastructure.Migrations
{
    [DbContext(typeof(YMouseButtonControlDbContext))]
    [Migration("20241207142135_CorrectDefaultProfileDisplayPriority")]
    partial class CorrectDefaultProfileDisplayPriority
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.ButtonMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AutoRepeatDelay")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("AutoRepeatRandomizeDelayEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("BlockOriginalMouseInput")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ButtonMappingType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("TEXT");

                    b.Property<string>("Keys")
                        .HasColumnType("TEXT");

                    b.Property<int>("MouseButton")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Selected")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SimulatedKeystrokeType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("ButtonMappings");

                    b.HasDiscriminator().HasValue("ButtonMapping");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Checked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DisplayPriority")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MatchType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentClass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Process")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WindowCaption")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WindowClass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Profiles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Checked = true,
                            Description = "Default description",
                            DisplayPriority = 0,
                            IsDefault = true,
                            MatchType = "N/A",
                            Name = "Default",
                            ParentClass = "N/A",
                            Process = "*",
                            WindowCaption = "N/A",
                            WindowClass = "N/A"
                        });
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Settings");

                    b.HasDiscriminator().HasValue("Setting");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Background")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Highlight")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Themes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Background = "SystemAltHighColor",
                            Highlight = "SystemAccentColor",
                            Name = "Default"
                        },
                        new
                        {
                            Id = 2,
                            Background = "White",
                            Highlight = "Yellow",
                            Name = "Light"
                        },
                        new
                        {
                            Id = 3,
                            Background = "Black",
                            Highlight = "#3700b3",
                            Name = "Dark"
                        });
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.DisabledMapping", b =>
                {
                    b.HasBaseType("YMouseButtonControl.Domain.Models.ButtonMapping");

                    b.HasDiscriminator().HasValue("DisabledMapping");
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.NothingMapping", b =>
                {
                    b.HasBaseType("YMouseButtonControl.Domain.Models.ButtonMapping");

                    b.HasDiscriminator().HasValue("NothingMapping");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ButtonMappingType = 0,
                            MouseButton = 0,
                            ProfileId = 1,
                            Selected = false
                        },
                        new
                        {
                            Id = 2,
                            ButtonMappingType = 0,
                            MouseButton = 1,
                            ProfileId = 1,
                            Selected = false
                        },
                        new
                        {
                            Id = 3,
                            ButtonMappingType = 0,
                            MouseButton = 2,
                            ProfileId = 1,
                            Selected = false
                        },
                        new
                        {
                            Id = 4,
                            ButtonMappingType = 0,
                            MouseButton = 3,
                            ProfileId = 1,
                            Selected = false
                        },
                        new
                        {
                            Id = 5,
                            ButtonMappingType = 0,
                            MouseButton = 4,
                            ProfileId = 1,
                            Selected = false
                        },
                        new
                        {
                            Id = 6,
                            ButtonMappingType = 0,
                            MouseButton = 5,
                            ProfileId = 1,
                            Selected = false
                        },
                        new
                        {
                            Id = 7,
                            ButtonMappingType = 0,
                            MouseButton = 6,
                            ProfileId = 1,
                            Selected = false
                        },
                        new
                        {
                            Id = 8,
                            ButtonMappingType = 0,
                            MouseButton = 7,
                            ProfileId = 1,
                            Selected = false
                        },
                        new
                        {
                            Id = 9,
                            ButtonMappingType = 0,
                            MouseButton = 8,
                            ProfileId = 1,
                            Selected = false
                        });
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.RightClick", b =>
                {
                    b.HasBaseType("YMouseButtonControl.Domain.Models.ButtonMapping");

                    b.HasDiscriminator().HasValue("RightClick");
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.SimulatedKeystroke", b =>
                {
                    b.HasBaseType("YMouseButtonControl.Domain.Models.ButtonMapping");

                    b.HasDiscriminator().HasValue("SimulatedKeystroke");
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.SettingBool", b =>
                {
                    b.HasBaseType("YMouseButtonControl.Domain.Models.Setting");

                    b.Property<bool>("BoolValue")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("SettingBool");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "StartMinimized",
                            BoolValue = false
                        });
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.SettingInt", b =>
                {
                    b.HasBaseType("YMouseButtonControl.Domain.Models.Setting");

                    b.Property<int>("IntValue")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("SettingInt");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Name = "Theme",
                            IntValue = 3
                        });
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.SettingString", b =>
                {
                    b.HasBaseType("YMouseButtonControl.Domain.Models.Setting");

                    b.Property<string>("StringValue")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("SettingString");
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.ButtonMapping", b =>
                {
                    b.HasOne("YMouseButtonControl.Domain.Models.Profile", "Profile")
                        .WithMany("ButtonMappings")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("YMouseButtonControl.Domain.Models.Profile", b =>
                {
                    b.Navigation("ButtonMappings");
                });
#pragma warning restore 612, 618
        }
    }
}
