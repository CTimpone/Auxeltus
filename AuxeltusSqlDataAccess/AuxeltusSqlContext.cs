using Microsoft.EntityFrameworkCore;
using System;

namespace Auxeltus.AccessLayer.Sql
{
    public class AuxeltusSqlContext : DbContext
    {
        public DbSet<JobEntity> Jobs { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }

        public AuxeltusSqlContext()
        { 
        
        }

        public AuxeltusSqlContext(DbContextOptions<AuxeltusSqlContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("Auxeltus_SQLConnectionString"));

                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationEntity>()
                .HasIndex(prop => prop.Name)
                .IsUnique();
            modelBuilder.Entity<LocationEntity>()
                .HasIndex(prop => new { prop.Latitude, prop.Longitude })
                .IsUnique();
            modelBuilder.Entity<LocationEntity>()
                .HasMany(loc => loc.Jobs)
                .WithOne(job => job.Location!)
                .HasForeignKey(job => job.LocationId!);

            modelBuilder.Entity<RoleEntity>()
                .HasMany(rol => rol.Jobs)
                .WithOne(job => job.Role)
                .HasForeignKey(job => job.RoleId);
            modelBuilder.Entity<RoleEntity>()
                .HasIndex(prop => prop.Title)
                .IsUnique();

            modelBuilder.Entity<JobEntity>()
                .HasCheckConstraint("CK_EmployeeHasSalary", "([EmployeeId] IS NULL AND [Salary] IS NULL) OR ([EmployeeId] IS NOT NULL AND [Salary] IS NOT NULL)")
                .HasCheckConstraint("CK_EmployeeLocationSanity", "([LocationId] IS NOT NULL AND [Remote] = 0) OR ([Remote] = 1 AND [LocationId] IS NULL)")
                .Property(prop => prop.Archived)
                .HasDefaultValue(false);

            base.OnModelCreating(modelBuilder);
        }

    }
}