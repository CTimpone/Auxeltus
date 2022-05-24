using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IJobsQuery
    {
        public Task<Job> RetrieveJobAsync(int jobId);
        public Task<List<Job>> RetrieveJobsReportingToAsync(int employeeId);
        public Task<List<Job>> RetrieveOpenJobsAsync(int? maxReturn, int? startIndex);
    }
}
