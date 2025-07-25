﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeighborhoodServices.API.Data;

#nullable disable

namespace NeighborhoodServices.Migrations
{
    [DbContext(typeof(NeighborhoodDbContext))]
    [Migration("20250619104528_AddUniqueBookingConstraint")]
    partial class AddUniqueBookingConstraint
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NeighborhoodServices.API.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("ProviderId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("Services");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Pet Care",
                            Description = "Professional dog walking service in your neighborhood",
                            Price = 25.00m,
                            ProviderId = 1,
                            Title = "Dog Walking"
                        },
                        new
                        {
                            Id = 2,
                            Category = "Education",
                            Description = "High school and college level math tutoring",
                            Price = 40.00m,
                            ProviderId = 1,
                            Title = "Math Tutoring"
                        });
                });

            modelBuilder.Entity("NeighborhoodServices.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "john@example.com",
                            Name = "John Provider",
                            Role = "Provider"
                        },
                        new
                        {
                            Id = 2,
                            Email = "jane@example.com",
                            Name = "Jane Customer",
                            Role = "Customer"
                        });
                });

            modelBuilder.Entity("ServiceBooking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ScheduledTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ServiceId", "CustomerId", "ScheduledTime")
                        .IsUnique();

                    b.ToTable("ServiceBookings");
                });

            modelBuilder.Entity("NeighborhoodServices.API.Models.Service", b =>
                {
                    b.HasOne("NeighborhoodServices.API.Models.User", "Provider")
                        .WithMany("ServicesOffered")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("ServiceBooking", b =>
                {
                    b.HasOne("NeighborhoodServices.API.Models.User", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("NeighborhoodServices.API.Models.Service", "Service")
                        .WithMany("Bookings")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("NeighborhoodServices.API.Models.Service", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("NeighborhoodServices.API.Models.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("ServicesOffered");
                });
#pragma warning restore 612, 618
        }
    }
}
