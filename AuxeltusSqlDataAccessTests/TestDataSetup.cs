using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AuxeltusSqlDataAccessTests
{
    internal static class TestDataSetup
    {
        public static List<Location> Locations;
        public static List<Role> Roles;
        public static List<Job> Jobs;

        public static AuxeltusSqlContext GenerateTestDataContext(bool setupData = true)
        {
            DbContextOptions<AuxeltusSqlContext> options = new DbContextOptionsBuilder<AuxeltusSqlContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            AuxeltusSqlContext context = new AuxeltusSqlContext(options);

            context.Database.OpenConnection();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (setupData)
            {
                AddLocationsToContext(context);
                AddRolesToContext(context);
                AddJobsToContext(context);
            }

            return context;
        }

        private static void AddLocationsToContext(AuxeltusSqlContext context)
        {
            Locations = new List<Location>
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
            context.Locations.AddRange(Locations);
            context.SaveChanges();

            Locations = context.Locations.ToListAsync().GetAwaiter().GetResult();
        }

        private static void AddRolesToContext(AuxeltusSqlContext context)
        {
            Roles = new List<Role>
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

            context.Roles.AddRange(Roles);
            context.SaveChanges();

            Roles = context.Roles.ToListAsync().GetAwaiter().GetResult();
        }

        private static void AddJobsToContext(AuxeltusSqlContext context)
        {
            Jobs = new List<Job>
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
                    LocationId = Locations.First().Id,
                    RoleId = Roles[3].Id
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
                    LocationId = Locations.Last().Id,
                    RoleId = Roles.First().Id
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
                    LocationId = Locations.Last().Id,
                    RoleId = Roles[1].Id
                },
                new Job
                {
                    Description = "You can never escape management.",
                    EmployeeId = 3,
                    ReportingEmployeeId = 5,
                    EmployeeType = EmployeeType.FullTime,
                    Remote = true,
                    Salary = 100000,
                    SalaryType = SalaryType.Annual,
                    Archived = false,
                    LocationId = null,
                    RoleId = Roles[5].Id
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
                    LocationId = Locations.Last().Id,
                    RoleId = Roles[5].Id
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
                    LocationId = Locations.Last().Id,
                    RoleId = Roles[5].Id
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
                    LocationId = Locations.Last().Id,
                    RoleId = Roles[2].Id
                },
                new Job
                {
                    Description = "Unfilled remote.",
                    EmployeeId = null,
                    ReportingEmployeeId = 5,
                    EmployeeType = EmployeeType.FullTime,
                    SalaryType = SalaryType.Annual,
                    Remote = true,
                    Salary = null,
                    Archived = false,
                    LocationId = null,
                    RoleId = Roles[1].Id
                },
                new Job
                {
                    Description = "Open but archived.",
                    EmployeeId = null,
                    ReportingEmployeeId = 5,
                    EmployeeType = EmployeeType.FullTime,
                    SalaryType = SalaryType.Annual,
                    Remote = false,
                    Salary = null,
                    Archived = true,
                    LocationId = Locations.Last().Id,
                    RoleId = Roles[5].Id
                },


            };

            context.Jobs.AddRange(Jobs);
            context.SaveChanges();

            Jobs = context.Jobs
                .Include(j => j.Location)
                .Include(j => j.Role)
                .ToList();
        }
    }
}
