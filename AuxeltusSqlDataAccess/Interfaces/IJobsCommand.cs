namespace Auxeltus.AccessLayer.Sql
{
    public interface IJobsCommand
    {
        public void CreateJob(Job job);
        public void UpdateJob(int jobId, Job job);
        public void DeleteJob(int jobId);
    }
}
