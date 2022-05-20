using System.Collections.Generic;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IJobsAccessor
    {
        public Job GetJob(int jobId);
        public List<Job> RetrieveJobsReportingTo(int employeeId);
        public void CreateJob(Job job);
        public void UpdateJob(int jobId, Job job);
    }
}
