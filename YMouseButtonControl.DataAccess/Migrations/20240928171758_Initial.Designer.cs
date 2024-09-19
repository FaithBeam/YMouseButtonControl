﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YMouseButtonControl.DataAccess.Context;

#nullable disable

namespace YMouseButtonControl.DataAccess.Migrations
{
    [DbContext(typeof(YMouseButtonControlDbContext))]
    [Migration("20240928171758_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.ButtonMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("BlockOriginalMouseInput")
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

                    b.Property<int?>("SimulatedKeystrokeType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("ButtonMappings");

                    b.HasDiscriminator().HasValue("ButtonMapping");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.Profile", b =>
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

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.Setting", b =>
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

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.DisabledMapping", b =>
                {
                    b.HasBaseType("YMouseButtonControl.DataAccess.Models.ButtonMapping");

                    b.HasDiscriminator().HasValue("DisabledMapping");
                });

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.NothingMapping", b =>
                {
                    b.HasBaseType("YMouseButtonControl.DataAccess.Models.ButtonMapping");

                    b.HasDiscriminator().HasValue("NothingMapping");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BlockOriginalMouseInput = false,
                            MouseButton = 0,
                            ProfileId = 1
                        },
                        new
                        {
                            Id = 2,
                            BlockOriginalMouseInput = false,
                            MouseButton = 1,
                            ProfileId = 1
                        },
                        new
                        {
                            Id = 3,
                            BlockOriginalMouseInput = false,
                            MouseButton = 2,
                            ProfileId = 1
                        },
                        new
                        {
                            Id = 5,
                            BlockOriginalMouseInput = false,
                            MouseButton = 4,
                            ProfileId = 1
                        },
                        new
                        {
                            Id = 6,
                            BlockOriginalMouseInput = false,
                            MouseButton = 5,
                            ProfileId = 1
                        },
                        new
                        {
                            Id = 7,
                            BlockOriginalMouseInput = false,
                            MouseButton = 6,
                            ProfileId = 1
                        },
                        new
                        {
                            Id = 8,
                            BlockOriginalMouseInput = false,
                            MouseButton = 7,
                            ProfileId = 1
                        },
                        new
                        {
                            Id = 9,
                            BlockOriginalMouseInput = false,
                            MouseButton = 8,
                            ProfileId = 1
                        });
                });

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.RightClick", b =>
                {
                    b.HasBaseType("YMouseButtonControl.DataAccess.Models.ButtonMapping");

                    b.HasDiscriminator().HasValue("RightClick");
                });

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.SimulatedKeystroke", b =>
                {
                    b.HasBaseType("YMouseButtonControl.DataAccess.Models.ButtonMapping");

                    b.HasDiscriminator().HasValue("SimulatedKeystroke");

                    b.HasData(
                        new
                        {
                            Id = 4,
                            BlockOriginalMouseInput = true,
                            Keys = "ABC123",
                            MouseButton = 3,
                            ProfileId = 1,
                            SimulatedKeystrokeType = 7
                        });
                });

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.SettingBool", b =>
                {
                    b.HasBaseType("YMouseButtonControl.DataAccess.Models.Setting");

                    b.Property<bool>("Value")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("SettingBool");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "StartMinimized",
                            Value = false
                        });
                });

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.SettingString", b =>
                {
                    b.HasBaseType("YMouseButtonControl.DataAccess.Models.Setting");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.ToTable("Settings", t =>
                        {
                            t.Property("Value")
                                .HasColumnName("SettingString_Value");
                        });

                    b.HasDiscriminator().HasValue("SettingString");
                });

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.ButtonMapping", b =>
                {
                    b.HasOne("YMouseButtonControl.DataAccess.Models.Profile", "Profile")
                        .WithMany("ButtonMappings")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("YMouseButtonControl.DataAccess.Models.Profile", b =>
                {
                    b.Navigation("ButtonMappings");
                });
#pragma warning restore 612, 618
        }
    }
}
