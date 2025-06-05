using JobService.Models;

namespace JobService.Repositories.JobRequiredSkillsRepository
{
    public interface IJobRequiredSkillRepository
    {
        Task<List<JobRequiredSkillDto>> Retrieve();
        Task<JobRequiredSkillDto> Insert(JobRequiredSkillDto entity);
        Task<bool> Delete(Guid id);
        Task<List<SkillDto>> GetSkillsByJobId(Guid jobId);
        Task<List<JobDto>> GetJobsBySkillId(Guid skillId);
        Task<bool> Exists(Guid jobId, Guid skillId);
    }
}