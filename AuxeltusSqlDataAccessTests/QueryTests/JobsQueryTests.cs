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
    public class JobsQueryTests: TestBase
    {
        public AuxeltusSqlContext context;
        public ILogger testLogger;
        public IJobsQuery query;

        [TestInitialize]
        public void Initialize()
        {
            context = GenerateTestDataContext();
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
            JobEntity initialJob = Jobs.First(x => x.Description.Equals("A job without peer!"));
            JobEntity job = await query.RetrieveJobAsync(initialJob.Id);

            Assert.IsNotNull(job);
            Assert.IsNotNull(job.LocationId);
            Assert.IsNotNull(job.RoleId);

            CompareJobs(initialJob, job);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        public async Task RetrieveJobAsync_Remote_NoLocation()
        {
            JobEntity initialJob = Jobs.First(x => x.Description.Equals("You can never escape management."));
            JobEntity job = await query.RetrieveJobAsync(initialJob.Id);

            Assert.IsNotNull(job);
            Assert.IsNull(job.LocationId);
            Assert.IsNotNull(job.RoleId);

            CompareJobs(initialJob, job);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        public async Task RetrieveJobAsync_UnfilledPosition()
        {
            JobEntity initialJob = Jobs.First(x => x.Description.Equals("A job in search of an employee!"));
            JobEntity job = await query.RetrieveJobAsync(initialJob.Id);

            Assert.IsNotNull(job);

            CompareJobs(initialJob, job);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        public async Task RetrieveJobAsync_UnfilledRemotePosition()
        {
            JobEntity initialJob = Jobs.First(x => x.Description.Equals("Unfilled remote."));
            JobEntity job = await query.RetrieveJobAsync(initialJob.Id);

            Assert.IsNotNull(job);
            Assert.IsNull(job.LocationId);

            CompareJobs(initialJob, job);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task RetrieveJobAsync_Error_JobIdDoesNotEist()
        {
            await query.RetrieveJobAsync(9999);

            //Should fail prior to reaching this, resulting in the ExpectedException 
            Assert.Fail();
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_JOB_ASYNC_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task RetrieveJobAsync_Error_EFCoreContextThrows()
        {
            query = new JobsQuery(testLogger, null);
            await query.RetrieveJobAsync(1);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_OPEN_JOBS_CATEGORY)]
        public async Task RetrieveOpenJobsAsync_RetrieveAllQualifyingRecords()
        {
            int maxReturns = 100;
            int startIndex = 0;
            List<JobEntity> jobs = await query.RetrieveOpenJobsAsync(maxReturns, startIndex);

            List<JobEntity> possibleJobs = Jobs
                .Where(j => j.EmployeeId == null && j.Archived == false)
                .ToList();
            
            Assert.AreEqual(possibleJobs.Count, jobs.Count);
            Assert.IsTrue(jobs.All(j => j.EmployeeId == null && j.Archived == false));

            foreach (JobEntity job in jobs)
            {
                JobEntity match = Jobs.First(j => j.Id == job.Id);
                CompareJobs(match, job);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_OPEN_JOBS_CATEGORY)]
        public async Task RetrieveOpenJobsAsync_RetrieveSubset_UpToMaxReturns()
        {
            int maxReturns = 2;
            int startIndex = 0;
            List<JobEntity> jobs = await query.RetrieveOpenJobsAsync(maxReturns, startIndex);

            Assert.AreEqual(2, jobs.Count);
            Assert.IsTrue(jobs.All(j => j.EmployeeId == null && j.Archived == false));

            foreach (JobEntity job in jobs)
            {
                JobEntity match = Jobs.First(j => j.Id == job.Id);
                CompareJobs(match, job);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_OPEN_JOBS_CATEGORY)]
        public async Task RetrieveOpenJobsAsync_RetrieveSubset_GreaterThanStartIndex()
        {
            int maxReturns = 100;
            int startIndex = 2;
            List<JobEntity> jobs = await query.RetrieveOpenJobsAsync(maxReturns, startIndex);

            Assert.AreEqual(1, jobs.Count);
            Assert.IsTrue(jobs.All(j => j.EmployeeId == null && j.Archived == false && j.Id >= startIndex));

            foreach (JobEntity job in jobs)
            {
                JobEntity match = Jobs.First(j => j.Id == job.Id);
                CompareJobs(match, job);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_OPEN_JOBS_CATEGORY)]
        public async Task RetrieveOpenJobsAsync_NoData_StartIndexTooGreat()
        {
            int maxReturns = 100;
            int startIndex = 100;
            List<JobEntity> jobs = await query.RetrieveOpenJobsAsync(maxReturns, startIndex);

            Assert.AreEqual(0, jobs.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_OPEN_JOBS_CATEGORY)]
        public async Task RetrieveOpenJobsAsync_NoData_NoDataOnDatabase()
        {
            context.Jobs.RemoveRange(Jobs);
            context.SaveChanges();

            int maxReturns = 100;
            int startIndex = 0;
            List<JobEntity> jobs = await query.RetrieveOpenJobsAsync(maxReturns, startIndex);

            Assert.AreEqual(0, jobs.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_OPEN_JOBS_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task RetrieveOpenJobsAsync_Error_EFCoreContextThrows()
        {
            query = new JobsQuery(testLogger, null);
            await query.RetrieveOpenJobsAsync(null, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_REPORTING_JOBS_CATEGORY)]
        public async Task RetrieveJobsReportingToAsync_RetrieveAllJobsReportingToSingleEmployee()
        {
            int reportingEmployeeId = 3;
            List<JobEntity> jobs = await query.RetrieveJobsReportingToAsync(reportingEmployeeId);

            Assert.IsTrue(jobs.All(j => j.ReportingEmployeeId == reportingEmployeeId && j.Archived == false));

            foreach (JobEntity job in jobs)
            {
                JobEntity match = Jobs.First(j => j.Id == job.Id);
                CompareJobs(match, job);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_REPORTING_JOBS_CATEGORY)]
        public async Task RetrieveJobsReportingToAsync_RetrieveAllJobsReportingToSingleEmployee_NotArchived()
        {
            int reportingEmployeeId = 5;
            List<JobEntity> jobs = await query.RetrieveJobsReportingToAsync(reportingEmployeeId);

            Assert.IsTrue(jobs.All(j => j.ReportingEmployeeId == reportingEmployeeId && j.Archived == false));

            Assert.AreNotEqual(Jobs.Where(j => j.ReportingEmployeeId == reportingEmployeeId).Count(),
                jobs.Count);

            foreach (JobEntity job in jobs)
            {
                JobEntity match = Jobs.First(j => j.Id == job.Id);
                CompareJobs(match, job);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_REPORTING_JOBS_CATEGORY)]
        public async Task RetrieveJobsReportingToAsync_NoMatchOnReportingEmployeeId_NoData()
        {
            int reportingEmployeeId = 10000;
            List<JobEntity> jobs = await query.RetrieveJobsReportingToAsync(reportingEmployeeId);

            Assert.AreEqual(0, jobs.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryConstants.RETRIEVE_REPORTING_JOBS_CATEGORY)]
        [ExpectedException(typeof(AuxeltusSqlException))]
        public async Task RetrieveJobsReportingToAsync_Error_EFCoreContextThrows()
        {
            query = new JobsQuery(testLogger, null);
            await query.RetrieveJobsReportingToAsync(3);
        }
    }
}