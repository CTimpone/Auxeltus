using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IJobsQuery
    {
        public async Task<Job> RetrieveJob(int jobId);
        public async Task<List<Job>> RetrieveJobsReportingTo(int employeeId);
    }
}
