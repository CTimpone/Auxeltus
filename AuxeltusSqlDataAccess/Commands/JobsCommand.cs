using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// Class <c>JobsCommand</c> allows for the modification of the Auxeltus SQL data model, specifically with regards to <c>Job</c>.
    /// It utilizes asynchronous EF Core methodology to add, update, and delete items from the SQL database.
    /// </summary>
    public class JobsCommand : IJobsCommand
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public JobsCommand(ILogger<JobsCommand> logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Adds a new <c>Job</c> to the data structure.
        /// Input object should not include an Id (as it is auto-generated as the primary key).
        /// </summary>
        public async Task CreateJobAsync(JobEntity job)
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
                string message = $"Exception thrown in {nameof(JobsCommand)}.{nameof(CreateJobAsync)}";
                _logger.LogError(ex, message);
                throw new AuxeltusSqlException(message, ex);
            }
        }

        /// <summary>
        /// Updates an existing <c>Job</c> on the data structure.
        /// </summary>
        public async Task UpdateJobAsync(int jobId, JobEntity updatedJob)
        {
            try
            {
                JobEntity foundJob = await _context.Jobs
                    .FirstAsync(job => job.Id == jobId)
                    .ConfigureAwait(false);

                foundJob.Mutate(updatedJob);

                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string message = $"Exception thrown in {nameof(JobsCommand)}.{nameof(UpdateJobAsync)}";
                _logger.LogError(ex, message);
                throw new AuxeltusSqlException(message, ex);
            }
        }

        /// <summary>
        /// Deletes an existing <c>Location</c> on the data structure.
        /// </summary>
        public async Task DeleteJobAsync(int jobId)
        {
            try
            {
                JobEntity foundJob = await _context.Jobs
                    .FirstAsync(job => job.Id == jobId)
                    .ConfigureAwait(false);

                _context.Remove(foundJob);
                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string message = $"Exception thrown in {nameof(JobsCommand)}.{nameof(DeleteJobAsync)}";
                _logger.LogError(ex, message);
                throw new AuxeltusSqlException(message, ex);
            }
        }
    }
}
