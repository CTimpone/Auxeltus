using System.Collections.Generic;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IJobsQuery
    {
        public Job RetrieveJob(int jobId);
        public List<Job> RetrieveJobsReportingTo(int employeeId);
    }
}
