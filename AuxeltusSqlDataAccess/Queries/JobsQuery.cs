using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
    }
}
