using Auxeltus.AccessLayer.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuxeltusSqlDataAccessTests
{
    [TestClass]
    public class JobsQueryTests
    {
        public AuxeltusSqlContext context;
        public ILogger testLogger;
        public IJobsQuery query;

        [TestInitialize]
        public void Initialize()
        {
            context = TestDataSetup.GenerateTestDataContext();
            testLogger = NullLogger.Instance;
            query = new JobsQuery(testLogger, context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Database?.CloseConnection();
            context?.Dispose();
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        public async Task RetrieveJobAsync_RoleAndLocationIncluded()
        {
            Job initialJob = TestDataSetup.Jobs.First(x => x.Description.Equals("A job without peer!"));
            Job job = await query.RetrieveJobAsync(initialJob.Id);

            Assert.IsNotNull(job);
            Assert.IsNotNull(job.LocationId);
            Assert.IsNotNull(job.RoleId);

            CompareJobs(initialJob, job);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        public async Task RetrieveJobAsync_Remote_NoLocation()
        {
            Job initialJob = TestDataSetup.Jobs.First(x => x.Description.Equals("You can never escape management."));
            Job job = await query.RetrieveJobAsync(initialJob.Id);

            Assert.IsNotNull(job);
            Assert.IsNull(job.LocationId);
            Assert.IsNotNull(job.RoleId);

            CompareJobs(initialJob, job);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        public async Task RetrieveJobAsync_UnfilledPosition()
        {
            Job initialJob = TestDataSetup.Jobs.First(x => x.Description.Equals("A job in search of an employee!"));
            Job job = await query.RetrieveJobAsync(initialJob.Id);

            Assert.IsNotNull(job);

            CompareJobs(initialJob, job);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        public async Task RetrieveJobAsync_UnfilledRemotePosition()
        {
            Job initialJob = TestDataSetup.Jobs.First(x => x.Description.Equals("Unfilled remote."));
            Job job = await query.RetrieveJobAsync(initialJob.Id);

            Assert.IsNotNull(job);
            Assert.IsNull(job.LocationId);

            CompareJobs(initialJob, job);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task RetrieveJobAsync_Error_JobIdDoesNotEist()
        {
            Job job = await query.RetrieveJobAsync(9999);
        }



        private void CompareJobs(Job expected, Job actual)
        {
            Assert.AreEqual(expected.Salary, actual.Salary);
            Assert.AreEqual(expected.SalaryType, actual.SalaryType);
            Assert.AreEqual(expected.Archived, actual.Archived);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.ReportingEmployeeId, actual.ReportingEmployeeId);

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
            } else
            {
                Assert.IsFalse(actual.Remote);
                Assert.IsNotNull(actual.Location);
                Assert.IsNotNull(actual.LocationId);
                Assert.AreEqual(expected.Location.Id, actual.Location.Id);
                Assert.AreEqual(expected.Location.Name, actual.Location.Name);
                Assert.AreEqual(expected.Location.Longitude, actual.Location.Longitude);
                Assert.AreEqual(expected.Location.Latitude, actual.Location.Latitude);
            }

            Assert.AreEqual(expected.Role.Title, actual.Role.Title);
            Assert.AreEqual(expected.Role.MaximumSalary, actual.Role.MaximumSalary);
            Assert.AreEqual(expected.Role.MinimumSalary, actual.Role.MinimumSalary);
            Assert.AreEqual(expected.Role.Tier, actual.Role.Tier);
        }
    }
}
