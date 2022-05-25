using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// Class <c>JobsQuery</c> allows for the retrieval of <c>Job</c>s from the Auxeltus SQL data model.
    /// It utilizes asynchronous EF Core methodology to retrieve <c>Job</c>s from the SQL database.
    /// </summary>
    public class JobsQuery : IJobsQuery
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public JobsQuery(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// It utilizes asynchronous EF Core methodology to retrieve a <c>Job</c>s from the SQL database using the primary key.
        /// </summary>
        public async Task<Job> RetrieveJobAsync(int jobId)
        {
            try
            {
                Job foundJob = await _context.Jobs
                    .AsNoTracking()
                    .FirstAsync(job => job.Id == jobId)
                    .ConfigureAwait(false);

                return foundJob;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsQuery)}.{nameof(RetrieveJobAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all <c>Job</c>s that report to a specific employee.
        /// </summary>
        public async Task<List<Job>> RetrieveJobsReportingToAsync(int employeeId)
        {
            try
            {
                List<Job> reports = await _context.Jobs
                    .AsNoTracking()
                    .Where(job => job.ReportingEmployeeId == employeeId && job.Archived != true)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return reports;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsQuery)}.{nameof(RetrieveJobsReportingToAsync)}");
                throw;
            }
        }

        /// <summary>
        /// It utilizes asynchronous EF Core methodology to retrieve <c>Job</c>s from the SQL database.
        /// Will only return <c>Job</c>s without a corresponding <c>Employee</c> that are not archived.
        /// Only returns a subset (default of 25) of found <c>Job</c>s, based on the <c>maxReturns</c> parameter.
        /// The <c>startIndex</c> parameter can be used to facilitate paging.
        /// </summary>
        public async Task<List<Job>> RetrieveOpenJobsAsync(int? maxReturns, int? startIndex)
        {
            maxReturns ??= 25;
            startIndex ??= 0;

            try
            {
                List<Job> reports = await _context.Jobs
                    .AsNoTracking()
                    .Where(job => job.EmployeeId == null && job.Id > startIndex && job.Archived != true)
                    .OrderBy(job => job.Id)
                    .Take(maxReturns.Value)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return reports;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsQuery)}.{nameof(RetrieveOpenJobsAsync)}");
                throw;
            }
        }

    }
}
