﻿// <auto-generated />
using System;
using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Auxeltus.AccessLayer.Sql.Migrations
{
    [DbContext(typeof(AuxeltusSqlContext))]
    partial class AuxeltusSqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Auxeltus.AccessLayer.Sql.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Archived")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeType")
                        .HasColumnType("int");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<bool?>("Remote")
                        .HasColumnType("bit");

                    b.Property<int?>("ReportingEmployeeId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<double?>("Salary")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("RoleId");

                    b.ToTable("Jobs");

                    b.HasCheckConstraint("CK_EmployeeHasSalary", "([EmployeeId] IS NULL AND [Salary] IS NULL) OR ([EmployeeId] IS NOT NULL AND [Salary] IS NOT NULL)");

                    b.HasCheckConstraint("CK_EmployeeLocationSanity", "[LocationId] IS NOT NULL OR [Remote] = 1");
                });

            modelBuilder.Entity("Auxeltus.AccessLayer.Sql.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Latitude")
                        .IsRequired()
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .IsRequired()
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Latitude", "Longitude")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Auxeltus.AccessLayer.Sql.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MaximumSalary")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("MinimumSalary")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("Tier")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Auxeltus.AccessLayer.Sql.Job", b =>
                {
                    b.HasOne("Auxeltus.AccessLayer.Sql.Location", "Location")
                        .WithMany("Jobs")
                        .HasForeignKey("LocationId");

                    b.HasOne("Auxeltus.AccessLayer.Sql.Role", "Role")
                        .WithMany("Jobs")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Auxeltus.AccessLayer.Sql.Location", b =>
                {
                    b.Navigation("Jobs");
                });

            modelBuilder.Entity("Auxeltus.AccessLayer.Sql.Role", b =>
                {
                    b.Navigation("Jobs");
                });
#pragma warning restore 612, 618
        }
    }
}
