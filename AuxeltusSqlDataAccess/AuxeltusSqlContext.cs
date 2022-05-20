using Microsoft.EntityFrameworkCore;
using System;

namespace Auxeltus.AccessLayer.Sql
{
    public class AuxeltusSqlContext : DbContext
    {
        public AuxeltusSqlContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("Auxeltus_SQLConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasMany(loc => loc.Jobs)
                .WithOne(job => job.Location)
                .HasForeignKey(job => job.LocationId);

            modelBuilder.Entity<Role>()
                .HasMany(rol => rol.Jobs)
                .WithOne(job => job.Role)
                .HasForeignKey(job => job.RoleId);
        }

    }
}