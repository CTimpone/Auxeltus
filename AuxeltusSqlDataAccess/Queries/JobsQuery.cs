using System;
using System.Collections.Generic;

namespace Auxeltus.AccessLayer.Sql
{
    public class JobsQuery : IJobsQuery
    {
        public Job RetrieveJob(int jobId)
        {
            throw new NotImplementedException();
        }
        public List<Job> RetrieveJobsReportingTo(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
