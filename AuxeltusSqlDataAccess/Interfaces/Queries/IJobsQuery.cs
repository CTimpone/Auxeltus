using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// The <c>IJobsQuery</c> interface provides an outline for the retrieval of <c>Job</c>s from the Auxeltus data model.
    /// </summary>
    public interface IJobsQuery
    {
        /// <summary>
        /// Retrieves a <c>Job</c> using the given primaryId of the object.
        /// </summary>
        public Task<JobEntity> RetrieveJobAsync(int jobId);

        /// <summary>
        /// Retrieves all <c>Job</c>s that report to the same <c>Employee</c>.
        /// </summary>
        public Task<List<JobEntity>> RetrieveJobsReportingToAsync(int employeeId);

        /// <summary>
        /// Retrieves a set of <c>Job</c>s that are active and do not have an employee set.
        /// </summary>

        public Task<List<JobEntity>> RetrieveOpenJobsAsync(int? maxReturn, int? startIndex);
    }
}
