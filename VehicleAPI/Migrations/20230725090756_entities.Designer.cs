﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VehicleContext;

#nullable disable

namespace VehicleAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230725090756_entities")]
    partial class entities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("VehicleTypeModel.Brands", b =>
                {
                    b.Property<Guid>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<Guid>("VehicleTypeId")
                        .HasColumnType("char(36)");

                    b.HasKey("BrandId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("VehicleTypeModel.VehicleType", b =>
                {
                    b.Property<Guid>("VehicleTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("VehicleTypeName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("VehicleTypeId");

                    b.ToTable("VehicleTypes");
                });

            modelBuilder.Entity("VehicleTypeModel.Brands", b =>
                {
                    b.HasOne("VehicleTypeModel.VehicleType", "VehicleType")
                        .WithMany("Brands")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("VehicleTypeModel.VehicleType", b =>
                {
                    b.Navigation("Brands");
                });
#pragma warning restore 612, 618
        }
    }
}