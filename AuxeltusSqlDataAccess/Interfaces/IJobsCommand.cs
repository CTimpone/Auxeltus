using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IJobsCommand
    {
        public Task CreateJobAsync(Job job);
        public Task UpdateJobAsync(int jobId, Job job);
        public Task DeleteJobAsync(int jobId);
    }
}
