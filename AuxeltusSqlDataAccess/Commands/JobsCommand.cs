using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Auxeltus.AccessLayer.Sql
{
    public class JobsCommand : IJobsCommand
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public JobsCommand(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task CreateJobAsync(Job job)
        {
            try
            {
                await _context.Jobs
                    .AddAsync(job)
                    .ConfigureAwait(false);

                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsCommand)}.{nameof(CreateJobAsync)}");
                throw;
            }
        }

        public async Task UpdateJobAsync(int jobId, Job updatedJob)
        {
            try
            {
                Job foundJob = await _context.Jobs
                    .FirstAsync(job => job.Id == jobId)
                    .ConfigureAwait(false);

                foundJob.Mutate(updatedJob);

                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsCommand)}.{nameof(UpdateJobAsync)}");
                throw;
            }
        }

        public async Task DeleteJobAsync(int jobId)
        {
            try
            {
                Job foundJob = await _context.Jobs
                    .FirstAsync(job => job.Id == jobId)
                    .ConfigureAwait(false);

                _context.Remove(foundJob);
                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(JobsCommand)}.{nameof(DeleteJobAsync)}");
                throw;
            }
        }
    }
}
