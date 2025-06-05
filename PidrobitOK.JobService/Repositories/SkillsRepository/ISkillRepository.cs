using JobService.Models;

namespace JobService.Repositories
{
    public interface ISkillRepository
    {
        public Task<SkillDto> Retrieve(Guid skillId);
        public Task<List<SkillDto>> Retrieve();
        Task<SkillDto> Insert(SkillDto skill);
        Task<SkillDto> Update(SkillDto skill);
        Task<bool> Delete(Guid skillId);
        Task<bool> ExistsByName(string name);
    }
}
