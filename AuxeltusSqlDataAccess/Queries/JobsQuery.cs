using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public class JobsQuery : IJobsQuery
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public JobsQuery(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Job> RetrieveJob(int jobId)
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
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsQuery)}.{nameof(RetrieveJob)}");
                throw;
            }
        }

        public async Task<List<Job>> RetrieveJobsReportingTo(int employeeId)
        {
            try
            {
                List<Job> reports = await _context.Jobs
                    .AsNoTracking()
                    .Where(job => job.ReportingEmployeeId == employeeId)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return reports;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsQuery)}.{nameof(RetrieveJob)}");
                throw;
            }
        }
        public async Task<List<Job>> RetrieveOpenJobs(int? maxReturns, int? startIndex)
        {
            maxReturns ??= 25;
            startIndex ??= 0;

            try
            {
                List<Job> reports = await _context.Jobs
                    .AsNoTracking()
                    .Where(job => job.EmployeeId == null && job.Id > startIndex)
                    .OrderBy(job => job.Id)
                    .Take(maxReturns.Value)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return reports;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsQuery)}.{nameof(RetrieveJob)}");
                throw;
            }
        }

    }
}
