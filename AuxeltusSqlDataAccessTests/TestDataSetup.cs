﻿using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AuxeltusSqlDataAccessTests
{
    internal static class TestDataSetup
    {
        public static List<Location> locations;
        public static List<Role> roles;
        public static List<Job> jobs;

        public static AuxeltusSqlContext GenerateTestDataContext()
        {
            DbContextOptions<AuxeltusSqlContext> options = new DbContextOptionsBuilder<AuxeltusSqlContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            AuxeltusSqlContext context = new AuxeltusSqlContext(options);

            AddLocationsToContext(context);
            AddRolesToContext(context);
            AddJobsToContext(context);

            return context;
        }

        private static void AddLocationsToContext(AuxeltusSqlContext context)
        {
            locations = new List<Location>
            {
                new Location
                {
                    Name = "Columbus, Ohio",
                    Latitude = 40.03261910026816,
                    Longitude = -83.03349425205028
                },
                new Location
                {
                    Name = "Evanston, Illinois",
                    Latitude = 42.05796965571029,
                    Longitude = -87.67593067172223
                },
                new Location
                {
                    Name = "Chennai, India",
                    Latitude = 13.07149818062577,
                    Longitude = 80.25647616462626
                },
                new Location
                {
                    Name = "North Vegas",
                    Latitude = 36.20616312689968,
                    Longitude = -115.11229175325266
                },
                new Location
                {
                    Name = "Antarctica",
                    Latitude = -82,
                    Longitude = 17
                }
            };
            context.Locations.AddRange(locations);
            context.SaveChanges();

            locations = context.Locations.AsNoTracking().ToListAsync().GetAwaiter().GetResult();
        }

        private static void AddRolesToContext(AuxeltusSqlContext context)
        {
            roles = new List<Role>
            {
                new Role
                {
                    Title = "Software Developer",
                    Tier = 2,
                    MinimumSalary = 75000,
                    MaximumSalary = 120000
                },
                new Role
                {
                    Title = "Senior Software Developer",
                    Tier = 3,
                    MinimumSalary = 105000,
                    MaximumSalary = 150000
                },
                new Role
                {
                    Title = "Super Senior Software Developer",
                    Tier = 4,
                    MinimumSalary = 137293,
                    MaximumSalary = 215247
                },
                new Role
                {
                    Title = "Custodian",
                    Tier = 2,
                    MinimumSalary = 85000,
                    MaximumSalary = 85000
                },
                new Role
                {
                    Title = "Junior Software Developer",
                    Tier = 1,
                    MinimumSalary = 60000,
                    MaximumSalary = 80000
                },
                new Role
                {
                    Title = "Manager",
                    Tier = 3,
                    MinimumSalary = 95000,
                    MaximumSalary = 135000
                },
                new Role
                {
                    Title = "Super Manager Deluxe with Cheese",
                    Tier = 4,
                    MinimumSalary = 127500,
                    MaximumSalary = 152500
                },
                new Role
                {
                    Title = "Real Big Executive",
                    Tier = 5,
                    MinimumSalary = 300000,
                    MaximumSalary = 500000
                }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();

            roles = context.Roles.AsNoTracking().ToListAsync().GetAwaiter().GetResult();
        }

        private static void AddJobsToContext(AuxeltusSqlContext context)
        {
            jobs = new List<Job>
            {
                new Job
                {
                    Description = "A job without peer!",
                    EmployeeId = 1,
                    ReportingEmployeeId = 2,
                    EmployeeType = EmployeeType.FullTime,
                    Remote = false,
                    Salary = 85000,
                    SalaryType = SalaryType.Annual,
                    Archived = false,
                    LocationId = locations.First().Id,
                    RoleId = roles[3].Id
                },
                new Job
                {
                    Description = "A job in search of an employee!",
                    EmployeeId = null,
                    ReportingEmployeeId = 3,
                    EmployeeType = EmployeeType.FullTime,
                    Remote = false,
                    Salary = null,
                    Archived = false,
                    LocationId = locations.Last().Id,
                    RoleId = roles.First().Id
                },
                new Job
                {
                    Description = "The elusive contractor!",
                    EmployeeId = 4,
                    ReportingEmployeeId = 3,
                    EmployeeType = EmployeeType.Contractor,
                    Remote = false,
                    Salary = 84000,
                    SalaryType = SalaryType.Annual,
                    Archived = false,
                    LocationId = locations.Last().Id,
                    RoleId = roles[1].Id
                },
                new Job
                {
                    Description = "You can never escape management.",
                    EmployeeId = 3,
                    ReportingEmployeeId = 5,
                    EmployeeType = EmployeeType.FullTime,
                    Remote = false,
                    Salary = 100000,
                    SalaryType = SalaryType.Annual,
                    Archived = false,
                    LocationId = locations.Last().Id,
                    RoleId = roles[5].Id
                },
                new Job
                {
                    Description = "The true management starts here.",
                    EmployeeId = 5,
                    ReportingEmployeeId = 6,
                    EmployeeType = EmployeeType.FullTime,
                    Remote = false,
                    Salary = 130000,
                    SalaryType = SalaryType.Annual,
                    Archived = false,
                    LocationId = locations.Last().Id,
                    RoleId = roles[5].Id
                },
                new Job
                {
                    Description = "Grand poo-bah.",
                    EmployeeId = 6,
                    ReportingEmployeeId = 6,
                    EmployeeType = EmployeeType.FullTime,
                    SalaryType = SalaryType.Annual,
                    Remote = false,
                    Salary = 400000,
                    Archived = false,
                    LocationId = locations.Last().Id,
                    RoleId = roles[5].Id
                },
                new Job
                {
                    Description = "An hour is a unit of time conventionally reckoned as 1/24 of a day and scientifically reckoned as 3,599-3,601 seconds, depending on conditions (thanks Wikipedia).",
                    EmployeeId = 7,
                    ReportingEmployeeId = 5,
                    EmployeeType = EmployeeType.Hourly,
                    SalaryType = SalaryType.Hourly,
                    Remote = false,
                    Salary = 95,
                    Archived = false,
                    LocationId = locations.Last().Id,
                    RoleId = roles[2].Id
                }
            };

            context.Jobs.AddRange(jobs);
            context.SaveChanges();

            jobs = context.Jobs.AsNoTracking().ToListAsync().GetAwaiter().GetResult();

        }

    }
}
