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
        public DbSet<VehicleTypes> VehicleTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the primary key
            modelBuilder.Entity<VehicleTypes>().HasKey(v => v.VehicleTypeId);

            // Configure the unique constraint
            modelBuilder.Entity<VehicleTypes>().HasIndex(v => v.VehicleTypeId).IsUnique();

            // Configure the column properties
            modelBuilder.Entity<VehicleTypes>().Property(v => v.VehicleType).HasMaxLength(50);
            modelBuilder.Entity<VehicleTypes>().Property(v => v.Description).HasMaxLength(100);
            modelBuilder.Entity<VehicleTypes>().Property(v => v.IsActive);

            // Configure the table name
            modelBuilder.Entity<VehicleTypes>().ToTable("vehicletype");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Brands>().HasKey(b => b.BrandId);
            modelBuilder.Entity<Brands>().Property(b => b.Brand).HasMaxLength(45);
            modelBuilder.Entity<Brands>().Property(b => b.Description).HasMaxLength(100);
            modelBuilder.Entity<Brands>().Property(b => b.SortOrder);
            modelBuilder.Entity<Brands>().Property(b => b.IsActive);

            // Configure the foreign key relationship
            modelBuilder.Entity<Brands>()
            .HasOne(b => b.VehicleType) // Brand has one VehicleType
            .WithMany(v => v.Brands) // VehicleType has many Brands
            .HasForeignKey(b => b.VehicleTypeId) // Foreign key property in Brand entity
            .OnDelete(DeleteBehavior.Restrict); // Optional: Specify the delete behavior


            // Configure the table name
            modelBuilder.Entity<Brands>().ToTable("brand");

            base.OnModelCreating(modelBuilder);


        }
    }
}
