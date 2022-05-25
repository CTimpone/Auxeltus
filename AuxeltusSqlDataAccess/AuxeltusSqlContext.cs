using Microsoft.EntityFrameworkCore;
using System;

namespace Auxeltus.AccessLayer.Sql
{
    public class AuxeltusSqlContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AuxeltusSqlContext(DbContextOptions<AuxeltusSqlContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("Auxeltus_SQLConnectionString"));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasIndex(prop => prop.Name)
                .IsUnique();
            modelBuilder.Entity<Location>()
                .HasIndex(prop => new { prop.Latitude, prop.Longitude })
                .IsUnique();
            modelBuilder.Entity<Location>()
                .HasMany(loc => loc.Jobs)
                .WithOne(job => job.Location!)
                .HasForeignKey(job => job.LocationId!);

            modelBuilder.Entity<Role>()
                .HasMany(rol => rol.Jobs)
                .WithOne(job => job.Role)
                .HasForeignKey(job => job.RoleId);
            modelBuilder.Entity<Role>()
                .HasIndex(prop => prop.Title)
                .IsUnique();

            modelBuilder.Entity<Job>()
                .HasCheckConstraint("CK_EmployeeHasSalary", "([EmployeeId] IS NULL AND [Salary] IS NULL) OR ([EmployeeId] IS NOT NULL AND [Salary] IS NOT NULL)")
                .HasCheckConstraint("CK_EmployeeLocationSanity", "[LocationId] IS NOT NULL OR [Remote] = 1")
                .Property(prop => prop.Archived)
                .HasDefaultValue(false);

            base.OnModelCreating(modelBuilder);
        }

    }
}