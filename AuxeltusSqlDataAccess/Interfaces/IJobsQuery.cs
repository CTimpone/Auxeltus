using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IJobsQuery
    {
        public Task<Job> RetrieveJob(int jobId);
        public Task<List<Job>> RetrieveJobsReportingTo(int employeeId);
        public Task<List<Job>> RetrieveOpenJobs(int? maxReturn, int? startIndex);
    }
}
