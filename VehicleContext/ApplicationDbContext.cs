using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTypeModel;

namespace VehicleContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Brands> Brands { get; set; }
        public DbSet<VehicleTypes> VehicleTypes { get; set; }
        public DbSet<Models> Models { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region VehicleType Property

            //Setting Vehicle Type Id is Required 
            modelBuilder.Entity<VehicleTypes>()
                 .Property(vehicleType => vehicleType.VehicleTypeId).IsRequired();

            //Setting Vehicle Type Name Limit
            modelBuilder.Entity<VehicleTypes>()
                 .Property(vehicleType => vehicleType.VehicleType).IsRequired().HasMaxLength(50);

            //Setting Vehicle Type Description Limit
            modelBuilder.Entity<VehicleTypes>()
                 .Property(vehicleType => vehicleType.Description).IsRequired().HasMaxLength(100);

            #endregion

            #region VehicleType Key Constraints & Relations
            //Primary Key For Brands
            modelBuilder.Entity<VehicleTypes>().HasKey(vehicleType => vehicleType.VehicleTypeId);


            //Relation between Vehicle Type & Brands
            modelBuilder.Entity<VehicleTypes>()
                        .HasMany(vehicleType => vehicleType.Brands)
                        .WithOne(brand => brand.VehicleType)
                        .HasForeignKey(brand => brand.VehicleTypeId);

            #endregion

            #region Brands Property

            //Setting Brand Id is Required 
            modelBuilder.Entity<Brands>()
                 .Property(brands => brands.BrandId).IsRequired();

            //Setting Brand Name Limit
            modelBuilder.Entity<Brands>()
                 .Property(brands => brands.Brand).IsRequired().HasMaxLength(45);

            //Setting Brand Description Limit
            modelBuilder.Entity<Brands>()
                 .Property(brands => brands.Description).IsRequired().HasMaxLength(100);

            #endregion    

            #region Brands Key Constraints & Relations
            //Primary Key For Brands
            modelBuilder.Entity<Brands>().HasKey(brands => brands.BrandId);


            //Relation between Brands & Vehicle Brands
            modelBuilder.Entity<Brands>()
                        .HasOne(brand => brand.VehicleType)
                        .WithMany(vehicleType => vehicleType.Brands)
                        .HasForeignKey(brand => brand.VehicleTypeId);

            #endregion

            #region Model Property

            //Setting Model Id is Required 
            modelBuilder.Entity<Models>()
                 .Property(model => model.ModelId).IsRequired();

            //Setting Model Name Limit
            modelBuilder.Entity<Models>()
                 .Property(model => model.modelname).IsRequired().HasMaxLength(45);

            //Setting Model Description Limit
            modelBuilder.Entity<Models>()
                 .Property(model => model.Description).IsRequired().HasMaxLength(100);

            #endregion

            #region Model Key Constraints & Relations
            //Primary Key For Model
            modelBuilder.Entity<Models>().HasKey(model => model.ModelId);


            //Relation between Model & Brands
            modelBuilder.Entity<Models>()
                .HasOne(model=>model.Brands)
                .WithMany(brands=>brands.Model)
                .HasForeignKey(model => model.BrandId);

            #endregion
        }
    }
}
