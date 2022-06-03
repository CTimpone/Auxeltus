using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// The <c>IJobsCommand</c> interface provides an outline for the modification of the Auxeltus data model, specifically with regards to <c>Job</c> model.
    /// </summary>
    public interface IJobsCommand
    {
        /// <summary>
        /// Adds a new <c>Job</c> to the data structure.
        /// Input object should not include an Id (as it is auto-generated as the primary key).
        /// </summary>
        public Task CreateJobAsync(JobEntity job);

        /// <summary>
        /// Updates an existing <c>Job</c> on the data structure.
        /// </summary>
        public Task UpdateJobAsync(int jobId, JobEntity job);

        /// <summary>
        /// Updates an existing <c>Job</c> on the data structure.
        /// </summary>
        public Task DeleteJobAsync(int jobId);
    }
}
