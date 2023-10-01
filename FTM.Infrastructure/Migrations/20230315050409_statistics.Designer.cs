﻿// <auto-generated />
using System;
using FTM.Domain.Models.IssueModel;
using FTM.Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FTM.Infrastructure.Migrations
{
    [DbContext(typeof(FtmDbContext))]
    [Migration("20230315050409_statistics")]
    partial class statistics
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FTM.Domain.Models.BotStatusModel.BotStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EditingField")
                        .HasColumnType("text");

                    b.Property<int?>("EntityId")
                        .HasColumnType("integer");

                    b.Property<IssueFilter>("Filter")
                        .HasColumnType("jsonb");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BotStatuses");
                });

            modelBuilder.Entity("FTM.Domain.Models.IssueModel.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Folder")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RemindTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("RetryDaysCount")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Step[]>("Steps")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("FTM.Domain.Models.SettingsModel.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DailyScheduleHour")
                        .HasColumnType("integer");

                    b.Property<int?>("DefaultRemindHours")
                        .HasColumnType("integer");

                    b.Property<int?>("DefaultRemindMinutes")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDailyScheduleEnabled")
                        .HasColumnType("boolean");

                    b.Property<int?>("Timezone")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("FTM.Domain.Models.StatisticModel.Statistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Counter")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<object>("Data")
                        .HasColumnType("jsonb");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("FTM.Domain.Models.UserModel.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FTM.Domain.Models.BotStatusModel.BotStatus", b =>
                {
                    b.HasOne("FTM.Domain.Models.UserModel.User", "User")
                        .WithMany("BotStatuses")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FTM.Domain.Models.IssueModel.Issue", b =>
                {
                    b.HasOne("FTM.Domain.Models.UserModel.User", "User")
                        .WithMany("Issues")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FTM.Domain.Models.SettingsModel.Settings", b =>
                {
                    b.HasOne("FTM.Domain.Models.UserModel.User", "User")
                        .WithMany("Settings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FTM.Domain.Models.UserModel.User", b =>
                {
                    b.Navigation("BotStatuses");

                    b.Navigation("Issues");

                    b.Navigation("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
