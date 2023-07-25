using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTypeModel;

namespace VehicleContext
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
                
        }

        public DbSet<Brands> Brands { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region VehicleType Property

            //Setting Vehicle Type Id is Required 
            modelBuilder.Entity<VehicleType>()
                 .Property(vehicleType => vehicleType.VehicleTypeId).IsRequired();

            //Setting Vehicle Type Name Limit
            modelBuilder.Entity<VehicleType>()
                 .Property(vehicleType => vehicleType.VehicleTypeName).IsRequired().HasMaxLength(255);

            //Setting Vehicle Type Description Limit
            modelBuilder.Entity<VehicleType>()
                 .Property(vehicleType => vehicleType.Description).IsRequired().HasMaxLength(500);

            #endregion

            #region VehicleType Key Constraints & Relations
            //Primary Key For Brands
            modelBuilder.Entity<VehicleType>().HasKey(vehicleType => vehicleType.VehicleTypeId);


            //Relation between Vehicle Type & Brands
            modelBuilder.Entity<VehicleType>()
                        .HasMany(vehicleType => vehicleType.Brands)
                        .WithOne(brand => brand.VehicleType)
                        .HasForeignKey(brand => brand.VehicleTypeId);

            #endregion

            #region Brands Property

            //Setting Vehicle Type Id is Required 
            modelBuilder.Entity<Brands>()
                 .Property(brands => brands.BrandId).IsRequired();

            //Setting Vehicle Type Name Limit
            modelBuilder.Entity<Brands>()
                 .Property(brands => brands.BrandName).IsRequired().HasMaxLength(255);

            //Setting Vehicle Type Description Limit
            modelBuilder.Entity<Brands>()
                 .Property(brands => brands.Description).IsRequired().HasMaxLength(500);

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


        }
    }
}
