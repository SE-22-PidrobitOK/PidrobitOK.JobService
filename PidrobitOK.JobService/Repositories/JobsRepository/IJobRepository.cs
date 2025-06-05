using JobService.Models;

namespace JobService.Repositories.JobsRepository
{
    public interface IJobRepository
    {
        Task<JobDto> Retrieve(Guid id);
        Task<List<JobDto>> Retrieve();
        Task<JobDto> Insert(JobDto job);
        Task<JobDto> Update(JobDto job);
        Task<bool> Delete(Guid id);
    }
}
