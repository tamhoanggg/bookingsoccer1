﻿// <auto-generated />
using System;
using BookingSoccers.Repo.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookingSoccers.Repo.Migrations
{
    [DbContext(typeof(BookingSoccersContext))]
    partial class BookingSoccersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BookingSoccers.Repo.Entities.BookingInfo.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FieldId")
                        .HasColumnType("integer");

                    b.Property<byte>("Rating")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("integer");

                    b.Property<int>("ZoneId")
                        .HasColumnType("integer");

                    b.Property<byte>("ZoneTypeId")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("FieldId");

                    b.HasIndex("ZoneId");

                    b.HasIndex("ZoneTypeId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.BookingInfo.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<int>("BookingId")
                        .HasColumnType("integer");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("integer");

                    b.Property<int>("ReceiverInfoId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("ReceiverInfoId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.ImageFolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FieldId")
                        .HasColumnType("integer");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.ToTable("ImageFolders");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.PriceItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("interval");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("PriceMenuId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("interval");

                    b.Property<byte>("TimeAmount")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("PriceMenuId");

                    b.ToTable("PriceItems");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.PriceMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DayType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FieldId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<byte>("ZoneTypeId")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("ZoneTypeId");

                    b.ToTable("PriceMenus");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.SoccerField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("BaseTimeInterval")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("CloseHour")
                        .HasColumnType("interval");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FieldName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("OpenHour")
                        .HasColumnType("interval");

                    b.Property<int>("ReviewScoreSum")
                        .HasColumnType("integer");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<int>("TotalReviews")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("SoccerFields");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.Zone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Area")
                        .HasColumnType("integer");

                    b.Property<int>("FieldId")
                        .HasColumnType("integer");

                    b.Property<int>("Length")
                        .HasColumnType("integer");

                    b.Property<byte>("Number")
                        .HasColumnType("smallint");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.Property<byte>("ZoneTypeId")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("ZoneTypeId");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.ZoneSlot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<int>("ZoneId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ZoneId");

                    b.ToTable("ZoneSlots");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.ZoneType", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<byte>("Id"));

                    b.Property<byte>("DepositPercent")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.HasKey("Id");

                    b.ToTable("ZoneTypes");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.UserInfo.Role", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<byte>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.UserInfo.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<byte>("RoleId")
                        .HasColumnType("smallint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.BookingInfo.Booking", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.UserInfo.User", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.SoccerField", "FieldInfo")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.Zone", "ZoneInfo")
                        .WithMany()
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.ZoneType", "TypeZone")
                        .WithMany()
                        .HasForeignKey("ZoneTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("FieldInfo");

                    b.Navigation("TypeZone");

                    b.Navigation("ZoneInfo");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.BookingInfo.Payment", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.BookingInfo.Booking", "BookingInfo")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingSoccers.Repo.Entities.UserInfo.User", "ReceiverInfo")
                        .WithMany()
                        .HasForeignKey("ReceiverInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookingInfo");

                    b.Navigation("ReceiverInfo");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.ImageFolder", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.SoccerField", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.PriceItem", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.PriceMenu", "Menu")
                        .WithMany()
                        .HasForeignKey("PriceMenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.PriceMenu", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.SoccerField", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.ZoneType", "TypeOfZone")
                        .WithMany()
                        .HasForeignKey("ZoneTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("TypeOfZone");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.SoccerField", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.UserInfo.User", "user")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.Zone", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.SoccerField", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.ZoneType", "ZoneCate")
                        .WithMany()
                        .HasForeignKey("ZoneTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("ZoneCate");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.SoccerFieldInfo.ZoneSlot", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.SoccerFieldInfo.Zone", "FieldZone")
                        .WithMany()
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FieldZone");
                });

            modelBuilder.Entity("BookingSoccers.Repo.Entities.UserInfo.User", b =>
                {
                    b.HasOne("BookingSoccers.Repo.Entities.UserInfo.Role", "role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("role");
                });
#pragma warning restore 612, 618
        }
    }
}
