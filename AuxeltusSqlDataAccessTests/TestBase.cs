using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AuxeltusSqlDataAccessTests
{
    public class TestBase
    {
        public List<LocationEntity> Locations = new List<LocationEntity>();
        public List<RoleEntity> Roles = new List<RoleEntity>();
        public List<JobEntity> Jobs = new List<JobEntity>();

        public void CompareRoles(RoleEntity expected, RoleEntity actual)
        {
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Tier, actual.Tier);
            Assert.AreEqual(expected.MaximumSalary, actual.MaximumSalary);
            Assert.AreEqual(expected.MinimumSalary, actual.MinimumSalary);
        }

        public void CompareLocations(LocationEntity expected, LocationEntity actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Longitude, actual.Longitude);
            Assert.AreEqual(expected.Latitude, actual.Latitude);
        }

        public void CompareJobs(JobEntity expected, JobEntity actual)
        {
            Assert.AreEqual(expected.Salary, actual.Salary);
            Assert.AreEqual(expected.SalaryType, actual.SalaryType);
            Assert.AreEqual(expected.Archived, actual.Archived);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.ReportingEmployeeId, actual.ReportingEmployeeId);

            CompareRoles(expected.Role, actual.Role);

            if (expected.EmployeeId == null)
            {
                Assert.IsNull(actual.EmployeeId);

            }
            else
            {
                Assert.AreEqual(expected.EmployeeType, actual.EmployeeType);
                Assert.AreEqual(expected.EmployeeId, actual.EmployeeId);
            }

            if (expected.Remote == true)
            {
                Assert.IsTrue(actual.Remote);
                Assert.IsNull(actual.Location);
                Assert.IsNull(actual.LocationId);
            }
            else
            {
                Assert.IsFalse(actual.Remote);
                Assert.IsNotNull(actual.Location);
                Assert.IsNotNull(actual.LocationId);
                Assert.AreEqual(expected.Location.Id, actual.Location.Id);
                CompareLocations(expected.Location, actual.Location);
            }
        }

        public AuxeltusSqlContext GenerateTestDataContext(bool setupData = true)
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

        private void AddLocationsToContext(AuxeltusSqlContext context)
        {
            Locations = new List<LocationEntity>
            {
                new LocationEntity
                {
                    Name = "Columbus, Ohio",
                    Latitude = 40.03261910026816,
                    Longitude = -83.03349425205028
                },
                new LocationEntity
                {
                    Name = "Evanston, Illinois",
                    Latitude = 42.05796965571029,
                    Longitude = -87.67593067172223
                },
                new LocationEntity
                {
                    Name = "Chennai, India",
                    Latitude = 13.07149818062577,
                    Longitude = 80.25647616462626
                },
                new LocationEntity
                {
                    Name = "North Vegas",
                    Latitude = 36.20616312689968,
                    Longitude = -115.11229175325266
                },
                new LocationEntity
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

        private void AddRolesToContext(AuxeltusSqlContext context)
        {
            Roles = new List<RoleEntity>
            {
                new RoleEntity
                {
                    Title = "Software Developer",
                    Tier = 2,
                    MinimumSalary = 75000,
                    MaximumSalary = 120000
                },
                new RoleEntity
                {
                    Title = "Senior Software Developer",
                    Tier = 3,
                    MinimumSalary = 105000,
                    MaximumSalary = 150000
                },
                new RoleEntity
                {
                    Title = "Super Senior Software Developer",
                    Tier = 4,
                    MinimumSalary = 137293,
                    MaximumSalary = 215247
                },
                new RoleEntity
                {
                    Title = "Custodian",
                    Tier = 2,
                    MinimumSalary = 85000,
                    MaximumSalary = 85000
                },
                new RoleEntity
                {
                    Title = "Junior Software Developer",
                    Tier = 1,
                    MinimumSalary = 60000,
                    MaximumSalary = 80000
                },
                new RoleEntity
                {
                    Title = "Manager",
                    Tier = 3,
                    MinimumSalary = 95000,
                    MaximumSalary = 135000
                },
                new RoleEntity
                {
                    Title = "Super Manager Deluxe with Cheese",
                    Tier = 4,
                    MinimumSalary = 127500,
                    MaximumSalary = 152500
                },
                new RoleEntity
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

        private void AddJobsToContext(AuxeltusSqlContext context)
        {
            Jobs = new List<JobEntity>
            {
                new JobEntity
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
                new JobEntity
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
                new JobEntity
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
                new JobEntity
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
                new JobEntity
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
                new JobEntity
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
                new JobEntity
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
                new JobEntity
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
                new JobEntity
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
                new JobEntity
                {
                    Description = "Archived but reporting.",
                    EmployeeId = 10,
                    ReportingEmployeeId = 5,
                    EmployeeType = EmployeeType.FullTime,
                    SalaryType = SalaryType.Annual,
                    Remote = false,
                    Salary = 84000,
                    Archived = true,
                    LocationId = Locations.Last().Id,
                    RoleId = Roles[1].Id
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
