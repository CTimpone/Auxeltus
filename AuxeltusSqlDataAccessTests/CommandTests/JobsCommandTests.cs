using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuxeltusSqlDataAccessTests
{
    [TestClass]
    public class JobsCommandTests: TestBase
    {
        public AuxeltusSqlContext context;
        public ILogger<JobsCommand> testLogger;
        public IJobsCommand command;

        [TestInitialize]
        public void Initialize()
        {
            context = GenerateTestDataContext();
            testLogger = NullLogger<JobsCommand>.Instance;
            command = new JobsCommand(testLogger, context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Database?.CloseConnection();
            context?.Dispose();
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_JOB_CATEGORY)]
        public async Task CreateJobAsync_Success()
        {
            Random rnd = new Random();
            LocationEntity underlyingLoc = Locations[rnd.Next(0, Locations.Count - 1)];
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];
            JobEntity newJob = new JobEntity
            {
                EmployeeId = rnd.Next(),
                ReportingEmployeeId = rnd.Next(),
                Archived = false,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = underlyingLoc,
                Role = underlyingRole,
                Remote = false,
                Salary = 10000,
                SalaryType = SalaryType.Hourly
            };

            await command.CreateJobAsync(newJob);

            JobEntity addedJob = context.Jobs
                .Include(j => j.Location)
                .Include(j => j.Role)
                .First(j => j.Description == newJob.Description);

            Assert.IsNotNull(addedJob.Id);
            CompareJobs(newJob, addedJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_JOB_CATEGORY)]
        public async Task CreateJobAsync_Success_RemoteNoLocation()
        {
            Random rnd = new Random();
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];
            JobEntity newJob = new JobEntity
            {
                EmployeeId = rnd.Next(),
                ReportingEmployeeId = rnd.Next(),
                Archived = false,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = null,
                Role = underlyingRole,
                Remote = true,
                Salary = 10000,
                SalaryType = SalaryType.Hourly
            };

            await command.CreateJobAsync(newJob);

            JobEntity addedJob = context.Jobs
                .Include(j => j.Location)
                .Include(j => j.Role)
                .First(j => j.Description == newJob.Description);

            Assert.IsNotNull(addedJob.Id);
            CompareJobs(newJob, addedJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_JOB_CATEGORY)]
        public async Task CreateJobAsync_Success_NoEmployeeNoSalary()
        {
            Random rnd = new Random();
            LocationEntity underlyingLoc = Locations[rnd.Next(0, Locations.Count - 1)];
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];
            JobEntity newJob = new JobEntity
            {
                EmployeeId = null,
                ReportingEmployeeId = rnd.Next(),
                Archived = false,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = underlyingLoc,
                Role = underlyingRole,
                Remote = false,
                Salary = null,
                SalaryType = null
            };

            await command.CreateJobAsync(newJob);

            JobEntity addedJob = context.Jobs
                .Include(j => j.Location)
                .Include(j => j.Role)
                .First(j => j.Description == newJob.Description);

            Assert.IsNotNull(addedJob.Id);
            CompareJobs(newJob, addedJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateJobAsync_Error_RemoteWithLocation()
        {
            Random rnd = new Random();
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];
            LocationEntity underlyingLoc = Locations[rnd.Next(0, Locations.Count - 1)];

            JobEntity newJob = new JobEntity
            {
                EmployeeId = rnd.Next(),
                ReportingEmployeeId = rnd.Next(),
                Archived = false,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = underlyingLoc,
                Role = underlyingRole,
                Remote = true,
                Salary = 10000,
                SalaryType = SalaryType.Hourly
            };

            await command.CreateJobAsync(newJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateJobAsync_Error_NotRemoteNoLocation()
        {
            Random rnd = new Random();
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];

            JobEntity newJob = new JobEntity
            {
                EmployeeId = rnd.Next(),
                ReportingEmployeeId = rnd.Next(),
                Archived = false,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = null,
                Role = underlyingRole,
                Remote = false,
                Salary = 10000,
                SalaryType = SalaryType.Hourly
            };

            await command.CreateJobAsync(newJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateJobAsync_Error_FilledPositionNoSalary()
        {
            Random rnd = new Random();
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];
            LocationEntity underlyingLoc = Locations[rnd.Next(0, Locations.Count - 1)];

            JobEntity newJob = new JobEntity
            {
                EmployeeId = rnd.Next(),
                ReportingEmployeeId = rnd.Next(),
                Archived = false,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = underlyingLoc,
                Role = underlyingRole,
                Remote = true,
                Salary = null,
                SalaryType = SalaryType.Hourly
            };

            await command.CreateJobAsync(newJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateJobAsync_Error_UnilledPositionWithSalary()
        {
            Random rnd = new Random();
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];
            LocationEntity underlyingLoc = Locations[rnd.Next(0, Locations.Count - 1)];

            JobEntity newJob = new JobEntity
            {
                EmployeeId = null,
                ReportingEmployeeId = rnd.Next(),
                Archived = false,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = underlyingLoc,
                Role = underlyingRole,
                Remote = true,
                Salary = 111,
                SalaryType = SalaryType.Hourly
            };

            await command.CreateJobAsync(newJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.CREATE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task CreateJobAsync_Error_EFCoreContextNull()
        {
            Random rnd = new Random();
            LocationEntity underlyingLoc = Locations[rnd.Next(0, Locations.Count - 1)];
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];
            JobEntity newJob = new JobEntity
            {
                EmployeeId = rnd.Next(),
                ReportingEmployeeId = rnd.Next(),
                Archived = false,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = underlyingLoc,
                Role = underlyingRole,
                Remote = false,
                Salary = 10000,
                SalaryType = SalaryType.Hourly
            };

            int initialCount = Jobs.Count;

            command = new JobsCommand(testLogger, null);

            await command.CreateJobAsync(newJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_JOB_CATEGORY)]
        public async Task DeleteJobAsync_Success()
        {
            int initialCount = Jobs.Count;

            JobEntity toBeDeleted = Jobs[4];
            await command.DeleteJobAsync(toBeDeleted.Id);

            Assert.IsFalse(context.Jobs.Any(j => j.Id == toBeDeleted.Id));
            Assert.AreEqual(initialCount - 1, context.Jobs.Count());
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task DeleteJobAsync_Error_IdDoesNotExist()
        {
            int initialCount = Jobs.Count;

            await command.DeleteJobAsync(initialCount + 1000);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.DELETE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task DeleteJobAsync_Error_EFCoreContextNull()
        {
            command = new JobsCommand(testLogger, null);

            await command.DeleteJobAsync(1);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_JOB_CATEGORY)]
        public async Task UpdateJobAsync_AllFields()
        {
            Random rnd = new Random();

            int initialCount = Jobs.Count;
            JobEntity toBeUpdated = Jobs[rnd.Next(0, initialCount - 1)];

            LocationEntity underlyingLoc = Locations[rnd.Next(0, Locations.Count - 1)];
            RoleEntity underlyingRole = Roles[rnd.Next(0, Locations.Count - 1)];

            JobEntity jobUpdate = new JobEntity
            {
                EmployeeId = rnd.Next(),
                ReportingEmployeeId = rnd.Next(),
                Archived = !toBeUpdated.Archived,
                Description = Guid.NewGuid().ToString("N"),
                EmployeeType = EmployeeType.FullTime,
                Location = underlyingLoc,
                Role = underlyingRole,
                Remote = false,
                Salary = 10000,
                SalaryType = SalaryType.Hourly
            };


            await command.UpdateJobAsync(toBeUpdated.Id, jobUpdate);

            JobEntity finalJob = context.Jobs
                .Include(j => j.Location)
                .Include(j => j.Role)
                .First(j => j.Description == jobUpdate.Description);

            Assert.AreEqual(toBeUpdated.Id, finalJob.Id);
            CompareJobs(jobUpdate, finalJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_JOB_CATEGORY)]
        public async Task UpdateJobAsync_OnlyDescription()
        {
            Random rnd = new Random();

            int initialCount = Jobs.Count;
            JobEntity toBeUpdated = Jobs[rnd.Next(0, initialCount - 1)];

            JobEntity jobUpdate = new JobEntity
            {
                Description = Guid.NewGuid().ToString("N"),
            };

            await command.UpdateJobAsync(toBeUpdated.Id, jobUpdate);

            JobEntity finalJob = context.Jobs.First(j => j.Description == jobUpdate.Description);

            JobEntity expected = new JobEntity
            {
                Description = jobUpdate.Description,
                EmployeeId = toBeUpdated.EmployeeId,
                ReportingEmployeeId = toBeUpdated.ReportingEmployeeId,
                Archived = toBeUpdated.Archived,
                EmployeeType = toBeUpdated.EmployeeType,
                Location = toBeUpdated.Location,
                Role = toBeUpdated.Role,
                Remote = toBeUpdated.Remote,
                Salary = toBeUpdated.Salary,
                SalaryType = toBeUpdated.SalaryType
            };

            Assert.AreEqual(toBeUpdated.Id, finalJob.Id);
            CompareJobs(expected, finalJob);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task UpdateJobAsync_Error_IdNotFound()
        {
            int initialCount = Jobs.Count;

            JobEntity jobUpdate = new JobEntity
            {
                Description = Guid.NewGuid().ToString("N"),
            };

            await command.UpdateJobAsync(initialCount + 1000, jobUpdate);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.UPDATE_JOB_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task UpdateJobAsync_Error_EFCoreContextNull()
        {
            JobEntity jobUpdate = new JobEntity
            {
                Description = Guid.NewGuid().ToString("N"),
            };

            command = new JobsCommand(testLogger, null);
            await command.UpdateJobAsync(1, jobUpdate);
        }
    }
}
