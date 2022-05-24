using System.Collections.Generic;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IJobsAccessor
    {
        public Job RetrieveJob(int jobId);
        public List<Job> RetrieveJobsReportingTo(int employeeId);
        public void CreateJob(Job job);
        public void UpdateJob(int jobId, Job job);
        public void DeleteJob(int jobId);
    }
}
