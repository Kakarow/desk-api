﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using test_app.Data;

#nullable disable

namespace test_app.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20220528221921_DeskUserDeskIdUSerId")]
    partial class DeskUserDeskIdUSerId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("test_app.Models.Desk", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<int>("locationid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("locationid");

                    b.ToTable("desks");
                });

            modelBuilder.Entity("test_app.Models.Location", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("locations");
                });

            modelBuilder.Entity("test_app.Models.Reservation", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("days")
                        .HasColumnType("INTEGER");

                    b.Property<int>("deskId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("reservationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("userId")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("deskId");

                    b.HasIndex("userId");

                    b.ToTable("reservations");
                });

            modelBuilder.Entity("test_app.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("passwordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("test_app.Models.Desk", b =>
                {
                    b.HasOne("test_app.Models.Location", "location")
                        .WithMany()
                        .HasForeignKey("locationid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("location");
                });

            modelBuilder.Entity("test_app.Models.Reservation", b =>
                {
                    b.HasOne("test_app.Models.Desk", "desk")
                        .WithMany()
                        .HasForeignKey("deskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("test_app.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("desk");

                    b.Navigation("user");
                });
#pragma warning restore 612, 618
        }
    }
}
